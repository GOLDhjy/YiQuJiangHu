--[[ 
模块: lualogger
命名空间: CLogger
说明: 日志工具

  创建:
    local logger = CLogger.new(name, console, async, level)
      name: 日志名字
      console: 是否输出到控制台 true / false
      async: 是否异步 true / false
      level:
        0: trace
		1: info
		2: debug
		3: warn
		4: error
		5: critical

  对象方法:
    logger:trace(str)
    logger:info(str)
    logger:debug(str)
    logger:warn(str)
    logger:error(str)
    logger:critical(str)
    logger:flush()
--]]
