using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TangSheng.TUIO
{
    /// <summary>
    /// 类BUTTON操作（调用BUTTON已有的方法，物体上必须绑定button组件）
    /// </summary>
    public class ButtonOriginalEvent : TUIOButtonBase
    {
        protected override void OnClickHandler()
        {
            base.OnClickHandler();
            //调用button点击事件
            transform.GetComponent<Button>().onClick.Invoke();
        }
    }
}

