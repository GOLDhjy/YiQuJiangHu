--工具组

function GetTableSize(table)
    local numItems = 0
    if table ~= nil then
        for k,v in pairs(table) do
            numItems = numItems + 1
        end
    end
    return numItems
end