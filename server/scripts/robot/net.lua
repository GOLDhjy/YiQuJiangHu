function Add_connect(ip, port, reconnect, env)
    return CNet.add_client({
        address 	= ip,
        port		= port,
        reconnect 	= reconnect,
        encrypt 	= 0,
        handler     = {
            ["on_connect"] = function(session)
                env:call("On_connect", session)
                RobotStatus.current_count = RobotStatus.current_count + 1
            end,
            ["on_close"] = function(session)
                env:call("On_close")
                RobotStatus.current_count = RobotStatus.current_count - 1
            end,
            ["on_package"] = function(session, data)
                local pbname, message = Decode_proto(data)
                if pbname then
                    local fname, _ = string.gsub(pbname, "%.", "_", 1)
                    env:call(fname, message)
                end
                RobotStatus.package_count = RobotStatus.package_count + 1
            end
        }
    })
end