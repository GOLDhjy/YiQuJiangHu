require("class")

--player data
local playerMgr = Class(function(self)
    self.sid = 0
    self.name = nil

    self.playerdata = {
        ["grade"] = 0,      --游戏分数


    }   
end)

function playerMgr:Account(sid,name,data)
    self.sid = sid
    self.name = name
    self.playerdata = data
end

return playerMgr