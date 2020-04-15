-- last 2019-08-05

local TIMER_WHEEL_SCALE = 100 -- 进制：每个轮子上刻度的个数
local TIMER_WHEEL_COUNT	= 4 -- 轮子数:  100^4 次,如果最小精度是100ms,则最长定时时间10,000,000秒 7.6年

-- ------------------------------------------------------------------------- --

local Print = function(...)
    local info = debug.getinfo( 2, "nSl")
    local str = ""
    if info then
        local source = info.source or ""
        local last = source
        local pattern = string.format("([^%s]+)", "/")
        source:gsub(pattern, function(c) last = c end)
        str = string.format(" [%s:%d]",last, info.currentline)
    end
    print("===lua=== " .. os.date("%H:%M:%S", os.time()) .. str,...)
end

-- -------------------------------------------------------------------------------------- --
local _wheel=class()
function _wheel:Ctor(scale)
    self.m_slots = {}
    self.m_pointer = 0      -- (0 ~ 99)
    self.m_scale = scale    -- (1,100, 10000, 1000000)
end
-- 转动一个刻度，剔除本轮子超时的一个列表
function _wheel:_turn()
    local n = self.m_pointer
    n =  (n+1) % TIMER_WHEEL_SCALE

    local result = self.m_slots[n]
    self.m_slots[n] = nil
    self.m_pointer = n
    return result, (0 == n) -- 是否需要进制
end

function _wheel:_add_tooth(mc, node)

    local max = self.m_scale * TIMER_WHEEL_SCALE
    if mc >= max then -- 时间没落在本轮子上
        return false
    end
  
    local up = math.floor(mc / self.m_scale)
    local place = up % TIMER_WHEEL_SCALE -- 放入的位置
    local remain = mc % self.m_scale

    node.m_remain_tick = remain  -- 剩余时间(ms)
-- Print(mc,"*(100ms) node push to", self.m_scale," wheel, place:", place,'remain:', remain, "pointer:", self.m_pointer)
    node:_set_next(self.m_slots[place])
    self.m_slots[place] = node
    return true
end

-- ------------------------------------------------------------------------- --

local _tooth = class()
function _tooth:Ctor(idx, intervalTick, cb, repeat_exec, ...)
        self.m_handler_idx = idx
        self.m_remain_tick = intervalTick
        self.m_raw_interval = intervalTick  -- 原参数
        self.m_call_back = cb
        self.m_repeat = repeat_exec
        self.m_args = {...}
end

function _tooth:_get_next()
    return self.m_next
end

function _tooth:_set_next(next)
    self.m_next = next
end

function _tooth:_operator()
    if self.m_call_back then
        local again = self.m_call_back(self.m_handler_idx, self, unpack(self.m_args))
        if not self.m_repeat then
            return false
        end

        return again == nil
    end

    return false
end

function _tooth:HandlerIdx()
    return self.m_handler_idx
end

-- public
function _tooth:Cancel()
    self.m_call_back = nil
    self.m_args = nil
end

-- ------------------------------------------------------------------------- --

local _clock = class()
local _handler_idx = 100


-- 外部时钟更新
local function Update(self)
    
    if self.m_pause then
        return
    end
    self.m_loop_count = self.m_loop_count + 1

    for i = 1, TIMER_WHEEL_COUNT do -- 转动时间轮
        local head, over = self.m_scalewheels[i]:_turn()
        self:_carry(head)
        if not over then
            break
        end
    end
end

function _clock:Ctor(delay)
    self.m_scalewheels  = {}
    self.m_head = nil
    self.m_tail = nil
    self.m_delay = delay or 1
    self.m_pause = false
    self.m_loop_count = 0
    local scale = 1
    for _ = 1,TIMER_WHEEL_COUNT do
        table.insert(self.m_scalewheels, _wheel.new(scale))
        scale = scale * TIMER_WHEEL_SCALE
    end
    self.timer = CTimer.new(self.m_delay, 0, function() Update(self) return true end)
end



-- timeout,弹出一个事件
function _clock:_provide(node)
    if node:_operator() then -- again
        self:_append(node) -- 重新加入
        return true
    end
    return false
end

-- 到点的齿轮上的事件，重新分布到轮子上
function _clock:_carry(head)
    while (head) do
        local next = head:_get_next()
        local mc = head.m_remain_tick
        if mc <= 0 then
            if not self:_provide(head) then
                head = nil
            end
        else
            local relative= 0
            for i = 1, TIMER_WHEEL_COUNT do
                local wheel= self.m_scalewheels[i]
                relative = relative + wheel.m_pointer * wheel.m_scale
                if self.m_scalewheels[i]:_add_tooth(mc+relative, head) then
                    break
                end
            end
        end
        head = next
    end
end

-- 以原设定的时间，重新加入到时间轮中
function _clock:_append(node)
    local mc = node.m_raw_interval
    if mc > 0 then
        local relative= 0
        for i = 1, TIMER_WHEEL_COUNT do
            local wheel= self.m_scalewheels[i]
            relative = relative + wheel.m_pointer * wheel.m_scale
            if self.m_scalewheels[i]:_add_tooth(mc+relative, node) then
                return true
            end
        end
    end
    return false
end


-- published
function _clock:AddTimer(ms, cb, repeat_exec, ...)
    if  type(cb) ~= "function" then
        Print("callback error")
        return -1, nil
    end
    if ms < 0 then ms = 1 end

    _handler_idx = _handler_idx + 1 -- timer id,为了兼容和日志
    local tooth = _tooth.new(_handler_idx, math.ceil(ms / self.m_delay), cb, repeat_exec, ...) -- 新建一个节点

    if self:_append(tooth) then
        return _handler_idx, tooth
    else
        return -1, nil
    end
end

function _clock:SetPause(bool)
    self.m_pause = bool
end
-- published
function _clock:NewTicker(ms, cb, ...)
    return self:AddTimer(ms, cb, true, ...)
end

-- published
function _clock:After(ms, cb, ...)
    return self:AddTimer(ms, cb, false, ...)
end
-- return millisecond
function _clock:GetTime()
   return self.m_loop_count * self.m_delay
end

-- 构建
function CreateTimeWheel(delay)
    local c = _clock.new(delay)
    return c
end
