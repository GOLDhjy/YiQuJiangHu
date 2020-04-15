--[[ 
模块: luatask
命名空间: CWorkPool
说明: 工作池，会创建n个线程，每个线程一个lua虚拟机用来处理耗时操作

  创建:
    local workPool = CWorkPool.new({
      scirptPath = "./scripts/task.lua",
      count = 5,
      on_task_message = function(index, data) end
    })
      scirptPath: 子虚拟机加载脚本
      count: 线程数
      on_task_message: 自虚拟机发来的消息

  对象方法:
    workPoll:start_task_with_index(index, data, expiredTime, callback)
    workPool:start_task(data, expiredTime, callback)

      index: 子虚拟机索引
      data: 发送过去的数据(string)
      expiredTime: 超过这个时间自虚拟机没有返回，会调用err为超时的callback
      callback: function(err, rsp) end
        err: 错误，无错误为空字符串""
        rsp: 其他虚拟机返回的消息

  task.lua中的task处理函数:
    function on_task(data)
      -- data是主虚拟机调用start_task传进来的参数
      return "这个是返回给主虚拟机的数据"
    end

  task.lua中的全局方法,发送数据给主虚拟机,
    g_send_to_master(data)
--]]
