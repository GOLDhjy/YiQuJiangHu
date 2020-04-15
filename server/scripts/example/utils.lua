--[[
模块: luautils
命名空间: CUtils
说明: 常用工具

    获取毫秒时间: 
        CUtils.millisecond()

    获取微秒时间: 
        CUtils.microsecond()

    获取[min, max]范围内的随机整数:
        CUtils.random(min, max)

    遍历目录:
        CUtils.recursive_directory(directory, callback, isRecursive)
            directory: 路径名
            callback: 回调函数
                function(name, isDir) end
                    name: 文件(夹)名 
                    idDir: 是否文件夹
            isRecursive: 是否递归目录


    base64编码:
        local encodeStr = CUtils.base64_encode(str)

    base64解码:
        local decodeStr = CUtils.base64_decode(str)

    休眠:
        CUtils.sleep(ms)

    哈希string:
        local hashValue = CUtils.stringhash(str)

    id生成器:
        范围0~2^53: workid(10 bit) timestamp(31 bit) seq(12 bit), 每秒最大生成数量2^12个id
        初始化:
            CUtils.IDGenerate.init(workID)
        生成:
            CUtils.IDGenerate.next()

    aes加解密器:
        创建:
            local crypter = CUtils.AesCrypter.new(key, iv)
        加密：
            local encryptData = crypter:encrypt(data)
        解密：
            local decryptData = crypter:decrypt(encryptData)
--]]
