using UnityEngine;
using System.Collections;

public class DottedLineController : MonoBehaviour
{
    public GameObject horizontal;
    public GameObject vertical;
    public Camera UICamera;
    //public Vector3 testHorizontal;
    //public Vector3 testVerical;
    public static DottedLineController shareInstance;
    public static bool verticalLineIsAppear;
    public static bool horizontalLineIsAppear;

    void Start()
    {
        verticalLineIsAppear = false;
        horizontalLineIsAppear = false;
        //Debug.Log("dot start");
        shareInstance = this;
    }

    /// <summary>
    /// 正 往左下
    /// </summary>
    /// <param name="horizontalPosition"></param>
    /// <param name="verticalPostion"></param>
    public void setDottedLine(Vector3 horizontalPosition,Vector3 verticalPostion)
    {
        horizontal.GetComponent<MoveXu>().moveTo(horizontalPosition);
        vertical.GetComponent<MoveXu>().moveTo(verticalPostion);
    }

    public void appear(Vector3 horizontalPosition, Vector3 verticalPostion)
    {
        setDottedLine(horizontalPosition, verticalPostion);
        NGUITools.SetActive(horizontal,true);
        NGUITools.SetActive(vertical, true);
        verticalLineIsAppear = true;
        horizontalLineIsAppear = true;
    }
    public void appear(Vector3 position, ORIENTATION appear)
    {
        if (appear == ORIENTATION.VERTICAL)
        {
            //Debug.Log("vertical appear");
            setDottedLine(Vector3.zero, position);
            NGUITools.SetActive(vertical, true);
            verticalLineIsAppear = true;
        }
        else
        {
            setDottedLine(position,Vector3.zero);
            NGUITools.SetActive(horizontal, true);
            horizontalLineIsAppear = true;
        }
    }
    public void disappear(ORIENTATION o)
    {
        if (o == ORIENTATION.ALL || o == ORIENTATION.HORIZONTAL)
        {
            NGUITools.SetActive(horizontal, false);
            horizontalLineIsAppear = false;
        }
        if (o == ORIENTATION.ALL || o == ORIENTATION.VERTICAL)
        {
            NGUITools.SetActive(vertical, false);
            verticalLineIsAppear = false;
        }
    }

}
