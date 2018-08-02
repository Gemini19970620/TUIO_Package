using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace TangSheng.TUIO
{
    public class TUIOFunction : TUIOComponent
    {
        public UnityEvent Invoking_event;
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickHandler()
        {
            base.OnClickHandler();
            Invoking_event.Invoke();
            Debug.Log("触发自定义事件");
        }
    }
}
