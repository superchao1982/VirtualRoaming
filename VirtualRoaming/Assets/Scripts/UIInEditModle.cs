using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIInEditModle : MonoBehaviour {
    public Transform parentPlanes;
    public Transform basePlane;
    public GameObject shareObject;
    public GameObject prefab;
    public Transform camera;
    public DottedLineController dottedLineController;
    public Camera mainCamera;
    public float minOffsetOfModelPosition;
    public static SortedList xList;
    public static SortedList yList;
    public static UIInEditModle shareInstance;
    public float zoomSpeed;
    public float moveSpeed;
    public float offsetSize;
    public static bool canMove = true;
    private static int planesNum;
    private static Vector3 center;
    private static Vector2 offset;
    private static Color[] colors;

    private const int numsColor = 7;

    private static float centerX =0;
    private static float centerY=0;

    private Vector2 oldPos1 = Vector2.zero;
    private Vector2 oldPos2 = Vector2.zero;
    //public GameObject plane;
	// Use this for initialization
	void Start () {
        shareInstance = this;
        addBasePosData();
        if (colors == null)
        {
            colors = new Color[numsColor]{
                Color.blue,
                //Color.clear,
                Color.cyan,
                Color.gray,
                Color.green,
                Color.grey,
                Color.red,
                Color.yellow,
            };
        }
        planesNum = 0;
        xList = new SortedList();
        yList = new SortedList();
        if (shareData.topPos == null)
            shareData.topPos = new List<float>();
        if (shareData.bottomPos == null)
            shareData.bottomPos = new List<float>();
        if (shareData.leftPos == null)
            shareData.leftPos = new List<float>();
        if (shareData.rightPos == null)
            shareData.rightPos = new List<float>();
	}
	
	void Update ()
    {
#if UNITY_EDITOR
        //Debug.Log("UIInEditModle update");
        if (Input.GetKey(KeyCode.LeftShift))
        {
            zoom(true);
        }
        if(Input.GetKey(KeyCode.LeftControl))
        {
            zoom(false);
        }
        if (Input.GetMouseButton(0))
        {
            if(canMove)
                move();
        }
#else
        if (Input.touchCount < 1)
        {
            oldPos1 = Vector3.zero;
            oldPos2 = Vector3.zero;
        }
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (canMove)
                    move();
            }
        }
        if (Input.touchCount > 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                Vector2 tempPos1 = Input.GetTouch(0).position;
                Vector2 tempPos2 = Input.GetTouch(1).position;
                float len = isEnlarge(oldPos1, oldPos2, tempPos1, tempPos2);
                zoom(len);
                oldPos1 = tempPos1;
                oldPos2 = tempPos2;
            }
        }
