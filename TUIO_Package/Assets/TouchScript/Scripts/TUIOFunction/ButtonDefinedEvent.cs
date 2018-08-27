using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace TangSheng.TUIO
{
    /// <summary>
    /// 类BUTTON操作（传入自定义的方法）
    /// </summary>
    public class ButtonDefinedEvent : TUIOButtonBase
    {
        public UnityEvent Enter_event;
        public UnityEvent Exit_event;
        public UnityEvent Click_event;

        protected override void OnPointerEnter()
        {
            base.OnPointerEnter();
            Enter_event.Invoke();
        }

        protected override void OnPointerExit()
        {
            base.OnPointerExit();
            Exit_event.Invoke();
        }

        protected override void OnClickHandler()
        {
            base.OnClickHandler();
            Click_event.Invoke();
        }
    }
}
