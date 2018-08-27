using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TouchScript.Pointers;

namespace TangSheng.TUIO
{
    /// <summary>
    /// TUIO组件基类
    /// </summary>
    /// type（1=点击，2=长按）
    /// Check_time(按键时长判断，踩上去多久算点击)
    public class TUIOComponent : MonoBehaviour
    {
        //监听开关
        protected bool monitoring = true;
        public void Open_monitoring()
        {
            monitoring = true;
        }
        public void Close_monitoring()
        {
            monitoring = false;
        }

        //监听输入源
        public virtual void OnUpdatePointer(Pointer pointer)
        {
            if (monitoring == false)
            {
                return;
            }
        }

        //移除输入源
        public virtual void OnPointerOut(Pointer pointer)
        {

        }
    }
}

