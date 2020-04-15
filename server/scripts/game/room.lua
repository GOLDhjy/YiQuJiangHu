require("class")

--room
local RoomMatch = Class(function(self)
    self.id = 0
    self.gamestart = false

    self.roomowner =  nil
    self.roomguest =  {}
end)


function RoomMatch:SetroomOwner(player)
    if self.roomowner then
        Logger:info("roomowner is cunzai")
        return false
    end

    self.roomowner = player
    return true
end

function RoomMatch:SetroomGuest(player)
    table.insert(self.roomguest,player)
end

function RoomMatch:GetRoomPlayerNumbers()

    if self.roomowner == nil then
        Logger:info("GetroomNumbers is error")
        return false
    end

    local numbers = numbers + #self.roomguest + 1;

    return numbers
end

function RoomMatch:StartGame()

    if self.gamestart then
        Logger:info("room is start")
        return false
    end

    self.gamestart = true
    return true
end

function RoomMatch:UpdateGrade(player)
    --判断玩家是否在房间中
    if player.name != self.roomowner.name then  --是否是房主
        for _,v in ipairs(roomguest) do
            if player.name == v.name then
                v.playerdata.grade = player.grade
                break
            end
            Logger:info("UpdateGrade is error")
            return false
        end
    end

    self.roomowner.playerdata.grade = player.grade

    --发送
    Broadcast()
end

--广播
local function Broadcast()
    local sendMsg = RoomMsgpack()
    Send_user_message(self.roomowner.sid,"",sendMsg)

    for i=0,#roomguest do
        local sid = self.roomguest[i].sid

        Send_user_message(sid,"",sendMsg)

    end
    
end

--打包
local function RoomMsgpack()
    local sendMsg = {}
    
    local ownmsg = {}
    ownmsg.name = self.roomowner.name
    ownmsg.grade = self.roomowner.playerdata.grade
    sendMsg[1] = ownmsg

    for _,v in ipairs(roomguest) do
        local msg = {}
        msg.name = v.playerdata.name
        msg.grade = v.playerdata.grade
        table.insert( sendMsg, msg)
    end

    return sendMsg
end

return RoomMatch