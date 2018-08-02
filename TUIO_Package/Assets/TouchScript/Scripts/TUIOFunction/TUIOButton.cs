using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// 触发Button点击事件
/// </summary>
namespace TangSheng.TUIO
{
    public class TUIOButton : TUIOComponent
    {
        protected override void Update()
        {
            base.Update();
        }

        protected override void OnClickHandler()
        {
            base.OnClickHandler();
            transform.GetComponent<Button>().onClick.Invoke();
            Debug.Log("触发BUTTON点击事件");
        }
    }
}

