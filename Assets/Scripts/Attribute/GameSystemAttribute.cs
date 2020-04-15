using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class GameSystemAttribute : Attribute
    {
        public Type subType { get; set; }
        public Type FaSystem { get; set; }
        public GameSystemAttribute(Type BaseSystemType ,Type Mytype)
        {
            this.FaSystem = BaseSystemType;
            subType = Mytype;
            GameSystems.Instance.Add(BaseSystemType, Mytype);
        }
    }
    //[System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    //sealed class MyAttribute : Attribute
    //{
    //    readonly string positionalString;

    //    // This is a positional argument
    //    public MyAttribute(string positionalString)
    //    {
    //        this.positionalString = positionalString;

    //        // TODO: Implement code here

    //        throw new NotImplementedException();
    //    }

    //    public string PositionalString
    //    {
    //        get { return positionalString; }
    //    }

    //    // This is a named argument
    //    public int NamedInt { get; set; }
    //}
    //[MyAttribute("2333",NamedInt =100)]
    //public class test
    //{
    //    void get()
    //    {
            
    //    }
    //}
}
