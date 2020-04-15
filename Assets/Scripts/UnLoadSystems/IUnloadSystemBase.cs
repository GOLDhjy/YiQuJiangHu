using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public interface IUnloadSystemBase
    {
        void Awake();

        void Start();


        void Update();


        void LateUpdate();

        void FixedUpdate();

        void OnDestroy();

    }
}
