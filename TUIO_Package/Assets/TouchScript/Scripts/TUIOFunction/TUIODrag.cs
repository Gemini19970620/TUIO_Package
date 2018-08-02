using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TangSheng.TUIO
{
    /// <summary>
    /// 拖拽事件
    /// </summary>
    public class TUIODrag : TUIOComponent
    {
        private bool IsDrag = false;

        protected override void Update()
        {
            base.Update();
            if (IsDrag)
            {
                transform.position = new Vector3(pointer_v2.x, pointer_v2.y, transform.position.z);
            }
        }
        protected override void OnClickHandler()
        {
            IsDrag = true;
            Debug.Log("触发拖拽事件");
        }
    }
}
