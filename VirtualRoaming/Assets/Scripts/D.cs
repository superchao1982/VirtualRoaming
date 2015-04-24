using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class D : MonoBehaviour {
    public static void Log(params Vector2[] o)
    {
        int num = o.Length;
        string logStr = "";
        for (int i = 0; i < num; i++)
        {
            logStr += " [x:" + o[i].x + " y:"+o[i].y+"] ";
        }
        Debug.Log(logStr);
    }
    public static void Log(params Vector3[] o)
    {
        int num = o.Length;
        string logStr = "";
        for (int i = 0; i < num; i++)
        {
            logStr += " [x:" + o[i].x + " y:" + o[i].y + " z:"+o[i].z+"] ";
        }
        Debug.Log(logStr);
    }
    public static void Log(List<Vector3> l)
    {
        int c = l.Count;
        string str = "";
        for (int i = 0; i < c; i++)
        {
            str += "[{0}:{1}]";
            str = string.Format(str, i, l[i]);
        }
        Debug.Log(str);
    }
}
