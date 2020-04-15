using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MusicInfo
{
    public float BarTime;
    public float Len;
    public float DeltaTime;
    public float Speed;
    public float OffsetTime;
    public List<KeyNode> keyNodes;
}
public class KeyNode
{
    public bool HaveNext;
    public int PathWay;
    public float StartTime;
    public float EndTime;
    public int Bar;
    public int Beat;
    public int Part;
    public int index;
    public ItemType itemType;
}
public enum ItemType
{
    None,
    ShortBeat,
    LongBeatStart,
    LongBeatEnd,
    Trap1,
    Trap2,
    Trap3,
    Boss,
}
