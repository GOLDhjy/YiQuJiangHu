-- task虚拟机使用
local json = require("cjson")
function on_task(request)
    local succ, req = pcall(json.decode, request)
    if not succ then
        return string.format("{\"error\": \"decode request failed %s\"}", request)
    end

    if req and req.cmd then
        local func = _G[req.cmd]
        if func then
            return func(req.params)
        else
            return string.format("{\"error\": \"unknown cmd %s\"}", req.cmd)
        end
    else
        return string.format("{\"error\": \"invalid request %s\"}", request)
    end
end