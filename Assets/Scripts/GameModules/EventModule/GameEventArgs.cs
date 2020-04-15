using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game;
namespace Modules
{
    public class GameEventArgs : EventArgs
    {
    }
    public class UIEventArgs : GameEventArgs
    {

    }
    public class InputEventArgs : GameEventArgs
    {

    }
    
    public class AttackArgs : GameEventArgs
    {
        public static readonly int Id = typeof(AttackArgs).GetHashCode();
        public bool Attack = false;
        public int UPorDOWN;
    }
    public class StateChanged : GameEventArgs
    {
        public static readonly int Id = typeof(StateChanged).GetHashCode();
        public IGameBaseState GameState;
    }
    public class AttackTypeChange : GameEventArgs
    {
        public static readonly int id = typeof(AttackTypeChange).GetHashCode();
        public JudgeType judgeType;
    }
    public class ScoreChange : GameEventArgs
    {
        public static readonly int id = typeof(ScoreChange).GetHashCode();
        public float Score;
        public int AttackNum;
    }
    public class HpChange : GameEventArgs
    {
        public static readonly int id = typeof(HpChange).GetHashCode();
        public float Hp;
    }
    public class LoginEvent : GameEventArgs
    {
        public static readonly int id = typeof(LoginEvent).GetHashCode();
        public bool Loginfor;
    }
}
