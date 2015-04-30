using UnityEngine;
using System.Collections;
using System;
public enum SIDE
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM,
}
public enum MODE
{
    VERTICAL,
    HORIZONTAL,
}
public class editModel : MonoBehaviour {
    public int speed = 100;
    public string modelName;
    [HideInInspector]
    public bool isOnMouseDrag;
    [HideInInspector]
    public bool overModel;
    public float minOffset;

    private Color modelDefaltColor;
    private Color offsetColor;
    private bool posChange;
	// Use this for initialization
	void Start () {
        modelName = gameObject.name;
        posChange = false;
        modelDefaltColor = transform.GetComponent<Renderer>().material.color;
        offsetColor = new Color(0.5f, 0.5f, 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        

        if (isOnMouseDrag
            || overModel
            )
        {
            changeColor();
            if (isOnMouseDrag)
            {

            }
        }
        else
        {
            resetColor();
            if (posChange && !modelName.Equals("base"))
            {
                posChange = false;
                int i = Int32.Parse(modelName);
                //Debug.Log("pos" + transform.position);
                shareData.positions[i] = transform.position;
                addPosData(i);
            }
            else if (posChange && modelName.Equals("base"))
            {
                posChange = false;
                addBasePosData();
            }
        }
	}
    void adjustPlane()
    {
        //左
        Vector3 Lpos = transform.FindChild("leftSide").position;
        float SLpos = Camera.main.WorldToScreenPoint(Lpos).x;
        //右
        Vector3 Rpos = transform.FindChild("rightSide").position;
        float SRpos = Camera.main.WorldToScreenPoint(Rpos).x;

        //上
        Vector3 Tpos = transform.FindChild("topSide").position;
        float STpos = Camera.main.WorldToScreenPoint(Tpos).y;
        //下
        Vector3 Bpos = transform.FindChild("bottomSide").position;
        float SBpos = Camera.main.WorldToScreenPoint(Bpos).y;

        float minVertical =Mathf.Abs( STpos - shareData.btPos);//初始是 上边和base的上边距离
        int minVerticalId = -1;
        SIDE VerticalAjustSide = SIDE.TOP;
        SIDE VerticalBaseSide = SIDE.TOP;
        //上下最小距离求出
        for (int i = 0, max = shareData.topPos.Count; i < max; i++)
        {
            float temTT = Mathf.Abs(STpos - shareData.topPos[i]);
            float temTB = Mathf.Abs(SBpos - shareData.topPos[i]);

            float temBT = Mathf.Abs(STpos - shareData.bottomPos[i]);
            float temBB = Mathf.Abs(SBpos - shareData.bottomPos[i]);
            if (minVertical < temTT)
            {
                minVertical = temTT;
                minVerticalId = i;
                VerticalAjustSide = SIDE.TOP;
                VerticalBaseSide = SIDE.TOP;
            }
            if (minVertical < temTB)
            {
                minVertical = temTB;
                minVerticalId = i;
                VerticalAjustSide = SIDE.BOTTOM;
                VerticalBaseSide = SIDE.TOP;
            }
            if (minVertical < temBT)
            {
                minVertical = temBT;
                minVerticalId = i;
                VerticalAjustSide = SIDE.TOP;
                VerticalBaseSide = SIDE.BOTTOM;
            }
            if (minVertical < temBB)
            {
                minVertical = temBB;
                minVerticalId = i;
                VerticalAjustSide = SIDE.BOTTOM;
                VerticalBaseSide = SIDE.BOTTOM;
            }
            
        }

        float minHorizontal = Mathf.Abs(SLpos - shareData.blPos);//初始是 左边和base的左边距离
        int minHorizontalId = -1;
        SIDE HorizontalAjustSide = SIDE.LEFT;
        SIDE HorizontalBaseSide = SIDE.LEFT;
        //左右最小距离求出
        for (int i = 0, max = shareData.leftPos.Count; i < max; i++)
        {
            float temTT = Mathf.Abs(SLpos - shareData.leftPos[i]);
            float temTB = Mathf.Abs(SRpos - shareData.leftPos[i]);

            float temBT = Mathf.Abs(SLpos - shareData.rightPos[i]);
            float temBB = Mathf.Abs(SRpos - shareData.rightPos[i]);
            if (minHorizontal < temTT)
            {
                minHorizontal = temTT;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.LEFT;
                HorizontalBaseSide = SIDE.LEFT;
            }
            if (minHorizontal < temTB)
            {
                minHorizontal = temTB;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.RIGHT;
                HorizontalBaseSide = SIDE.LEFT;
            }
            if (minHorizontal < temBT)
            {
                minHorizontal = temBT;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.LEFT;
                HorizontalBaseSide = SIDE.RIGHT;
            }
            if (minHorizontal < temBB)
            {
                minHorizontal = temBB;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.RIGHT;
                HorizontalBaseSide = SIDE.RIGHT;
            }
        }

        //若最小距离小于minOffset 提示或者直接吸附。。。
        if (minVertical <= minOffset)
        {
            
        }
    }
    void prompt(MODE mode,int id,SIDE ajustSide,SIDE baseSide)   //提示
    {

    }
    void addBasePosData()
    {
        shareData.btPos = Camera.main.WorldToScreenPoint(transform.FindChild("topSide").position).y;
        shareData.bbPos = Camera.main.WorldToScreenPoint(transform.FindChild("bottomSide").position).y;

        shareData.blPos = Camera.main.WorldToScreenPoint(transform.FindChild("leftSide").position).x;
        shareData.brPos = Camera.main.WorldToScreenPoint(transform.FindChild("rightSide").position).x;
    }
    void addPosData(int id)
    {
        shareData.topPos[id] = Camera.main.WorldToScreenPoint(transform.FindChild("topSide").position).y;
        shareData.bottomPos[id] = Camera.main.WorldToScreenPoint(transform.FindChild("bottomSide").position).y;

        shareData.leftPos[id] = Camera.main.WorldToScreenPoint(transform.FindChild("leftSide").position).x;
        shareData.rightPos[id] = Camera.main.WorldToScreenPoint(transform.FindChild("rightSide").position).x;
    }
    public void changeColor()
    {
        if(transform.GetComponent<Renderer>().material.color == modelDefaltColor)
            transform.GetComponent<Renderer>().material.color -= offsetColor;
    }
    public void resetColor()
    {
        if(transform.GetComponent<Renderer>().material.color != modelDefaltColor)
            transform.GetComponent<Renderer>().material.color = modelDefaltColor;
    }
    
    void OnMouseDrag()
    {
        UIInEditModle.canMove = false;
        isOnMouseDrag = true;
        transform.position += Vector3.right * Time.deltaTime * Input.GetAxis("Mouse X") * speed;
        transform.position += Vector3.up * Time.deltaTime * Input.GetAxis("Mouse Y") * speed;
        posChange = true;
    }
    void OnMouseOver()
    {
        overModel = true;
    }

    void OnMouseExit()
    {
        overModel = false;
    }
    void OnMouseUp()
    {
        isOnMouseDrag = false;
        UIInEditModle.canMove = true;
    }
}
