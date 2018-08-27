using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColor : MonoBehaviour
{
    public void Change_color()
    {
        float r = Random.Range(0, 1f);
        float g = Random.Range(0, 1f);
        float b = Random.Range(0, 1f);
        transform.GetComponent<Image>().color = new Color(r, g, b);
    }

    public void Enter_change()
    {
        transform.GetComponent<Image>().color = Color.red;
    }

    public void Exit_change()
    {
        transform.GetComponent<Image>().color = Color.white;
    }
}
