--[[ 
模块: luatimer
命名空间: CTimer
说明: 定时器

  创建:
    local timer = CTimer.new(interval, delay, callback, args...)
      interval: 触发周期, 单位毫秒
      delay: 第一次触发延时, 单位毫秒
      callback: 触发回调 function(t, args...), 
      args...: 可变参数

  回调: t是与上次触发间隔, 单位毫秒, args是创建时传入的参数, 返回true表示继续, 返回false表示不继续
    function(t, args...) return isContinue end

  停止: timer:stop() 或者在回调中返回false

  **注意: new会创建一个定时器对象，这对象gc时该定时器也会停止，因此要注意该定时器的生命周期
--]]