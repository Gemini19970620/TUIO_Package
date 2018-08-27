using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TouchScript.Pointers;

/// <summary>
/// 触发Button点击事件
/// </summary>
namespace TangSheng.TUIO
{
    public class TUIOButtonBase : TUIOComponent
    {
        [Range(1, 2)]
        public int type = 1; //按键类型1=单次点击，2=连点,默认为1

        private float pressed_time = 0; //按下时的当前时间
        public float check_time; //按下多久算点击

        private float released_time = -1; //释放时的时间
        public float check_released_time = 0; //抬起多久算释放

        private bool isEnter = false; //是否出于进入状态
        protected List<Pointer> ID_List = new List<Pointer>(); //进入该物体范围内的poiner数组
        [HideInInspector]
        protected Vector2 pointer_v2; //输入源的坐标(只接收进入的第一个点)
        private int pressID = -1; //硬件对应的点的id

        //拖拽物体开关
        protected bool isDrag = false;

        //该物体的范围
        float width;
        float height;
        float min_x;
        float max_x;
        float min_y;
        float max_y;

        private void OnEnable()
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
                check_time = TUIOComponentManager.CheckTime;
            }
            //配置释放时间
            if (check_released_time == 0)
            {
                check_released_time = TUIOComponentManager.Released_time;
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
        void OnReleased()
        {
            pressed_time = 0;
            pressID = -1;
            if (isEnter)
            {
                OnPointerExit();
                isEnter = false;
                Get_Diff();
            }
        }

        //延迟释放
        void Delay_Released()
        {
            if (released_time == 0)
            {
                released_time = Time.time;
            }
            else if (released_time > 0)
            {
                //如果超过0.2秒后依然没有输入源在当前物体上，则真正的释放
                if (Time.time - released_time >= check_released_time)
                {
                    if (ID_List.Count == 0)
                    {
                        //第二次释放
                        OnReleased();
                    }
                }
            }
        }

        //实时检测是否满足点击状态
        protected virtual void Update()
        {
            //当所有的pointer都移除时才做移除判断
            if (ID_List.Count == 0)
            {
                //第一次释放，延迟释放
                Delay_Released();
            }
            //每0.08秒检测一次
            if (Time.frameCount % 5 == 0)
            {
                if (isEnter)
                {
                    OnPointerEnter();
                }
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
        }

        //当踏入点更新时
        public override void OnUpdatePointer(Pointer pointer)
        {
            base.OnUpdatePointer(pointer);
            if (!gameObject.activeInHierarchy) return;
            if (monitoring)
            {
                Vector2 v2 = new Vector2(TUIOComponentManager.width_percent * pointer.Position.x, TUIOComponentManager.height_percent * pointer.Position.y);
                pointer_v2 = v2;
                //判断踏入点是否在元素的范围内
                if (v2.x >= min_x && v2.x <= max_x && v2.y >= min_y && v2.y <= max_y)
                {
                    //凡是有输入源进入，则加入到id数组
                    Add_id_list(pointer);
                }
                else
                {
                    Remove_id_list(pointer);
                }
            }
        }

        //当踏入点退出时,释放该点
        public override void OnPointerOut(Pointer pointer)
        {
            base.OnPointerOut(pointer);
            Remove_id_list(pointer);
            isDrag = false;
        }

        //输入源加入数组
        public void Add_id_list(Pointer pointer)
        {
            //当有输入源进入时，释放时间重置
            released_time = 0;
            //如果之前没记录过这个点，才添加
            if (!ID_List.Contains(pointer))
            {
                ID_List.Add(pointer);
                //输入源进入
                isEnter = true;
            }
            //只记录第一个输入源的ID
            if (ID_List.Count == 1)
            {
                OnPressed(ID_List[0].Id);
            }
        }

        //输入源移除数组
        public void Remove_id_list(Pointer pointer)
        {
            int first_pointer_id = -1;
            if (ID_List.Count > 0)
            {
                first_pointer_id = ID_List[0].Id;
                //之前是否存储过这个pointer
                if (ID_List.Contains(pointer))
                {
                    //移除该pointer
                    ID_List.Remove(pointer);
                }
            }
        }

        //当输入源进入(持续触发)
        protected virtual void OnPointerEnter()
        {
            Debug.Log("输入源进入");
        }

        //当输入源退出(触发一次)
        protected virtual void OnPointerExit()
        {
            Debug.Log("输入源退出");
        }

        //触发点击事件(触发一次)
        protected virtual void OnClickHandler()
        {
            Debug.Log("触发了点击事件");
        }
    }
}

