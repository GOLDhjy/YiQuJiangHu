function GetInsertSQL(tb,tbname,dbproxy)    
    local arrsql = {};
    table.insert(arrsql,"INSERT INTO ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," ( ");
    
    local index = 0;
    local tb_size = GetTableSize(tb);
    local first = true
    for k,v in pairs(tb) do      
        if not first then
            table.insert(arrsql,",");
        end  
        first = false
        table.insert(arrsql,k);        
    end
    table.insert(arrsql," ) VALUES ( ");  

 --   table.insert(arrsql," VALUES ( "); 

    index = 0;  
    first = true  
    for k,v in pairs(tb) do
        if not first then
            table.insert(arrsql,",");
        end  
        first = false
        if type(v) == "string" then
            table.insert(arrsql,"'");
            table.insert(arrsql,dbproxy:escape(v));
            table.insert(arrsql,"'");
        else
            table.insert(arrsql,v);
        end
    end

    table.insert(arrsql," );");    
    return table.concat(arrsql);
end

function GetDeleteSQL(wheretb,tbname,dbproxy)    
    local arrsql = {};
    table.insert(arrsql,"DELETE FROM ");
    table.insert(arrsql,tbname);

    table.insert(arrsql," WHERE ");
    local index = 0;
    local wheretb_size = GetTableSize(wheretb);
    local first = true
    for k,v in pairs(wheretb) do
        if not first then
            table.insert(arrsql," and ");
        end  
        first = false
        table.insert(arrsql,k);
        table.insert(arrsql," = ");

        table.insert(arrsql,"'");
        if type(v) == "string" then            
            table.insert(arrsql,dbproxy:escape(v));            
        else
            table.insert(arrsql,v);
        end
        table.insert(arrsql,"'");
    end
    table.insert(arrsql," ;");

    return table.concat(arrsql);
end

function GetUpdateSQL(tb,wheretb,tbname,dbproxy)    

    local arrsql = {};
    table.insert(arrsql,"UPDATE ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," SET ");
    
    local first = true
    local index = 0;
    for k,v in pairs(tb) do       
        if not first then
            table.insert(arrsql,",");
        end  
        first = false 
        table.insert(arrsql,k);
        table.insert(arrsql," = ");
        if type(v) == "string" then
            table.insert(arrsql,"'");
            table.insert(arrsql,dbproxy:escape(v));
            table.insert(arrsql,"'");
        else
            table.insert(arrsql,v);
        end        
    end 

    table.insert(arrsql," WHERE ");
    index = 0;
    local wheretb_size = GetTableSize(wheretb);
    first = true
    for k,v in pairs(wheretb) do
        if not first then
            table.insert(arrsql," and ");
        end  
        first = false 
        table.insert(arrsql,k);
        table.insert(arrsql," = ");
        if type(v) == "string" then
            table.insert(arrsql,"'");
            table.insert(arrsql,dbproxy:escape(v));
            table.insert(arrsql,"'");
        else
            table.insert(arrsql,v);
        end        
    end

    table.insert(arrsql,";");

    return table.concat(arrsql);
end

function GetReplaceIntoSQL(tb, tbname, dbproxy)

    local arrsql = {};
    table.insert(arrsql,"REPLACE INTO ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," ( ");

    local keys = {}
    local values = {}

    for k,v in pairs(tb) do
        table.insert(keys, k)
        table.insert(values, v)
    end

    for i = 1, #keys do
        local key = keys[i]
        table.insert(arrsql, key)
        if i < #keys then
            table.insert(arrsql, ",")
        end
    end

    table.insert(arrsql," ) values ( ")

    for i = 1, #values do
        local value = values[i]
        if type(value) == "string" then
            table.insert(arrsql,"'");
            table.insert(arrsql, dbproxy:escape(value));
            table.insert(arrsql,"'");
        else
            table.insert(arrsql, value);
        end
        if i < #values then
            table.insert(arrsql, ",")
        end
    end

    table.insert(arrsql,");");

    return table.concat(arrsql);
end

function GetSelectSQL(tb,wheretb,tbname,dbproxy)    

    local arrsql = {};
    table.insert(arrsql,"SELECT ");
    local index = 0;
    if tb == nil then
        table.insert(arrsql," * ");
    else
        for i = 1, #tb do
            table.insert(arrsql,tb[i]);
    
            index =  index + 1;
            if index ~= #tb then            
                table.insert(arrsql,",");
            end
        end 
    end

    table.insert(arrsql," FROM ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," WHERE ");

    index = 0;
    local wheretb_size = GetTableSize(wheretb);
    local first = true
    if wheretb then
        for k,v in pairs(wheretb) do
            if not first then
                table.insert(arrsql," and ");
            end  
            first = false 
            table.insert(arrsql,k);
            table.insert(arrsql," = ");

            if type(v) == "string" then
                table.insert(arrsql,"'");
                table.insert(arrsql,dbproxy:escape(v));
                table.insert(arrsql,"'");
            else
                table.insert(arrsql,v);
            end
        end    
    end
    table.insert(arrsql," ;");
    return table.concat(arrsql);
end

function GetSelectOrderBySQL(tb,wheretb,tbname,more,orderby,dbproxy)
    local arrsql = {};
    table.insert(arrsql,"SELECT ");
    local index = 0;
    if tb == nil then
        table.insert(arrsql," * ");
    else
        for i = 1, #tb do
            table.insert(arrsql,tb[i]);
    
            index =  index + 1;
            if index ~= #tb then            
                table.insert(arrsql,",");
            end
        end 
    end

    table.insert(arrsql," FROM ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," WHERE ");

    index = 0;
    local wheretb_size = GetTableSize(wheretb);
    local first = true
    if wheretb then
        for k,v in pairs(wheretb) do
            if not first then
                table.insert(arrsql," and ");
            end  
            first = false 
            table.insert(arrsql,k);
            table.insert(arrsql," = ");

            if type(v) == "string" then
                table.insert(arrsql,"'");
                table.insert(arrsql,dbproxy:escape(v));
                table.insert(arrsql,"'");
            else
                table.insert(arrsql,v);
            end
        end    
    end

    if more then
        for k,v in pairs(more) do
            for k2, v2 in pairs(v) do
                if not first then
                    table.insert(arrsql," or ");
                end
                first = false
                table.insert(arrsql,k);
                table.insert(arrsql," = ");

                if type(v2) == "string" then
                    table.insert(arrsql,"'");
                    table.insert(arrsql,dbproxy:escape(v2));
                    table.insert(arrsql,"'");
                else
                    table.insert(arrsql,v2);
                end
            end
        end
    end
    
    table.insert(arrsql," ORDER BY ")
    table.insert(arrsql, orderby)
    table.insert(arrsql, " DESC ")

    table.insert(arrsql," ;");
    return table.concat(arrsql);
end

function GetSelectLimitSQL(tb,wheretb,tbname,orderby,limit,dbproxy)
    local arrsql = {};
    table.insert(arrsql,"SELECT ");
    local index = 0;
    if tb == nil then
        table.insert(arrsql," * ");
    else
        for i = 1, #tb do
            table.insert(arrsql,tb[i]);
    
            index =  index + 1;
            if index ~= #tb then            
                table.insert(arrsql,",");
            end
        end 
    end

    table.insert(arrsql," FROM ");
    table.insert(arrsql,tbname);
    table.insert(arrsql," WHERE ");

    index = 0;
    local wheretb_size = GetTableSize(wheretb);
    local first = true
    if wheretb then
        for k,v in pairs(wheretb) do
            if not first then
                table.insert(arrsql," and ");
            end  
            first = false 
            table.insert(arrsql,k);
            table.insert(arrsql," = ");

            if type(v) == "string" then
                table.insert(arrsql,"'");
                table.insert(arrsql,dbproxy:escape(v));
                table.insert(arrsql,"'");
            else
                table.insert(arrsql,v);
            end
        end
    end

    table.insert(arrsql," ORDER BY ")
    table.insert(arrsql, orderby)
    table.insert(arrsql, " DESC ")
    table.insert(arrsql, " LIMIT ")
    table.insert(arrsql, limit)

    table.insert(arrsql," ;");
    return table.concat(arrsql);
end

------------------------------------------------------------------
local DBCommand = Class(function(self,dbname) 
    if not dbname then dbname = "yiqujianghu" end
    self.dbname = dbname
    self.dbproxy = GlobalDbModule:GetProxy(dbname)
end)

function DBCommand:Insert(dbt,tbname,callfn,key)
    self:ExcecuteSql(GetInsertSQL(dbt,tbname,self.dbproxy),callfn,key)
end

function DBCommand:Delete(wheretb,tbname,callfn)
    self:ExcecuteSql(GetDeleteSQL(wheretb,tbname,self.dbproxy),callfn)
end

function DBCommand:Update(dbt,wheretb,tbname,callfn)
    self:ExcecuteSql(GetUpdateSQL(dbt,wheretb,tbname,self.dbproxy),callfn)
end

function DBCommand:Replace(dbt,tbname,callfn)
    self:ExcecuteSql(GetReplaceIntoSQL(dbt,tbname,self.dbproxy),callfn)
end

function DBCommand:Select(dbt,wheretb,tbname,callfn)
    self:ExcecuteSql(GetSelectSQL(dbt,wheretb,tbname,self.dbproxy),callfn)
end

function DBCommand:SelectOrderBy(dbt,wheretb,tbname,more,orderby,callfn)
    self:ExcecuteSql(GetSelectOrderBySQL(dbt,wheretb,tbname,more,orderby,self.dbproxy),callfn)
end

function DBCommand:SelectLimit(dbt,wheretb,tbname,orderby,limit,callfn)
    self:ExcecuteSql(GetSelectLimitSQL(dbt,wheretb,tbname,orderby,limit,self.dbproxy),callfn)
end

function DBCommand:ExcecuteSql(sql,callfn,key)
    Logger:info(sql)
    if key == nil then key = 1 end
    self.dbproxy:execute(key,sql,function(eno, errMsg, results)
        if errMsg and errMsg ~= "" then
            Logger:info(string.format("error_db:%s  ##:%s", errMsg, sql))
        end
        if callfn then
            callfn(eno,errMsg,results)
        end
    end)
end
return DBCommand