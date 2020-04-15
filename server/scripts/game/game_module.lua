game_module = {}

function game_module.regist(sid, name, password)
    local dbcmd = require("db.db_command")()
    local wheretb = {Name = name}
    local msg = {m = ""}
    dbcmd:Select(nil, wheretb, "logininfo", function(eno,errMsg,results) 
        if (#results > 0) then
            msg.m = "regist again"
            Logger:info(msg.m)
            local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_regist_rsp")
            rsp.Msg = msg.m
            Send_user_message(sid, "YiQuJiangHuNetData.Server_regist_rsp", rsp())
        else
            local workID = os.date("%S");
            CUtils.IDGenerate.init(workID)
            local id = CUtils.IDGenerate.next()
            Logger:info(id)
            local logininfo = {ID = id, Name = name, Password = password, Status = 0}
            dbcmd:Insert(logininfo, "logininfo", function(eno,errMsg,results) 
                if (#results > 0) then
                    msg.m = "success"
                else
                    msg.m = "failed"
                end
                Logger:info(msg.m)
                local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_regist_rsp")
                rsp.Msg = msg.m
                Send_user_message(sid, "YiQuJiangHuNetData.Server_regist_rsp", rsp())
            end)  
        end
    end)
end

function game_module.login(sid, name, password)
    local dbcmd = require("db.db_command")()
    local where = {Name = name, Password = password}
    local msg = {m = ""}
    dbcmd:Select(nil, where, "logininfo", function(eno,errMsg,results)
        if (#results > 0) then
        --    Logger:info("Login Status:"results[1].Status)
            if tonumber(results[1].Status) == 1 then
                msg.m = "login again"
                Logger:info(msg.m)
                local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_login_rsp")
                rsp.Msg = msg.m
                Send_user_message(sid, "YiQuJiangHuNetData.Server_login_rsp", rsp())
            else 
                msg.m = "success"
                local tb = {Status = 1}
                local wheretb = {Name = name}
                playerMgr:Account(sid,results[1].Name)
                
                dbcmd:Update(tb, wheretb, "logininfo", function(eno,errMsg,results)
                    if (#results > 0) then
                        Logger:info(results[1].affected_rows)
                        if tonumber(results[1].affected_rows) <= 0 then 
                            msg.m = "failed"
                        end
                    else 
                        msg.m = "failed"
                    end
                    Logger:info(msg.m)
                    local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_login_rsp")
                    rsp.Msg = msg.m
                    Send_user_message(sid, "YiQuJiangHuNetData.Server_login_rsp", rsp())
                end)
            end
        else 
            msg.m = "failed"
            Logger:info(msg.m)
            local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_login_rsp")
            rsp.Msg = msg.m
            Send_user_message(sid, "YiQuJiangHuNetData.Server_login_rsp", rsp())
        end
    end)
end

function game_module.login_out(sid, id)
    local dbcmd = require("db.db_command")()
    local msg = {m = ""}
    local tb = {Status = 0}
    local wheretb = {ID = id}
    dbcmd:Update(tb, wheretb, "logininfo", function(eno,errMsg,results)
        if (#results > 0) then
            Logger:info(results[1].affected_rows)
            if tonumber(results[1].affected_rows) <= 0 then 
                msg.m = "failed"
            else
                msg.m = "success"
            end
        else 
            msg.m = "failed"
        end
        Logger:info(msg.m)
        local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_login_out_rsp")
        rsp.Msg = msg.m
        Send_user_message(sid, "YiQuJiangHuNetData.Server_login_out_rsp", rsp())
    end)
end

function game_module.save(sid, score, chapter, id)
    local dbcmd = require("db.db_command")()
    local wheretb = {Chapter = chapter, ID = id}
    local msg = {m = ""}
    dbcmd:Select(nil, wheretb, "gameinfo", function(eno,errMsg,results) 
        if (#results > 0) then
            local tb = {Score = score}
            dbcmd:Update(tb, wheretb, "gameinfo", function(eno,errMsg,results)
                if (#results > 0) then
                    Logger:info(results[1].affected_rows)
                    if tonumber(results[1].affected_rows) <= 0 then 
                        msg.m = "failed"
                    else
                        msg.m = "success"
                    end
                else 
                    msg.m = "failed"
                end
                Logger:info(msg.m)
                local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_save_rsp")
                rsp.Msg = msg.m
                Send_user_message(sid, "YiQuJiangHuNetData.Server_save_rsp", rsp())
            end)
        else 
            local gameinfo = {Score = score, Chapter = chapter, ID = id}
            dbcmd:Insert(gameinfo, "gameinfo", function(eno,errMsg,results) 
                if (#results > 0) then
                    msg.m = "success"
                else
                    msg.m = "failed"
                end
                Logger:info(msg.m)
                local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_save_rsp")
                rsp.Msg = msg.m
                Send_user_message(sid, "YiQuJiangHuNetData.Server_save_rsp", rsp())
            end)  
        end
    end)
end

function game_module.rank(sid, chapter)
    local dbcmd = require("db.db_command")()
    local wheretb = {Chapter = chapter}
    dbcmd:SelectOrderBy(nil, wheretb, "gameinfo", nil, "Score", function(eno,errMsg,results) 
         if (#results > 0) then
            local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_rank_rsp")
            for k, v in pairs(results) do
                local data = Protobuf_struct("YiQuJiangHuNetData.Gameinfo")
                data.Gid = v.Gid
                data.Score = v.Score
                data.Chapter = v.Chapter
                data.ID = v.ID
                Logger:info(data.Gid, data.Score, data.Chapter, data.ID)
                table.insert(rsp.Datas, data())
            end
            Send_user_message(sid, "YiQuJiangHuNetData.Server_rank_rsp", rsp())
        else 
            Logger:info("not find chapter:", chapter)
            local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_rank_rsp")
            rsp.Datas = nil
            Send_user_message(sid, "YiQuJiangHuNetData.Server_rank_rsp", rsp())
        end
    end)
end


return game_module