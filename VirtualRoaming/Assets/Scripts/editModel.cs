using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
public enum SIDE
{
    LEFT,
    RIGHT,
    TOP,
    BOTTOM,
}
/// <summary>
/// 横竖 mode
/// </summary>
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
    private bool canMove = false;
    private SIDE ajustVerticalSide;
    private SIDE ajustHorizontalSide;

    private static bool canDisappearDottedLine = true;
	// Use this for initialization
	void Start () {
        minOffset = UIInEditModle.shareInstance.minOffsetOfModelPosition;
        modelName = gameObject.name;
        posChange = false;
        modelDefaltColor = transform.GetComponent<Renderer>().material.color;
        offsetColor = new Color(0.3f, 0.3f, 0.3f);
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("editModel update");

        if (isOnMouseDrag)
        {
            if(!gameObject.name.Equals("base"))
            adjustPlane();
        }
        if (isOnMouseDrag
            || overModel
            )
        {
#if !UNITY_EDITOR
            if (canMove)
            {
#endif
                changeColor();
#if !UNITY_EDITOR
            }
#endif
        }
        else
        {
            resetColor();
            if(canDisappearDottedLine)
                DottedLineController.shareInstance.disappear(ORIENTATION.ALL);
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
#if !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            canMove = true;
        }
        else
        {
            canMove = false;
        }
        if (!canMove)
            resetColor();
#endif
	}

    #region 调整并显示dot line
    /// <summary>
    /// 
    /// </summary>
    /// <param name="mode">竖直 还是 横向</param>
    /// <param name="ajust1">左 或 上</param>
    /// <param name="ajust2">右 或 下</param>
    /// <param name="exceptID">排除自己</param>
    /// <param name="ajustSide">匹配边</param>
    /// <param name="baseSide">匹配边</param>
    /// <param name="offset">差距</param>
    /// <param name="isBase">是否只匹配baseplane</param>
    /// <returns></returns>
    int getBaseID(MODE mode, float ajust1, float ajust2,int exceptID,
            out SIDE ajustSide, out SIDE baseSide, out float offset, bool isBase = false)
    {
        float base1;
        float base2;
        List<float> baseList1;
        List<float> baseList2;
        SIDE side1;
        SIDE side2;
        if (mode == MODE.VERTICAL)
        {
            base1 = shareData.btPos;
            base2 = shareData.bbPos;
            baseList1 = shareData.topPos;
            baseList2 = shareData.bottomPos;
            side1 = SIDE.TOP;
            side2 = SIDE.BOTTOM;
        }
        else
        {
            base1 = shareData.blPos;
            base2 = shareData.brPos;
            baseList1 = shareData.leftPos;
            baseList2 = shareData.rightPos;
            side1 = SIDE.LEFT;
            side2 = SIDE.RIGHT;
        }
        if (isBase)
        {
            ajustSide = side1;
            baseSide = side1;

            offset = Mathf.Abs(ajust1 - base1);
            if (offset > Mathf.Abs(ajust2 - base1))
            {
                offset = Mathf.Abs(ajust2 - base1);
                ajustSide = side2;
                baseSide = side1;
            }
            if (offset > Mathf.Abs(ajust1 - base2))
            {
                offset = Mathf.Abs(ajust1 - base2);
                ajustSide = side1;
                baseSide = side2;
            }
            if (offset > Mathf.Abs(ajust2 - base2))
            {
                offset = Mathf.Abs(ajust2 - base2);
                ajustSide = side2;
                baseSide = side2;
            }
            return -1;
        }
        else
        {
            int offsetId = getBaseID(mode, ajust1, ajust2, 0, out ajustSide, out baseSide, out offset, true);
            for (int i=0, max = shareData.topPos.Count; i < max; i++)
            {
                if (i == exceptID)
                    continue;
                float temTT = Mathf.Abs(ajust1 - baseList1[i]);
                float temTB = Mathf.Abs(ajust2 - baseList1[i]);

                float temBT = Mathf.Abs(ajust1 - baseList2[i]);
                float temBB = Mathf.Abs(ajust2 - baseList2[i]);
                if (offset > temTT)
                {
                    offset = temTT;
                    offsetId = i;
                    ajustSide = side1;
                    baseSide = side1;
                }
                if (offset > temTB)
                {
                    offset = temTB;
                    offsetId = i;
                    ajustSide = side2;
                    baseSide = side1;
                }
                if (offset > temBT)
                {
                    offset = temBT;
                    offsetId = i;
                    ajustSide = side1;
                    baseSide = side2;
                }
                if (offset > temBB)
                {
                    offset = temBB;
                    offsetId = i;
                    ajustSide = side2;
                    baseSide = side2;
                }

            }
            return offsetId;
        }
    }
    
    void adjustPlane()
    {
        //Debug.Log("start adjust");
        int exceptID = Int32.Parse(gameObject.name);
        //左
        Vector3 Lpos = transform.FindChild("leftSide").position;
        float SLpos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(Lpos).x;
        //右
        Vector3 Rpos = transform.FindChild("rightSide").position;
        float SRpos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(Rpos).x;

        //上
        Vector3 Tpos = transform.FindChild("topSide").position;
        float STpos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(Tpos).y;
        //下
        Vector3 Bpos = transform.FindChild("bottomSide").position;
        float SBpos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(Bpos).y;
        #region 注释 以前的代码
        /*
        SIDE VerticalAjustSide = SIDE.TOP;
        SIDE VerticalBaseSide = SIDE.TOP;
        Debug.Log("ajust top:"+STpos + " buttom:"+SBpos+
                    "\nbase top:"+shareData.btPos + " bottom:"+shareData.bbPos);
        float minVertical =Mathf.Abs( STpos - shareData.btPos);//初始是 上边和base的上边距离
        int minVerticalId = -1;
        
        if(minVertical > Mathf.Abs(SBpos - shareData.btPos)){
            minVertical = Mathf.Abs(SBpos - shareData.btPos);
            VerticalAjustSide = SIDE.BOTTOM;
            VerticalBaseSide = SIDE.TOP;
        }
        if (minVertical > Mathf.Abs(STpos - shareData.bbPos))
        {
            minVertical = Mathf.Abs(STpos - shareData.bbPos);
            VerticalAjustSide = SIDE.TOP;
            VerticalBaseSide = SIDE.BOTTOM;
        }
        if (minVertical > Mathf.Abs(SBpos - shareData.bbPos))
        {
            minVertical = Mathf.Abs(SBpos - shareData.bbPos);
            VerticalAjustSide = SIDE.BOTTOM;
            VerticalBaseSide = SIDE.BOTTOM;
        }
        
        //上下最小距离求出
        for (int i = 0, max = shareData.topPos.Count; i < max; i++)
        {
            if (i == exceptID)
                continue;
            float temTT = Mathf.Abs(STpos - shareData.topPos[i]);
            float temTB = Mathf.Abs(SBpos - shareData.topPos[i]);

            float temBT = Mathf.Abs(STpos - shareData.bottomPos[i]);
            float temBB = Mathf.Abs(SBpos - shareData.bottomPos[i]);
            if (minVertical > temTT)
            {
                minVertical = temTT;
                minVerticalId = i;
                VerticalAjustSide = SIDE.TOP;
                VerticalBaseSide = SIDE.TOP;
            }
            if (minVertical > temTB)
            {
                minVertical = temTB;
                minVerticalId = i;
                VerticalAjustSide = SIDE.BOTTOM;
                VerticalBaseSide = SIDE.TOP;
            }
            if (minVertical > temBT)
            {
                minVertical = temBT;
                minVerticalId = i;
                VerticalAjustSide = SIDE.TOP;
                VerticalBaseSide = SIDE.BOTTOM;
            }
            if (minVertical > temBB)
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
            if (i == exceptID)
                continue;
            float temTT = Mathf.Abs(SLpos - shareData.leftPos[i]);
            float temTB = Mathf.Abs(SRpos - shareData.leftPos[i]);

            float temBT = Mathf.Abs(SLpos - shareData.rightPos[i]);
            float temBB = Mathf.Abs(SRpos - shareData.rightPos[i]);
            if (minHorizontal > temTT)
            {
                minHorizontal = temTT;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.LEFT;
                HorizontalBaseSide = SIDE.LEFT;
            }
            if (minHorizontal > temTB)
            {
                minHorizontal = temTB;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.RIGHT;
                HorizontalBaseSide = SIDE.LEFT;
            }
            if (minHorizontal > temBT)
            {
                minHorizontal = temBT;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.LEFT;
                HorizontalBaseSide = SIDE.RIGHT;
            }
            if (minHorizontal > temBB)
            {
                minHorizontal = temBB;
                minHorizontalId = i;
                HorizontalAjustSide = SIDE.RIGHT;
                HorizontalBaseSide = SIDE.RIGHT;
            }
        }*/
        #endregion
        float minVertical;
        SIDE baseVerticalSide;
        int minVerticalId = getBaseID(MODE.VERTICAL, STpos, SBpos, exceptID, 
            out ajustVerticalSide, out baseVerticalSide, out minVertical, false);

        float minHorizontal;
        SIDE baseHorizontalSide;
        int minHorizontalId = getBaseID(MODE.HORIZONTAL, SLpos, SRpos, exceptID, 
            out ajustHorizontalSide, out baseHorizontalSide, out minHorizontal, false);
        //Debug.Log("vertical: " + minVertical + " minoffset: " + minOffset
        //    + "minHorizontal:" + minHorizontal);
        //若最小距离小于minOffset 提示或者直接吸附。。。
        if (minVertical <= minOffset)
        {
            //Debug.Log("vertical id:" + minVerticalId);
            prompt(MODE.VERTICAL, minVerticalId, baseVerticalSide);
        }
        else
        {
            DottedLineController.shareInstance.disappear(ORIENTATION.VERTICAL);
        }
        if (minHorizontal <= minOffset)
        {
            prompt(MODE.HORIZONTAL, minHorizontalId, baseHorizontalSide);
        }
        else
        {
            DottedLineController.shareInstance.disappear(ORIENTATION.HORIZONTAL);
        }

    }
    void prompt(MODE mode,int id,SIDE baseSide)   //提示
    {
        canDisappearDottedLine = false;
        if (mode == MODE.VERTICAL)
        {
            Vector3 verticalPosition = Vector3.zero;
            if (id == -1)
            {

                switch (baseSide)
                {
                    case SIDE.TOP:
                        verticalPosition.Set(0, shareData.btPos, 0);
                        //Debug.Log(shareData.btPos);
                        break;
                    case SIDE.BOTTOM:
                        verticalPosition.Set(0, shareData.bbPos, 0);
                        break;
                }

            }
            else
            {
                switch (baseSide)
                {
                    case SIDE.TOP:
                        verticalPosition.Set(0, shareData.topPos[id], 0);
                        break;
                    case SIDE.BOTTOM:
                        verticalPosition.Set(0, shareData.bottomPos[id], 0);
                        break;
                }
            }
            DottedLineController.shareInstance.appear(verticalPosition, ORIENTATION.VERTICAL);
        }
        else
        {
            Vector3 horizontalPosition = Vector3.zero;
            if (id == -1)
            {

                switch (baseSide)
                {
                    case SIDE.LEFT:
                        horizontalPosition.Set(shareData.blPos,0, 0);
                        break;
                    case SIDE.RIGHT:
                        horizontalPosition.Set(shareData.brPos, 0,0);
                        break;
                }

            }
            else
            {
                switch (baseSide)
                {
                    case SIDE.LEFT:
                        horizontalPosition.Set(shareData.leftPos[id], 0,0);
                        break;
                    case SIDE.RIGHT:
                        horizontalPosition.Set(shareData.rightPos[id], 0,0);
                        break;
                }
            }
            DottedLineController.shareInstance.appear(horizontalPosition, ORIENTATION.HORIZONTAL);
        }
    }
    void addBasePosData()
    {
        shareData.btPos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("topSide").position).y;
        shareData.bbPos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("bottomSide").position).y;

        shareData.blPos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("leftSide").position).x;
        shareData.brPos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("rightSide").position).x;
    }
    void addPosData(int id)
    {

        if (shareData.topPos.Count > id)
        {
            shareData.topPos[id] = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("topSide").position).y;
            shareData.bottomPos[id] = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("bottomSide").position).y;

            shareData.leftPos[id] = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("leftSide").position).x;
            shareData.rightPos[id] = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("rightSide").position).x;
        }
        else
        {
            shareData.topPos.Add(UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("topSide").position).y);
            shareData.bottomPos.Add(UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("bottomSide").position).y);
            shareData.leftPos.Add(UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("leftSide").position).x);
            shareData.rightPos.Add(UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("rightSide").position).x);
        }
    }
