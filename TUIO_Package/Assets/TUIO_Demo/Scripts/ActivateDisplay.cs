using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDisplay : MonoBehaviour
{
    private void Awake()
    {
        //激活双屏
        if (Display.displays.Length > 1)
        {
            //强制设置分屏分辨率为1024，768
            Display.displays[1].Activate(1024, 768, 60);
        }
    }
}
