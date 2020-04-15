--[[ 
模块: luanet
命名空间: CNet
说明: 网络模块

  网络初始化/启动/停止:
    CNet.init(serverID, threadCount)
    CNet.start()  -- 这里才会启动网络线程
    CNet.stop()   -- 这里会关掉网络线程

  Session:
    发送: session:send(data)
    关闭: session:close()
    获取sid: session:get_sid()
    是否关闭: session:is_close()
    远端ip: session:remote_ip()
    远端端口: session:remote_port()
    自定义数据: session.data
      默认是空表
      如果是网关session, 可以通过 session.data.__gatewayID 获取网关id

  监听端口:
    CNet.add_server({
      ["port"] = 7771,
      ["encrypt"] = 0,
      ["handler"] = {
        ["on_connect"] = function(session) end,
        ["on_close"] = function(session) end,
        ["on_package"] = function(session, data) end
      },
    })

  添加客户端:
    CNet.add_client({
      ["address"] = "127.0.0.1",
      ["port"] = 7771,
      ["reconnect"] = 0,
      ["encrypt"] = 0,
      ["handler"] = {
        ["on_connect"] = function(session) end,
        ["on_close"] = function(session) end,
        ["on_package"] = function(session, data) end
      },
    })

  网关: CNet.Gateway
    初始化:
      CNet.Gateway.init(serverID, serverType) -- 用来设置自己的服务器类型

    添加网关连接:
      CNet.Gateway.add({
      ["address"] = "127.0.0.1",
      ["port"] = 7771,
      ["reconnect"] = 0,
      ["handler"] = {
        ["on_connect"] = function(session, gatewayUserAddress, gatewayUserPort) end,   -- 网关连接回调
        ["on_close"] = function(session) end,                                          -- 网关断开回调
        ["on_user_enter"] = function(session, userSid, extra) end,                     -- 网关上用户进入当前服务器回调, extra第一次网关过来的会记录用户ip
        ["on_user_leave"] = function(session, userSid) end,                            -- 网关上用户离开当前服务器回调
        ["on_user_message"] = function(session, userSid, data) end,                    -- 网关上用户消息回调
        ["on_backend_message"] = function(session, fromID, fromType, data) end,        -- 后端服务器消息回调
        ["on_gate_load_info"] = function(session, timestamp, load) end                 -- 网关负载信息回调
      },
    })

    发送消息给用户:
      CNet.Gateway.send_to_user(sid, data)

    踢掉用户:
      CNet.Gateway.kick_user(sid)

    切换上行服务器: extraData会传给上行服务器的on_user_enter回调
      CNet.Gateway.change_server(sid, serverID, serverType, extraData)

    广播消息给指定类型服务器:
      CNet.Gateway.broadcast_backend(serverType, data)

    广播消息给玩家:
      CNet.Gateway.broadcast_front({id1, id2, id3}, data)

    获取网关负载信息:
      CNet.Gateway.get_load_info(gatewaySession)
--]]
