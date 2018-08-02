using System.Collections;
using System.Collections.Generic;
using TangSheng.TUIO;
using UnityEngine;

public class Creat : MonoBehaviour
{
    public GameObject Created_Object;
    public GameObject Object_Parent;


    private void Start()
    {
        if (gameObject.GetComponent<TUIOFunctionWithVector2>() != null)
        {
            gameObject.GetComponent<TUIOFunctionWithVector2>().Function = new TUIOFunctionWithVector2.FunctionWithVector2(Creat_Object);
        }
        else
        {
            Debug.Log("请添加<TUIOFunctionWithVector2>组件");
        }
    }

    public void Creat_Object(Vector2 v2)
    {
        GameObject obj = Instantiate(Created_Object, Object_Parent.transform) as GameObject;
        obj.transform.position = v2;
    }
}
