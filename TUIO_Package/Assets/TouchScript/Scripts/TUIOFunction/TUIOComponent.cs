using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TangSheng.TUIO
{
    /// <summary>
    /// TUIO组件基类
    /// </summary>
    /// type（1=点击，2=长按）
    /// Check_time(按键时长判断，踩上去多久算点击)
    public class TUIOComponent : MonoBehaviour
    {
        [Range(1, 2)]
        public int type = 1;//按键类型1=单次点击，2=连点,默认为1

        private int pressID = -1; //硬件对应的点的id
        private float pressed_time = 0; //按下时的当前时间
        public float check_time; //按下多久算点击
        [HideInInspector]
        public float default_time; //默认点击时间

        [HideInInspector]
        public Camera less_camera; //相机
        protected Vector2 pointer_v2;

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

        //拖拽物体开关
        protected bool isDrag = false;

        //该物体的范围
        float width;
        float height;
        float min_x;
        float max_x;
        float min_y;
        float max_y;

        private void Start()
        {
            StartCoroutine(Reset());
        }

        IEnumerator Reset()
        {
            yield return new WaitForSeconds(0.2f);
            Init();
            Get_Size();
            Get_Diff();
        }

        //获取该物体所占屏幕像素
        private void Get_Size()
        {
            width = transform.GetComponent<RectTransform>().sizeDelta.x;
            height = transform.GetComponent<RectTransform>().sizeDelta.y;
        }

        //获取该物体的范围
        public void Get_Diff()
        {
            //在canvas的RenderMode设置为Overlay时，transform.position即为屏幕坐标。
            Vector2 screen_v2 = new Vector2(transform.position.x, transform.position.y);
            min_x = screen_v2.x - width / 2;
            max_x = screen_v2.x + width / 2;
            min_y = screen_v2.y - height / 2;
            max_y = screen_v2.y + height / 2;
        }

        //初始化
        private void Init()
        {
            pressed_time = 0;
            pressID = -1;
            //配置点击时间
            if (check_time == 0)
            {
                check_time = default_time;
            }
        }

        //按下处理
        void OnPressed(int id)
        {
            //如果物体或父物体为隐藏状态，跳出
            if (!gameObject.activeInHierarchy) return;
            if (pressID == -1)
            {
                pressed_time = Time.time;
                pressID = id;
            }
        }

        //释放处理
        void OnReleased(int id)
        {
            if (pressID == id)
            {
                pressed_time = 0;
                pressID = -1;
                Get_Diff();
            }
        }

        //实时检测是否满足点击状态
        protected virtual void Update()
        {
            if (pressed_time > 0)
            {
                if (Time.time - pressed_time >= check_time)
                {
                    OnClickHandler();
                    //单点模式
                    if (type == 1)
                    {
                        pressed_time = 0;
                    }
                    //连点模式
                    else if (type == 2)
                    {
                        pressed_time = Time.time;
                    }
                }
            }
        }

        //当踏入点更新时
        public void OnUpdatePointer(int pointer_id, Vector2 v2)
        {
            if (!gameObject.activeInHierarchy) return;
            if (monitoring)
            {
                //如果该物体已经有点踩入，则不接受其他的输入源
                if (pressID == -1 || pressID == pointer_id)
                {
                    //判断踏入点是否在元素的范围内
                    if (v2.x >= min_x && v2.x <= max_x && v2.y >= min_y && v2.y <= max_y)
                    {
                        //如果是，执行按下处理。
                        OnPressed(pointer_id);
                        pointer_v2 = v2;
                    }
                    else
                    {
                        //如果不是，但之前记录过这个点是按下的，释放该点
                        if (pressID == pointer_id)
                        {
                            OnReleased(pointer_id);
                        }
                    }

                }
            }
        }

        //当踏入点退出时,释放该点
        public void OnPointerOut(int id)
        {
            OnReleased(id);
            isDrag = false;
        }

        //触发点击事件
        protected virtual void OnClickHandler()
        {
            Debug.Log("触发了TUIO事件");
        }
    }
}