#endif

    }
    #region sharedata 存储等
    void addBasePosData()
    {
        shareData.btPos = mainCamera.WorldToScreenPoint(basePlane.FindChild("topSide").position).y;
        shareData.bbPos = mainCamera.WorldToScreenPoint(basePlane.FindChild("bottomSide").position).y;

        shareData.blPos = mainCamera.WorldToScreenPoint(basePlane.FindChild("leftSide").position).x;
        shareData.brPos = mainCamera.WorldToScreenPoint(basePlane.FindChild("rightSide").position).x;
    }
    void addPosData(int id)
    {

        if (shareData.topPos.Count > id)
        {
            shareData.topPos[id] = mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("topSide").position).y;
            shareData.bottomPos[id] = mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("bottomSide").position).y;

            shareData.leftPos[id] = mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("leftSide").position).x;
            shareData.rightPos[id] = mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("rightSide").position).x;
        }
        else
        {
            shareData.topPos.Add(mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("topSide").position).y);
            shareData.bottomPos.Add(mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("bottomSide").position).y);
            shareData.leftPos.Add(mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("leftSide").position).x);
            shareData.rightPos.Add(mainCamera.WorldToScreenPoint(parentPlanes.FindChild(id.ToString()).FindChild("rightSide").position).x);
        }
    }
    void savePosData()
    {
        int id = 0;
        while (shareData.topPos.Count > id)
        {
            //Debug.Log(id);
            addPosData(id);
            id++;
        }
    }
    #endregion

    #region 摄像机移动缩放
    void move()
    {
        
        //Vector3 cPos = camera.transform.position;
#if UNITY_EDITOR
        float inputOffsetX = Input.GetAxis("Mouse X");
        float inputOffsetY = Input.GetAxis("Mouse Y");
        Vector3 offset = new Vector3(inputOffsetX, inputOffsetY, 0) * moveSpeed;
#else
        Vector3 offset = Input.GetTouch(0).deltaPosition*(moveSpeed /10);//new Vector3(inputOffsetX, inputOffsetY, 0);
        
#endif
        camera.position -= offset;
        addBasePosData();
        savePosData();
    }
    float isEnlarge(Vector2 opos1, Vector2 opos2, Vector2 npos1, Vector2 npos2)
    {
        if (opos1 == Vector2.zero)
            return 0;
        float length1 = getLenth(opos1, opos2);
        float length2 = getLenth(npos1, npos2);
        return length2 - length1;
        //if (length1 < length2)
        //    return 1;
        //else if (length1 > length2)
        //    return -1;
        //else
        //    return 0;
    }
    float getLenth(Vector2 v1, Vector2 v2)
    {
        return Mathf.Sqrt((v1.x - v2.x) * (v1.x - v2.x) + (v1.y - v2.y) * (v1.y - v2.y));
    }
    void zoom(bool up)
    {
        //Debug.Log("zoom");
//#if UNITY_EDITOR
        Vector3 offset = new Vector3(0, 0, zoomSpeed);
//#else
        //Vector2 temp1 = Input.GetTouch(0).deltaPosition*(zoomSpeed /10);
        //Vector2 temp2 = Input.GetTouch(1).deltaPosition*(zoomSpeed /10);
        //Vector3 offset = temp1+temp2;
//#endif
        if (up)
            camera.position += offset;
        else
            camera.position -= offset;

        addBasePosData();
        savePosData();
    }
    void zoom(float len)
    {
        Vector3 offset = new Vector3(0, 0, len * zoomSpeed / 100);
        camera.position += offset;
    }
    #endregion

    #region button方法
    public void addPlane()
    {
        createPlane(planesNum.ToString(), getColor(planesNum), getLocalPos());
        addPosData(planesNum);
        planesNum++;
    }
    public void createHouse()
    {
        shareData.basePosition = formatVector(basePlane.transform.position);
        shareData.baseScale = formatVector(basePlane.transform.localScale);
        Application.LoadLevel("cence1");
    }
    #endregion

    #region 调整场景方法
    void adjust(int num) {
        Dictionary<string ,Vector3> basePos = new Dictionary<string ,Vector3 >();
        for (int i = 0, j = basePlane.childCount; i < j; i++)
        {
            Transform child = basePlane.GetChild(i);
            basePos.Add(child.gameObject.name, Camera.main.WorldToScreenPoint(child.position));
        }
        if (num == 0)
        {
            Dictionary<string, Vector3> childPos = new Dictionary<string, Vector3>();
            Transform plane = parentPlanes.GetChild(num);
            for (int i = 0, j = plane.childCount; i < j; i++)
            {
                Transform child = plane.GetChild(i);
                childPos.Add(child.gameObject.name, Camera.main.WorldToScreenPoint(child.position));
            }

            change(basePos, childPos, plane);
            change(basePos, childPos, plane, true);
        }
    }
    void change(Dictionary<string, Vector3> basePos, Dictionary<string, Vector3> childPos,Transform plane,bool Y=false)
    {
        string baseChangeName = "";
        string childChangeName = "";
        Vector3 baseV;
        Vector3 childV;
        Vector3 offset;

        getChangeName(basePos, childPos, out baseChangeName, out childChangeName,Y);
        basePos.TryGetValue(baseChangeName, out baseV);
        childPos.TryGetValue(childChangeName, out childV);
        if (!Y)
        {
            float x= Camera.main.ScreenToWorldPoint(baseV).x - Camera.main.ScreenToWorldPoint(childV).x;
            //Debug.Log("x:" + x);
            offset = new Vector3(0, 0, 0);
            //if (x > offsetSize)
                offset.Set(x, 0, 0);

        }
        else
        {
            float y = Camera.main.ScreenToWorldPoint(baseV).y - Camera.main.ScreenToWorldPoint(childV).y;
            //Debug.Log("y:" + y);
            offset = new Vector3(0,0 , 0);
            if (Mathf.Abs(y) > offsetSize)
                offset.Set(0, y, 0);
        }
        //Debug.Log(offset);
        plane.position += offset;
    }
    void getChangeName(Dictionary<string, Vector3> basePos, Dictionary<string, Vector3> childPos, out string baseName, out string childName,bool Y = false)
    {
        Vector3 basePosi = new Vector3();
        Vector3 childPosi = new Vector3();
        if (!Y)
        {
            basePos.TryGetValue("leftSide", out basePosi);
            childPos.TryGetValue("rightSide", out childPosi);
            float leftRight = Mathf.Abs(basePosi.x - childPosi.x);

            basePos.TryGetValue("rightSide", out basePosi);
            childPos.TryGetValue("leftSide", out childPosi);
            float rightLeft = Mathf.Abs(basePosi.x - childPosi.x);

            basePos.TryGetValue("leftSide", out basePosi);
            childPos.TryGetValue("leftSide", out childPosi);
            float lL = Mathf.Abs(basePosi.x - childPosi.x);

            basePos.TryGetValue("rightSide", out basePosi);
            childPos.TryGetValue("rightSide", out childPosi);
            float rR = Mathf.Abs(basePosi.x - childPosi.x);

            float smallest = leftRight;
            if (rightLeft < smallest)
                smallest = rightLeft;
            if (lL < smallest)
                smallest = lL;
            if (rR < smallest)
                smallest = rR;
            if (leftRight == smallest)
            {
                baseName = "leftSide";
                childName = "rightSide";
            }
            else if(smallest==rightLeft)
            {
                childName = "leftSide";
                baseName = "rightSide";
            }
            else if (smallest == lL)
            {
                childName = "leftSide";
                baseName = "leftSide";
            }
            else
            {
                childName = "rightSide";
                baseName = "rightSide";
            }
        }
        else
        {
            basePos.TryGetValue("topSide", out basePosi);
            childPos.TryGetValue("bottomSide", out childPosi);
            float topBottom = Mathf.Abs(basePosi.y - childPosi.y);

            basePos.TryGetValue("bottomSide", out basePosi);
            childPos.TryGetValue("topSide", out childPosi);
            float bottomTop = Mathf.Abs(basePosi.y - childPosi.y);

            basePos.TryGetValue("topSide", out basePosi);
            childPos.TryGetValue("topSide", out childPosi);
            float tT = Mathf.Abs(basePosi.y - childPosi.y);

            basePos.TryGetValue("bottomSide", out basePosi);
            childPos.TryGetValue("bottomSide", out childPosi);
            float bB = Mathf.Abs(basePosi.y - childPosi.y);

            float smallest = topBottom;
            if (bottomTop < smallest)
                smallest = bottomTop;
            if (tT < smallest)
                smallest = tT;
            if (bB < smallest)
                smallest = bB;
            if (smallest == topBottom)
            {
                baseName = "topSide";
                childName = "bottomSide";
            }
            else if (smallest == bottomTop)
            {
                childName = "topSide";
                baseName = "bottomSide";
            }
            else if (smallest == tT)
            {
                childName = "topSide";
                baseName = "topSide";
            }
            else
            {
                childName = "bottomSide";
                baseName = "bottomSide";
            }
        }
    }
    #endregion


    #region 生成场景的方法
    void formatPos(Transform t)
    { 
        Vector3 tPos = formatVector(t.position);
        shareData.positions.Add(tPos);

        Vector3 tScale = formatVector(t.localScale);
        shareData.scales.Add(tScale);
    }
    Vector3 formatVector(Vector3 v)
    {
        Vector3 result = v;
        if (result.x < 0)
            result.x = 0 - result.x;
        if (result.y < 0)
            result.y = 0 - result.y;
        if (result.z < 0)
            result.z = 0 - result.z;
        return result;
    }
    #endregion

    #region 添加片的方法
    GameObject createPlane(string name,Color c,Vector3 localPosition)
    {
        GameObject prego = Instantiate(prefab) as GameObject;
        prego.name = name;
        prego.transform.parent = parentPlanes;
        prego.transform.localPosition = localPosition;
        prego.transform.localRotation = basePlane.localRotation;
        prego.transform.localScale = new Vector3(1, 1, 1);
        prego.GetComponent<MeshRenderer>().material.color = c;

        if (shareData.positions == null)
        {
            shareData.positions = new List<Vector3>();
        }
        if (shareData.scales == null)
        {
            shareData.scales = new List<Vector3>();
        }
        shareData.positions.Add(localPosition);
        shareData.scales.Add(prego.transform.localScale);
        /*
        GameObject go = new GameObject(name);
        go.AddComponent<MeshCollider>();
        go.AddComponent<MeshRenderer>().material.color = c;
        go.AddComponent<MeshFilter>().mesh = basePlane.GetComponent<MeshFilter>().mesh;
        go.transform.parent = parentPlanes;
        go.transform.localPosition = localPosition;//new Vector3(0, 0, 0);
        go.transform.localRotation = basePlane.localRotation;
        go.transform.localScale = new Vector3(1, 1, 1);*/
        return prego;
    }
    Color getColor(int i)
    {
        /*
        int r = 255;
        int b = 255;
        int g = 255;
        Debug.Log(i);
        switch (i % 3)
        {
            case 0: 
                r = 200 - i * 40;
                if (r < 0)
                {
                    b = 255 - i * 40;
                }
                break;
            case 1: 
                b = 255 - i * 40;
                if (b < 0)
                    g = 255 - i * 40;
                break;
            case 2: g = 255 - i * 40; break;
        }
        Debug.Log(r + " " + b + " " + g);
        r = Mathf.Max(0, r);
        b = Mathf.Max(0, b);
        g = Mathf.Max(0, g);
        Color c = new Color(r / 255f, b / 255f, g / 255f);
        Debug.Log(c);*/
        //Debug.Log(colors[i % numsColor]);
        return colors[i % numsColor];
    }
    Vector3 getLocalPos()
    {
        setCenter();
        Vector3 result = center;
        Vector2 moffset = new Vector2(1,1);
        switch (planesNum % 4)
        {
            case 0: moffset.x = -moffset.x - offset.x; break;
            case 1: moffset.x = moffset.x+offset.x; break;
            case 2: moffset.y = moffset.y+offset.y; break;
            case 3: moffset.y = -moffset.y - offset.y; break;
        }
        moffset *= 5;
        result.x += moffset.x;
        result.y += moffset.y;
        //Debug.Log("center: "+center+" offset: "+offset+" moffset: "+moffset+" result: "+result);
        return result;
    }
    void setCenter()
    {
        if (planesNum == 0)
        {
            center = basePlane.transform.position;
            centerX = center.x;
            centerY = center.y;
            float x = basePlane.transform.localScale.x;
            float z = basePlane.transform.localScale.z;
            //Debug.Log("x: " + x + " z:" + z);
            offset.Set(x, z);
        }
        else
        {
            //set center
            int count = shareData.positions.Count;
            centerX += shareData.positions[count-1].x;
            centerY += shareData.positions[count-1].y;

            centerX /= 2;
            centerY /= 2;
            center.Set(centerX, centerX, 0);
            
            //set offset
            //float ox = 
        }
    }
    #endregion
}
