require("luapb")        -- CProtobuf
GlobalPBC = CProtobuf.new("./proto/game.proto")
--GlobalPBC = require "libs.pbcodec"

function Encode_proto_message(proto, data)
    local data = GlobalPBC:encode(proto, data)
    if data then
        return GlobalPBC:encode("YiQuJiangHuNetData.MsgHead", {proto = proto, data = data})
    else
        Logger:error("build failed. not found " .. proto)
    end
end

function Decode_proto(data)
    local head = GlobalPBC:decode("YiQuJiangHuNetData.MsgHead", data)
    if not head or not head.proto then
        Logger:error("Decode proto head failed.")
        return nil, nil
    end
    local msg = GlobalPBC:decode(head.proto, head.data)
    return head.proto, msg
end

local function deep_copy(orig)
    local orig_type = type(orig)
    local copy
    if orig_type == "table" then
        copy = {}
        for orig_key, orig_value in next, orig, nil do
            copy[deep_copy(orig_key)] = deep_copy(orig_value)
        end
    else
        copy = orig
    end
    return copy
end

-- noCopy == true, 返回一个“代理”表，所有的修改会落到原始表上
local function struct(base, noCopy)
    if base == nil or type(base) ~= "table" then
        assert(false, "parameter type error")
        return
    end
    if next(base) == nil then
        assert(false, "table is empty")
        return
    end

    local src = {}
    if noCopy then -- 不复制原始表
        src = base
    else
        src = deep_copy(base)
    end

    local m = {}
    local mt = {}
    setmetatable(m, mt)

    mt.__newindex = function(t, k, v)
        if v == nil then
            local env = debug.getinfo(2)
            Logger:warn("can`t assign to nil variable '" .. k .. "' in source(" .. env.source .. "), line:" .. env.currentline)
            return
        else
            if rawget(src, k) == nil then
                local env = debug.getinfo(2)
                Logger:warn("assign to undeclared variable '" .. k .. "' in source(" .. env.source .. "), line:" .. env.currentline)
                return
            else
                rawset(src, k, v)
                return
            end
        end
    end

    mt.__index = function(t, k)
        return rawget(src, k)
    end

    -- 返回实际的表:  CPP 代码如　jsonc:encode(tab)　读取是采用rawget,要改成 jsonc:encode( tab() )
    mt.__call = function()
        return src
    end

    return m
end

function Protobuf_struct(protoname)
    local base = GlobalPBC:get_message(protoname)
    if next(base) ~= nil then
        return struct(base, true)
    end
    return nil
end