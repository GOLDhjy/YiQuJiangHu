local _M = {}

local rpc_works = nil

local call_timeout = 1000

local json = require("cjson")

function _M.init(tasks, rpc_timeout)
    call_timeout = rpc_timeout or 1000
    rpc_works = tasks
end


-- 模拟 RPC 调用
function _M.CallRPC(func, params, callback)
    local tab = {cmd=func, params=params}
    local s = json.encode(tab)

    rpc_works:start_task(s, call_timeout,
        function(err, response)
            if err ~= '' then
                callback(err, nil)
            else
                local succ, res = pcall(json.decode, response)
                if not succ then
                    callback(string.format("decode response failed. json: %s", response), nil)
                elseif res.error then
                    callback(res.error, nil) 
                else
                    callback(nil, res)
                end
            end
        end)
end

return _M

