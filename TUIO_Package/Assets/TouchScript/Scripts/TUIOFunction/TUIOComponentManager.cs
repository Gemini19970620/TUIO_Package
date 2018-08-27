using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//引用TouchScript
using TouchScript;
using TouchScript.Pointers;

namespace TangSheng.TUIO
{
    public class TUIOComponentManager : MonoBehaviour
    {
        //要监听的物件
        public List<GameObject> TUIOGameobject = new List<GameObject>();

        public static float CheckTime = 0.8f; //按下多久算点击
        public static float Released_time = 0.3f;//输入源离开后多久释放

        [Header("开启时可以在编辑器里用鼠标模拟输入源进行测试，正式打包发布时请关闭！")]
        public bool Mouse_input = false; //是否检测鼠标源输入
        //主屏分屏的尺寸
        private int main_width;
        private int main_height;
        private int less_width;
        private int less_height;

        //屏幕转换尺寸比
        public static float width_percent;
        public static float height_percent;

        private void Awake()
        {
            Get_ScreenSize();
        }

        private void OnEnable()
        {
            //监听输入源进入事件
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersAdded += pointersPressedHandler;
                TouchManager.Instance.PointersUpdated += pointersPressedHandler;
                TouchManager.Instance.PointersRemoved += pointersRemovedHandler;
            }
        }

        private void OnDisable()
        {
            //删除输入源进入事件
            if (TouchManager.Instance != null)
            {
                TouchManager.Instance.PointersAdded -= pointersPressedHandler;
                TouchManager.Instance.PointersUpdated -= pointersPressedHandler;
                TouchManager.Instance.PointersRemoved -= pointersRemovedHandler;
            }
        }

        //输入源进入
        void pointersPressedHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                //屏蔽鼠标源输入（可以防止鼠标在屏幕时对输入源的影响）
                if (Mouse_input == false)
                {
                    if (pointer.Type.Equals(Pointer.PointerType.Mouse))
                    {
                        return;
                    }
                }
                foreach (var obj in TUIOGameobject)
                {
                    //场景中按钮订阅事件
                    obj.GetComponent<TUIOComponent>().OnUpdatePointer(pointer);
                }
            }
        }

        //输入源退出
        void pointersRemovedHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                foreach (var obj in TUIOGameobject)
                {
                    //移除输入的点
                    obj.GetComponent<TUIOComponent>().OnPointerOut(pointer);
                }
            }
        }

        //获取主屏和分屏的分辨率
        void Get_ScreenSize()
        {
            if (Display.displays.Length >= 2)
            {
                main_width = Display.displays[0].systemWidth;
                main_height = Display.displays[0].systemHeight;
                less_width = Display.displays[1].systemWidth;
                less_height = Display.displays[1].systemHeight;
            }
            else
            {
                main_width = 1920;
                main_height = 1080;
                less_width = 1024;
                less_height = 768;
            }
            if (Mouse_input)
            {
                width_percent = 1;
                height_percent = 1;
            }
            else
            {
                width_percent = (less_width / (float)main_width);
                height_percent = (less_height / (float)main_height);
            }
        }
    }
}
