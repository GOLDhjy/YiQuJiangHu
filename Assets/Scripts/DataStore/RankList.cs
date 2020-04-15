using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;
namespace Game
{
    /// <summary>
    /// 发送给服务器的数据封装
    /// </summary>
    public class RankList
    {
        public int ID;
        public string Name;
        public Int64  Score;

    }
}
