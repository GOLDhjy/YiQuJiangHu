-- ================================= load ===================================
package.cpath = package.cpath .. ";./windows/luamod/?.dll"
package.cpath = package.cpath .. ";./linux/luamod/?.so"
package.path = package.path .. ";./scripts/?.lua"
package.path = package.path .. ";./scripts/robot/?.lcfg"
package.path = package.path .. ";./scripts/robot/?.lua"

-- ================================= require ===================================
require("luanet")       -- CNet
require("lualogger")    -- CLogger
require("luautils")     -- CUtils
require("luarings")     -- CRings
require("luatimer")     -- CTimer

require("libs.strict")
require("libs.class")
require("libs.timewheel")
require("libs.rpc_response")
require("libs.proto")
require("net")

-- ================================= global ===================================
Config = require("config")
Json = require("cjson")
Timer = CreateTimeWheel(100)
Logger = __CPP_LOGGER

-- robot message delay info
DelayValues = {
    time = 0,
    count = 0
}

-- main get info
RobotStatus = {
    current_id = (__WORK_ID + 1) * 10000,
    current_count = 0,
    package_count = 0,
}

-- ================================= function ===================================
-- main lua state call
function Read_status(params)
	local res = {}
    res.online = RobotStatus.current_count
    res.packages = RobotStatus.package_count
    if DelayValues.count == 0 then
        res.delay = 0
    else
        res.delay = DelayValues.time / DelayValues.count
    end
    return Json.encode(res)
end

-- task虚拟机使用
local function create_robot()
    local env = CRings.new(true)
    env:script_file("scripts/robot/robot.lua")
    env:call("On_create", env)
end

-- create robot
for _ = 1, Config.per_thread_robot_count do
    create_robot()
end