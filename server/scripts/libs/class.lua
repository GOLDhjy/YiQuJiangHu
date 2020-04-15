local _class = {}

--[[
	使用方式
	Derived = class(Base or nil, function(self, ...)

	end)
	.
	.

	定义
	function Derived:Func()
		-- can call super Func()
		self.super:Func();
	end

	创建类
	local d = Derived() 
	or 
	local d = Derived.new()
	d:Func()
--]]

------------------------ start define class function --------------------
-- 支持继承: base is base class, and _ctor is construct function.
function class(base, _ctor)
	local child = {
		Ctor = false,
		__base__ = base,
	}

	-- define base class and construct function.
	if not _ctor and type(base) == "function" then
		local ctor = base
		child.Ctor = ctor
		child.__base__ = nil
	elseif not base and type(_ctor) == "function" then
		child.Ctor = _ctor
		child.__base__ = nil
    elseif _ctor and type(_ctor) == "function" then
        child.Ctor = _ctor
		child.__base__ = base
	end

	-- the onlye new function which will be called as follows.
	local function _new(...)
		local object = {}
		-- 将_class里的"类", 实例化成 object
		setmetatable(object, {__index = _class[child]})
		local function create(c, ...)
			-- try call base ctor
			if c.__base__ then
				create(c.__base__, ...)
			end
			-- call ctor function if have
			if c.Ctor then
				c.Ctor(object, ...)
			end
		end
		create(child, ...)
		return object
	end

	-- create instance, call all base Ctor function.
	child.new = function(...)
		return _new(...)	
	end

	-- 声明一个"空类", save intance member.
	local vtbl = {}
	vtbl.is_a = function(self, klass)
		local m = child;
		while m do 
			if m == klass then return true end
			m = m.__base__;
		end
		return false;
    end
	_class[child] = vtbl

	-- support class() to create instance: __call
	-- set value to vtbl table.
	setmetatable(child, {
		__newindex = function(t, k, v) rawset(vtbl, k, v) end,
		__call = function(class_tbl, ...) return _new(...) end 
	})

	-- 如果有基类，索引一个不存在的字段时，就去基类找（继承）
	if child.__base__ then
		setmetatable(vtbl, {__index = function(t, k)
			if not k then
				return nil
			end

			if k == "super" then -- if super then return __base__ data, this can simulate c++, can call base related method.
				return _class[child.__base__]
			else
				local value = _class[child.__base__][k]
				if value then -- only save not nil value.
				    vtbl[k] = value
				end
			    return value
			end
		end})
	end

	return child
end
--------------------- end of class define --------------------