/// <summary>
    /// 传那个线的屏幕位置 side1竖直 side2 横向
/// </summary>
/// <param name="o"></param>
/// <param name="x"></param>
/// <param name="y"></param>
/// <param name="side1"></param>
/// <param name="side2"></param>
    void moveModleToPos(ORIENTATION o,float x, float y, SIDE side1, SIDE side2)
    {
        //竖向
        Vector3 modelScreenPos1 = Vector3.zero;
        //横向
        Vector3 modelScreenPos2 = Vector3.zero;
        float offsetX = 0;
        float offsetY = 0;
        if (o == ORIENTATION.ALL || o == ORIENTATION.VERTICAL)
        {
            
            if (side1 == SIDE.TOP)
                modelScreenPos1 = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("topSide").position);
            else if (side1 == SIDE.BOTTOM)
                modelScreenPos1 = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("bottomSide").position);
            offsetY = modelScreenPos1.y - y;
            
        }
        if (o == ORIENTATION.ALL || o == ORIENTATION.HORIZONTAL)
        {
            if (side2 == SIDE.LEFT)
                modelScreenPos2 = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("leftSide").position);
            else if (side2 == SIDE.RIGHT)
                modelScreenPos2 = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.FindChild("rightSide").position);
            offsetX = modelScreenPos2.x - x;
        }
        Vector3 screenPos = UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.position);
        screenPos.Set(screenPos.x - offsetX, screenPos.y - offsetY, screenPos.z);
        transform.position = UIInEditModle.shareInstance.mainCamera.ScreenToWorldPoint(screenPos);
    }
    #endregion

    #region 设置模型颜色
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
    #endregion

    #region mouse method
    void OnMouseDrag()
    {
#if !UNITY_EDITOR
        if (canMove)
        {
            speed = 30;
#endif
            UIInEditModle.canMove = false;
            isOnMouseDrag = true;
            transform.position += Vector3.right * Time.deltaTime * Input.GetAxis("Mouse X") * speed;
            transform.position += Vector3.up * Time.deltaTime * Input.GetAxis("Mouse Y") * speed;
            posChange = true;
#if !UNITY_EDITOR
        }
#endif
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

        if (DottedLineController.verticalLineIsAppear)
        {
            //Debug.Log("try move vertical");
            float y = DottedLineController.shareInstance.UICamera.WorldToScreenPoint( 
                DottedLineController.shareInstance.vertical.transform.position).y;
            moveModleToPos(ORIENTATION.VERTICAL, 0, y, ajustVerticalSide, ajustHorizontalSide);
            //Debug.Log("move vertical");
        }
        if (DottedLineController.horizontalLineIsAppear)
        {
            //Debug.Log("try move horizontal");
            float x = DottedLineController.shareInstance.UICamera.WorldToScreenPoint(
                DottedLineController.shareInstance.horizontal.transform.position).x;
            moveModleToPos(ORIENTATION.HORIZONTAL, x, 0, ajustVerticalSide, ajustHorizontalSide);
            //Debug.Log("move horizontal");
        }
            canDisappearDottedLine = true;
    }
    #endregion
}
