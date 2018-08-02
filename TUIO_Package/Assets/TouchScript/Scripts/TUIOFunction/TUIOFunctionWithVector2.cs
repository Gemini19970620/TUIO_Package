using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TangSheng.TUIO
{
    public class TUIOFunctionWithVector2 : TUIOComponent
    {
        public delegate void FunctionWithVector2(Vector2 v2);
        public FunctionWithVector2 Function;

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickHandler()
        {
            Function(base.pointer_v2);
            Debug.Log("触发一个带二维坐标的事件");
        }
    }
}
