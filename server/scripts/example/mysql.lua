--[[
模块: luamysql
命名空间: CMySqlProxy
说明: mysql访问工具, 会创建n个线程, 每个线程一个mysql连接

  创建:
    local mysql = CMySqlProxy.new(address, port, user, password, database, character, count)
      参数看名字不解释了

  对象方法:
    获取当前连接数 mysql:count()
    转义: mysql:escape(str)
    执行语句:
      同步: local eno, errMsg, results = mysql:sync_execute(index, sql)
      异步: mysql:sync_execute(index, sql, function(eno, errMsg, results) end)
        index: 连接索引下标，内部会自动取模
        sql: 执行sql语句
        eno: 错误码，无错误0
        errMsg: 错误说明
        results: 返回结果集
          查询语句:
            results格式: {
              {field1 = value1, field2 = value2 ... },
              {field1 = value1, field2 = value2 ... },
              {field1 = value1, field2 = value2 ... },
              ...
            }

          非查询语句（只有一行数据，有两个字段）：
            results格式: {
              {affected_rows = value, last_insert_idrows = value}
            }
--]]