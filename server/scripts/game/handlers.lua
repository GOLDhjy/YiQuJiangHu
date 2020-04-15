require("game_module")

local _handlers = {}

function _handlers.YiQuJiangHuNetData_Client_ping_req(sid, msg)
    Logger:info("Receive client ping req:",sid, msg.Msg)
    local rsp = Protobuf_struct("YiQuJiangHuNetData.Server_pong_rsp")
    rsp.Msg = "Server"
    Send_user_message(sid, "YiQuJiangHuNetData.Server_pong_rsp", rsp())
end

function _handlers.YiQuJiangHuNetData_Client_regist_req(sid, msg)
    Logger:info("Receive client regist req:", msg.Name, msg.Password)
    game_module.regist(sid, msg.Name, msg.Password)
end

function _handlers.YiQuJiangHuNetData_Client_login_req(sid, msg)
    Logger:info("Receive client login req:", msg.Name, msg.Password)
    game_module.login(sid, msg.Name, msg.Password)
end

function _handlers.YiQuJiangHuNetData_Client_login_out_req(sid, msg)
    Logger:info("Receive client login out req:", msg.ID)
    game_module.login_out(sid, msg.ID)
end

function _handlers.YiQuJiangHuNetData_Client_save_req(sid, msg)
    Logger:info("Receive client save req:", msg.Score, msg.Chapter, msg.ID)
    game_module.save(sid, msg.Score, msg.Chapter, msg.ID)
end

function _handlers.YiQuJiangHuNetData_Client_rank_req(sid, msg)
    Logger:info("Receive client rank req:", msg.Chapter)
    game_module.rank(sid, msg.Chapter)
end

return _handlers