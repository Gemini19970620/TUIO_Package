using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//引用TouchScript
using TouchScript;
using TouchScript.Pointers;

namespace TangSheng.TUIO
{
    public class TUIOButtonManager : MonoBehaviour
    {
        //要监听的物件
        //触发button事件的物体
        public List<GameObject> TUIO_button = new List<GameObject>();
        //不带二维坐标的物体
        public List<GameObject> TUIO_function = new List<GameObject>();
        //带二维坐标的物体
        public List<GameObject> TUIO_function_v2 = new List<GameObject>();
        //拖拽的物体
        public List<GameObject> TUIO_object_drag = new List<GameObject>();
        //按下多久算点击
        public float CheckTime;
        public bool Mouse_input = false;
        //主屏分屏的尺寸
        private int main_width;
        private int main_height;
        private int less_width;
        private int less_height;

        private string show_content;

        private void Awake()
        {
            Get_ScreenSize();
            Add_TUIO_Component();
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

                //对输入点的坐标进行比例缩放
                show_content = "踏入点ID为:" + pointer.Id + "\r\n屏幕坐标" + pointer.Position;
                Vector2 trans_v2 = new Vector2((less_width / (float)main_width) * pointer.Position.x, (less_height / (float)main_height) * pointer.Position.y);
                show_content += "\r\n转换过的坐标" + trans_v2;

                foreach (var obj in TUIO_button)
                {
                    //场景中按钮订阅事件
                    obj.GetComponent<TUIOComponent>().OnUpdatePointer(pointer.Id, trans_v2);
                }
                foreach (var obj in TUIO_function)
                {
                    obj.GetComponent<TUIOComponent>().OnUpdatePointer(pointer.Id, trans_v2);
                }
                foreach (var obj in TUIO_function_v2)
                {
                    obj.GetComponent<TUIOComponent>().OnUpdatePointer(pointer.Id, trans_v2);
                }
                foreach (var obj in TUIO_object_drag)

                {
                    obj.GetComponent<TUIOComponent>().OnUpdatePointer(pointer.Id, trans_v2);
                }
            }
        }

        //输入源退出
        void pointersRemovedHandler(object sender, PointerEventArgs e)
        {
            foreach (var pointer in e.Pointers)
            {
                foreach (var obj in TUIO_button)
                {
                    //移除输入的点
                    obj.GetComponent<TUIOComponent>().OnPointerOut(pointer.Id);
                }
                foreach (var obj in TUIO_function)
                {
                    obj.GetComponent<TUIOComponent>().OnPointerOut(pointer.Id);
                }
                foreach (var obj in TUIO_function_v2)
                {
                    obj.GetComponent<TUIOComponent>().OnPointerOut(pointer.Id);
                }
                foreach (var obj in TUIO_object_drag)
                {
                    obj.GetComponent<TUIOComponent>().OnPointerOut(pointer.Id);
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
        }

        //对需要检测的物体添加脚本
        void Add_TUIO_Component()
        {
            foreach (var obj in TUIO_button)
            {
                //自动添加TuioButton脚本
                if (!obj.GetComponent<TUIOComponent>())
                {
                    obj.AddComponent<TUIOButton>();
                }
                //配置案件时长
                obj.GetComponent<TUIOComponent>().default_time = CheckTime;
            }
            foreach (var obj in TUIO_function)
            {
                if (!obj.GetComponent<TUIOComponent>())
                {
                    obj.AddComponent<TUIOFunction>();
                }
                obj.GetComponent<TUIOComponent>().default_time = CheckTime;
            }
            foreach (var obj in TUIO_function_v2)
            {
                if (!obj.GetComponent<TUIOComponent>())
                {
                    obj.AddComponent<TUIOFunctionWithVector2>();
                }
                obj.GetComponent<TUIOComponent>().default_time = CheckTime;
            }
            foreach (var obj in TUIO_object_drag)
            {
                if (!obj.GetComponent<TUIOComponent>())
                {
                    obj.AddComponent<TUIODrag>();
                }
                obj.GetComponent<TUIOComponent>().default_time = CheckTime;
            }
        }

        /*private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 80;
            GUI.Label(new Rect(Screen.width * 0.2f, Screen.height * 0.5f, 0, 0), show_content, style);
        }*/
    }
}
