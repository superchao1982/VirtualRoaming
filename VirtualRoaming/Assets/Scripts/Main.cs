using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SCALE
{
    L3_2,
    L4_3,
    L16_9,
    L16_10,
    P2_3,
    P3_4,
    P9_16,
    P10_16,
};

public class Main : MonoBehaviour {
    public static Main ShareInstance = null;
    public List<Texture> texture;
    public int realWidth;
    public int reakHeight;
    public static Transform[] house;
    private float crown;
    private float width = 2;
    private float heigth;
    public GameObject firstPersonControler;
    public GameObject androidFirstPersonControler;
    public int aspectX;
    public int aspectY;

    private float[] scale ;
    const float accuracy = 0.01f;

    private float scaleZ = 2.66f;

    public static Vector3[] defaltSideLocalPos;
    //public static Vector3 defaltTopLocalPos;
    void Awake()
    {
        scale = new float[]{1.5f,1.333f,1.778f,1.6f,0.667f,0.75f,0.5625f,0.625f};
        
#if UNITY_EDITOR
        androidFirstPersonControler.SetActive(false);
        firstPersonControler.SetActive(true);
#else
        Debug.Log("xfy android first person controler active");
        firstPersonControler.SetActive(false);
        androidFirstPersonControler.SetActive(true);
#endif
        ShareInstance = this;
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Home))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {

        if (GUI.Button(new Rect(10, 10, 150, 100), "reset"))//GUILayout.Button("打开手机相册"))
        {
            Reset();
            //NotifyCenter.Reset();
        }
        if (GUI.Button(new Rect(10, 150, 150, 100), "open Camera"))
        {
            NotifyCenter.openCamera();
        }
#if UNITY_EDITOR
        if (GUI.Button(new Rect(0, 205, 100, 100), "test"))
        {
            //Texture t = (Texture)Resources.Load("680400");

            //if (t != null)
            //{
            //    Debug.Log("xfy t not null");
            //    ShareInstance.texture.Add(t);
            //}
            setPicture();
        }
#endif
    }
    void Reset()
    {
        panelHouse.allHasTexture = false;
        for (int i = 0; i < 6; i++)
        {
            panelHouse.hasTexture[i] = false;
            house[i].gameObject.GetComponent<MeshRenderer>().materials[0].mainTexture = null;

        }

    }
    public void setPicture()
    {
        crown = (float)ShareInstance.realWidth / ShareInstance.reakHeight;
        setAspect(crown);
        setModel();
        //heigth = (int)(width * crown);
        //Debug.Log("height: " + heigth);
    }
    private void setAspect(float crown)
    {
        Debug.Log(crown);
        SCALE s = SCALE.L16_10;
        for (int i = 0; i < (int)SCALE.P10_16; i++)
        {
            if (Mathf.Abs(crown - scale[i]) < accuracy)
            {
                s = (SCALE)i;
                break;
            }
        }
        switch (s)
        {
            case SCALE.L3_2:    aspectX = 3;    aspectY = 2; break;
    	    case SCALE.L4_3:	aspectX = 4; 	aspectY = 3; break;
    	    case SCALE.L16_9:	aspectX = 16; 	aspectY = 9; break;
    	    case SCALE.L16_10:  aspectX = 16; 	aspectY = 10; break;
    	    case SCALE.P2_3:	aspectX = 2; 	aspectY = 3; break;
    	    case SCALE.P3_4:	aspectX = 3; 	aspectY = 4; break;
    	    case SCALE.P9_16:	aspectX = 9; 	aspectY = 16; break;
    	    case SCALE.P10_16:  aspectX = 10; 	aspectY = 16; break;
    	}
    }
    private void setModel()
    {
        heigth = (float)aspectY / aspectX * width;
        Debug.Log("z---" + heigth);
        Vector3 localScaleOfHouse = new Vector3(width, 1.0f, heigth);

        if (house != null)
        {
            for (int i = 0; i < 4; i++)
            {
                house[i].localScale = localScaleOfHouse;
                if (heigth != scaleZ)
                {
                    Debug.Log("heigth != scaleZ");
                    Vector3 temV = defaltSideLocalPos[i];
                    temV.y -= 5 * (scaleZ - heigth);
                    house[i].localPosition = temV;
                }
                //else if (heigth > scaleZ)
                //{
                //    Debug.Log("heigth > scaleZ");
                //    Vector3 temV = defaltSideLocalPos[i];
                //    temV.y += 5 * (scaleZ - heigth);
                //    house[i].localPosition = temV;
                //}
            }
            if (heigth != scaleZ)
            {
                //Debug.Log("heigth < scaleZ");
                //Vector3 topv = defaltTopLocalPos;
                //topv.y -= 10 * (scaleZ - heigth);
                //house[4].localPosition = topv;
            }
            //else if (heigth > scaleZ)
            //{
            //    Debug.Log("heigth > scaleZ");
            //    Vector3 topv = defaltTopLocalPos;
            //    topv.y += 10 * (scaleZ - heigth);
            //    house[4].localPosition = topv;
            //}
        }
        else
        {
            Debug.LogError("house is null");
        }
    }
}
