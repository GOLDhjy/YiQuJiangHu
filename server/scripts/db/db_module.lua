
--[[
    db lua sql封装
    对mysql.lua里面的进行封装

    可以不用这套封装 直接走mysql.lua里面的

    使用：函数在db_commmand里面  DBCommand:Insert Delete..   下面有test

]]

LDBModule = Class(function(self)
    self.m_game_proxys = {}; 
end)

function LDBModule:Init()
    local Connect_sql = require("connect_sql")
    for key,game_db_info in pairs(Connect_sql) do
        local game_proxy = CMySqlProxy.new(game_db_info.ip, game_db_info.port,game_db_info.user, game_db_info.passwd, game_db_info.dbname, game_db_info.charset, game_db_info.maxcount);
        if game_proxy:count() == 0 then
            Logger:info("dbmodule game init fail, faildbkey:"..key);
            return false; 
        end
        self.m_game_proxys[game_db_info.dbname] = game_proxy
    end
    Logger:info("dbmodule game init success");
    --MYSQLSUCESS = true
    return true;
end

function LDBModule:GetProxy(dbname)
    if not dbname then dbname = "yiqujianghu" end
    return self.m_game_proxys[dbname];
end

function LDBModule:dbtest()
    local dbcmd = require("db.db_command")()

    -- local accountinfo = {id= "1", name = "wangfuwei",password = "123456",data = "helloworld"}

    -- dbcmd:Insert(accountinfo,"PlayerBase",function(eno,errMsg,results)
    --     end)

    --select test实例
    local account = "jack"

    dbcmd:Select(nil,{Name = account},"logininfo",function(eno,errMsg,results)
        
        if errMsg and errMsg ~= "" then return end

        if results == nil or #results == 0 then --注册
            --没有找到jack

        else
            local db_info = results[1]
            --Logger:info(db_info)
        end
        
    end)

end


return LDBModule