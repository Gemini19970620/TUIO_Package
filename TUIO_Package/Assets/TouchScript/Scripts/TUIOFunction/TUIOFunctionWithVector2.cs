using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TangSheng.TUIO
{
    /// <summary>
    /// 类BUTTON操作（传入带二维坐标的自定义方法）
    /// </summary>
    public class TUIOFunctionWithVector2 : TUIOButtonBase
    {
        public delegate void FunctionWithVector2(Vector2 v2);
        public FunctionWithVector2 Enter_event;
        public FunctionWithVector2 Exit_event;
        public FunctionWithVector2 Click_event;

        protected override void OnPointerEnter()
        {
            base.OnPointerEnter();
            if (Enter_event != null)
            {
                Enter_event(base.pointer_v2);
            }
        }

        protected override void OnPointerExit()
        {
            base.OnPointerExit();
            if (Exit_event != null)
            {
                Exit_event(base.pointer_v2);
            }
        }

        protected override void OnClickHandler()
        {
            if (Click_event != null)
            {
                base.OnClickHandler();
                Click_event(base.pointer_v2);
            }
        }
    }
}
