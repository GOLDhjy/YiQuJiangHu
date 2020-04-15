local uitls = CUtils
local env = nil
local session = nil

---------------------------- action ------------------------------
local actions = {
    ["ping"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_ping_req")
        req.Msg = "Client"
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_ping_req", req())
        if session then
            session:send(bytes)
        end
    end,

    ["regist"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_regist_req")
        req.Name = "xuzhimo"
        req.Password = "888"
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_regist_req", req())
        if session then
            session:send(bytes)
        end
    end,

    ["login"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_login_req")
        req.Name = "jack"
        req.Password = "1234"
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_login_req", req())
        if session then
            session:send(bytes)
        end
    end,

    ["login_out"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_login_out_req")
        req.ID = 105926324748288
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_login_out_req", req())
        if session then
            session:send(bytes)
        end
    end,

    ["save"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_save_req")
        req.Score = 100
        req.Chapter = 4
        req.ID = 1
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_save_req", req())
        if session then
            session:send(bytes)
        end
    end,

    ["rank"] = function()
        local req = Protobuf_struct("YiQuJiangHuNetData.Client_rank_req")
        req.Chapter = 4
        local bytes = Encode_proto_message("YiQuJiangHuNetData.Client_rank_req", req())
        if session then
            session:send(bytes)
        end
    end
}

---------------------------- life ------------------------------
function On_create(selfEnv)
    env = selfEnv
    RobotStatus.current_id = RobotStatus.current_id + 1
    Add_connect(Config.address, Config.port, Config.reconnect, env)
end

function On_connect(sess)
    session = sess
--    actions.ping()
--    actions.regist()
      actions.login()
 --     actions.login_out()
--    actions.save()
--    actions.rank()
end

function On_close()
    session = nil
end

---------------------------- handler ---------------------------
function YiQuJiangHuNetData_Server_pong_rsp(msg)
    Logger:info("Receive server pong rsp:", msg.Msg)

    Timer:After(1000, function()
        actions.ping()
    end)
end

function YiQuJiangHuNetData_Server_regist_rsp(msg)
    Logger:info("Receive server regist rsp:", msg.Msg)
end

function YiQuJiangHuNetData_Server_login_rsp(msg)
    Logger:info("Receive server login rsp:", msg.Msg)

  --  Timer:After(2000, function()
   --     actions.login()
 --   end)
end

function YiQuJiangHuNetData_Server_login_out_rsp(msg)
    Logger:info("Receive server login_out rsp:", msg.Msg)
end

function YiQuJiangHuNetData_Server_save_rsp(msg)
    Logger:info("Receive server save rsp:", msg.Msg)
end

function YiQuJiangHuNetData_Server_rank_rsp(msg)
    Logger:info("Receive server rank rsp:")
    if (#msg.Datas == 0) then
        Logger:info("rsp is nil")
    else
        for k, v in pairs(msg.Datas) do
            Logger:info(v.Gid, v.Score, v.Chapter, v.ID)
        end
    end
end


