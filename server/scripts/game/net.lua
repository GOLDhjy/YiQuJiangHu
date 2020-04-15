local _users = {}
local _pbhandlers = require("handlers")

function Send_user_message(sid, proto, data)
    local session = _users[sid]
    if session then
        local sendBytes = Encode_proto_message(proto, data)
        session:send(sendBytes)
    else
        Logger:error("Send proto failed. Not found sid:", sid)
    end
end

function Create_listener(port)
    local handler = {
        -- on user connect
        ["on_connect"] = function(sess)
            _users[sess:get_sid()] = sess
            Logger:info(sess:get_sid())
        end,

        -- on user close
        ["on_close"] = function(sess)
            Logger:info("into on_close")
            _users[sess:get_sid()] = nil
        end,

        -- on user package
        ["on_package"] = function(sess, data)
            Logger:info("into on_package")

            local pbname, message = Decode_proto(data)
            if pbname then
                pbname = string.gsub(pbname, "%.", "_", 1)
                local pbhandler = _pbhandlers[pbname]
                if not pbhandler then
                    Logger:error("Not found pb: " .. pbname .. " handler")
                    return
                end
                pbhandler(sess:get_sid(), message)
            end
        end
    }

	return {
		["port"] = port,
		["encrypt"] = 0,
		["handler"] = handler
	}
end