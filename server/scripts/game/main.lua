---------------------------------------------------------------------------------------- require
package.cpath = package.cpath .. ";./windows/luamod/?.dll"
package.cpath = package.cpath .. ";./linux/luamod/?.so"
package.path = package.path .. ";./scripts/?.lua"
package.path = package.path .. ";./scripts/game/?.lua"
package.path = package.path .. ";./scripts/game/?.lcfg"
package.path = package.path .. ";./scripts/db/?.lua"
package.path = package.path .. ";./scripts/db/?.lcfg"

-- ================================= require ===================================
require("lualogger")    -- CLogger
require("luanet")       -- CNet
require("luatask")      -- CWorkPool
require("luatimer")     -- CTimer
require("luautils")     -- CUtils
require("luamysql")     -- CMySqlProxy

require("libs.strict")
require("libs.proto")
require("net")

-- ================================= global ===================================
Config = require("config")
CJson = require("cjson")
Logger = CLogger.new("game_lua", true, false, 0)

require("class")  --lua 面向对象class
require("utils")  --工具组

playerMgr = require("player")()
--require("room")
GlobalDbModule = require("db_module")() --db
GlobalDbModule:Init()
--GlobalDbModule:dbtest()

-- ================================= function ===================================
function on_start()
    CNet.init(Config.server_id, Config.net_thread)
    CNet.add_server(Create_listener(Config.listen_port))
    CNet.start()
end


function on_stop()
    CNet.stop()
    Logger:flush()
end