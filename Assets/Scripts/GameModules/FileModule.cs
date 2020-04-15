using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace Modules
{
    public class FileModule : Singleton<FileModule>
    {
        public bool WriteText(string path, string content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                if (string.IsNullOrEmpty(content))
                {
                    Debug.LogError("音乐数据文件为空，转换失败");
                    return false;
                }
                byte[] bytes = Encoding.UTF8.GetBytes(content);

                try
                {
                    fs.Write(bytes, 0, bytes.Length);
                    Debug.Log("导出完成");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                }
            }
            return true;
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(path))
            //    {

            //        sw.WriteLine(content);
            //        //LogService.Instance.Log(LogLevel.info, "写入文件成功");
            //    }
            //}
            //catch (Exception ep)
            //{
            //    //LogService.Instance.Log(LogLevel.err, "写入文件失败:" + ep.ToString());
            //}
        }

        public bool ReadText(string path, StringBuilder content)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                byte[] bytes = new byte[10240];
                try
                {
                    while (fs.Read(bytes, 0, bytes.Length) != 0)
                    {
                        content.Append(Encoding.UTF8.GetString(bytes));
                        Array.Clear(bytes, 0, bytes.Length);
                    }
                    Debug.Log("导出完成");
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    return false;
                   
                }
            }
            //try
            //{
            //    using (StreamReader sr = new StreamReader(path))
            //    {
            //        content.Append(sr.ReadLine());
            //       // LogService.Instance.Log(LogLevel.info, "读入文件成功");
            //    }
            //}
            //catch (Exception ep)
            //{
            //    return false;
            //   // LogService.Instance.Log(LogLevel.err, "读入文件失败:" + ep.ToString());
            //}
            return true;
        }
    }
}
