-- ================================= load ===================================
package.cpath = package.cpath .. ";./windows/luamod/?.dll"
package.cpath = package.cpath .. ";./linux/luamod/?.so"
package.path = package.path .. ";./scripts/?.lua"
package.path = package.path .. ";./scripts/robot/?.lua"
package.path = package.path .. ";./scripts/robot/?.lcfg"

-- ================================= require ===================================
require("luatimer")     -- CTimer
require("luanet")       -- CNet
require("luautils")     -- CUtils
require("luatask")      -- CWorkPool
require("lualogger")    -- CLogger

require("libs.strict")
-- ================================= global ===================================
Logger = __CPP_LOGGER
Config = require("config")

Global("Worker")
Global("TaskTimer")

-- ================================= function ===================================
local function statistics()
    if Config.statistics ~= 0 then
        local begin = CUtils.millisecond()
        TaskTimer = CTimer.new(Config.statistics, 0,
            function(t)
                local count = 0
                local total_connect = 0
                local total_package = 0
                local total_delay = 0
                for i = 1, Config.thread_count do
                    -- rpc call back
                    Worker.CallRPC("Read_status", {idx = i},
                        function(err, res)
                            if not err then
                                total_connect = total_connect + res.online
                                total_package = total_package + res.packages
                                total_delay = total_delay + res.delay
                            else
                                Logger:warn(err)
                            end
                            count = count + 1
                            if count == Config.thread_count then -- sum ok
                                Logger:info(string.format("Online: %d QPS: %.1f/s Delay: %.1f ms Package: %d ",
                                        total_connect,
                                        total_package * 1000 / (CUtils.millisecond() - begin),
                                        total_delay / count / 1000,
                                        total_package
                                    )
                                )
                            end
                        end
                    )
                end
                return true
            end
        )
    end
end

local function start_work()
    Worker = require("libs.rpc_request")
    Worker.init(CWorkPool.new({
        ["path"] = "./scripts/robot/task.lua",
        ["count"] = Config.thread_count
    }), Config.rpc_expired)
end

function on_start()
    CNet.init(0, Config.net_thread)
    CNet.start() -- 这里才启动网络线程

    start_work()
    statistics()
end

function on_stop()
    CNet.stop()
end
