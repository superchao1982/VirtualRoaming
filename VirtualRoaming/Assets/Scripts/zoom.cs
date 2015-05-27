using UnityEngine;
using System.Collections;
using System;

public class zoom : MonoBehaviour {
    public int speed=100;
    //public bool vertical;
    //public bool landscape;
    public string parentName;
    [HideInInspector]
    public bool onDrag;
    [HideInInspector]
    public bool overModel;

    private editModel parentModel;
    private string[] sideName;
    private bool isAddMaterial;
    private bool scaleChange;
    private bool canZoom = false;
	// Use this for initialization
	void Start () {
        //if(gameObject.name.Equals("topSide"))
        //Debug.Log("top screen:" + Camera.main.WorldToScreenPoint(transform.position) + " world:" + transform.localPosition);
        //if(gameObject.name.Equals("bottomSide"))
        //    Debug.Log("bottom screen:" + Camera.main.WorldToScreenPoint(transform.position) + " world:" + transform.localPosition);
        
        parentName = transform.parent.gameObject.name;
        scaleChange = false;
        isAddMaterial = false;
        onDrag = false;
        overModel = false;
        if (parentModel == null)
        {
            parentModel = transform.parent.GetComponent<editModel>();
        }
        if (sideName == null)
        {
            sideName = new string[4]{
                "bottomSide",
                "leftSide",
                "rightSide",
                "topSide"
            };
        }
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log("zoom update");
        if (overModel || onDrag)
        {
#if !UNITY_EDITOR
            if (canZoom)
            {
#endif
                addRenderer();
#if !UNITY_EDITOR
            }
#endif
        }
        else
        {
            Destroy(gameObject.GetComponent<MeshRenderer>());
            isAddMaterial = false;
            if (scaleChange && !parentName.Equals("base"))
            {
                scaleChange = false;
                int i = Int32.Parse(parentName);
                //Debug.Log("scale"+transform.parent.localScale);
                shareData.scales[i] = transform.parent.localScale;

                addPosData(i);
            }
        }
#if !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            Vector3 tem = Input.GetTouch(0).position;
            canZoom = true;
            //if (UIInEditModle.shareInstance.mainCamera.WorldToScreenPoint(transform.position) == tem)
            //{
                
            //}
            //else
            //{
            //    canZoom = false;
            //}
        }
        else
        {
            canZoom = false;
        }
#endif
    }
    
    void addPosData(int id)
    {
        if (shareData.topPos.Count > id)
        {
            shareData.topPos[id] = Camera.main.WorldToScreenPoint(transform.parent.FindChild("topSide").position).y;
            shareData.bottomPos[id] = Camera.main.WorldToScreenPoint(transform.parent.FindChild("bottomSide").position).y;

            shareData.leftPos[id] = Camera.main.WorldToScreenPoint(transform.parent.FindChild("leftSide").position).x;
            shareData.rightPos[id] = Camera.main.WorldToScreenPoint(transform.parent.FindChild("rightSide").position).x;
        }
        else
        {
            shareData.topPos.Add(Camera.main.WorldToScreenPoint(transform.parent.FindChild("topSide").position).y);
            shareData.bottomPos.Add(Camera.main.WorldToScreenPoint(transform.parent.FindChild("bottomSide").position).y);
            shareData.leftPos.Add(Camera.main.WorldToScreenPoint(transform.parent.FindChild("leftSide").position).x);
            shareData.rightPos.Add(Camera.main.WorldToScreenPoint(transform.parent.FindChild("rightSide").position).x);
        }
        
    }
    void OnMouseDrag()
    {
#if !UNITY_EDITOR
        if(canZoom){
            speed = 1;
#endif
        UIInEditModle.canMove = false;
        scaleChange = true;
        parentModel.isOnMouseDrag = true;
        onDrag = true;
        if (gameObject.name.Equals(sideName[0]))
        {
            Vector3 tem = Vector3.forward * Time.deltaTime * Input.GetAxis("Mouse Y") * speed;
            transform.parent.localScale -= tem;
            Vector3 p = transform.parent.position;
            p.y += tem.z * 5;
            transform.parent.position = p;
        }
        if (gameObject.name.Equals(sideName[3]))
        {
            Vector3 tem = Vector3.forward * Time.deltaTime * Input.GetAxis("Mouse Y") * speed;
            transform.parent.localScale += tem;
            Vector3 p = transform.parent.position;
            p.y += tem.z * 5;
            transform.parent.position = p;
        }
        if (gameObject.name.Equals(sideName[1]))
        {
            Vector3 tem = Vector3.left * Time.deltaTime * Input.GetAxis("Mouse X") * speed;
            transform.parent.localScale += tem;
            Vector3 p = transform.parent.position;
            p.x -= tem.x * 5;
            transform.parent.position = p;
        }
        if (gameObject.name.Equals(sideName[2]))
        {
            Vector3 tem = Vector3.left * Time.deltaTime * Input.GetAxis("Mouse X") * speed;
            transform.parent.localScale -= tem;
            Vector3 p = transform.parent.position;
            p.x -= tem.x * 5;
            transform.parent.position = p;
        }
        Vector3 v = transform.parent.localScale;
        //for (int i = 0; i < 4; i++)
        //{
            if (transform.name.Equals(sideName[1]) || transform.name.Equals(sideName[2]))
            {
                transform.parent.FindChild(sideName[1]).localScale = new Vector3(0.1f / v.x, 1, 1);
                transform.parent.FindChild(sideName[2]).localScale = new Vector3(0.1f / v.x, 1, 1);
            }
            else
            {
                transform.parent.FindChild(sideName[0]).localScale = new Vector3(0.1f / v.z, 1, 1);
                transform.parent.FindChild(sideName[3]).localScale = new Vector3(0.1f / v.z, 1, 1);
            }
            //editModel.transform.localScale= new Vector3(0.1f / v.x, 1, 1);
            //transform.localScale 
        //}
#if !UNITY_EDITOR
        }
#endif
    }
    void OnMouseOver()
    {
        //Debug.Log("On Mouse Over Event");
        //parentModel.isOnMouseDrag = true;
        overModel = true;
        parentModel.overModel = true;
        //UIInEditModle.canMove = false;
        //addRenderer();
        //parentModel.changeColor();
    }
    void addRenderer()
    {
        if (!isAddMaterial)
        {
            gameObject.AddComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
            isAddMaterial = true;
        }
    }
    void OnMouseExit()
    {
        overModel = false;
        parentModel.overModel = false;
        //Debug.Log("On Mouse exit!!!!!!!!!");
    }
    void OnMouseUp () {
        // When you release the drag over or outside the object
        //Debug.Log("Activaction of OnMouseUp!");
        onDrag = false;
        parentModel.isOnMouseDrag = false;
        UIInEditModle.canMove = true;
    }
}
