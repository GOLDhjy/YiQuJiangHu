local _M = {}
_M.pb = require("pb")
-- _M.pb.option("enum_as_value")

-- reset
function _M:reset()
    _M.pb.clear()
    package.loaded["libs.protoc"] = nil
    _M.protoc = require("libs.protoc")
    _M.protoc.reload()
    _M.protoc.include_imports = true
end

-- add pb path
function _M:add_pb_path(tbl)
    for index, path in pairs(tbl) do
        _M.protoc.paths[index] = path
	end 
end

function _M:load_enum(name)
    local enumTbl = {}
    local enumValueTbl = {}
    local namespace = name:split(".")[1]
    for k, v, _ in _M.pb.fields(name) do
        k = namespace .. "." .. k
        enumTbl[k] = v
        enumValueTbl[v] = k
    end
    return enumTbl, enumValueTbl
end

function _M:load_enum_without_namespace(name)
    local enumTbl = {}
    for k, v, _ in _M.pb.fields(name) do
		enumTbl[k] = v
    end
    return enumTbl
end

-- load pb
function _M:load_file(name)
    assert(self.protoc:loadfile(name))
end

-- print hex
function _M:toHex(bytes)
    print(self.pb.tohex(bytes))
end

function _M:get_message(protoname)
    --local a = self:find_message(protoname)
    --return proto
end

-- find message
function _M:find_message(message)
    return self.pb.type("." .. message)
end

-- encode message
function _M:encode(message, data)
    if self:find_message(message) == nil then
        return ""
    end

    -- encode lua table data into binary format in lua string and return
    local bytes = assert(self.pb.encode(message, data))
    -- print(self.pb.tohex(bytes))

    return bytes
end

-- decode message
function _M:decode(message, bytes)
    if self:find_message(message) == nil then
        return {}
    end

    -- decode the binary data back into lua table
    local data = assert(self.pb.decode(message, bytes))

    return data
end

function string:split(sep)
    local sep, fields = sep or "\t", {}
    local pattern = string.format("([^%s]+)", sep)
    self:gsub(pattern, function(c) fields[#fields+1] = c end)
    return fields
end

return _M
