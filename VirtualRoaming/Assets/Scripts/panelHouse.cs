using UnityEngine;
using System.Collections;
enum house
{
    left,
    front,
    right,
    back,
    //top,
    bottom,
}
public class panelHouse : MonoBehaviour {
    public GameObject left;
    public GameObject front;
    public GameObject right;
    public GameObject back;
    //public GameObject top;
    public GameObject bottom;
    public static panelHouse shareInstance;
    public static bool allHasTexture;
    public static bool[] hasTexture = new bool[6];

    void Awake()
    {
        Main.house = new Transform[5]{
            left.transform,
            front.transform,
            right.transform,
            back.transform,
            //top.transform,
            bottom.transform
        };
        Main.defaltSideLocalPos =new Vector3[4]{
            left.transform.localPosition,
            front.transform.localPosition,
            right.transform.localPosition,
            back.transform.localPosition
        };
        //Main.defaltTopLocalPos = top.transform.localPosition;
        shareInstance = this;
    }
	// Use this for initialization
	void Start () {
        
        for (int i = 0; i < 5; i++)
        {
            //getGameObjectById(i).GetComponent<MeshRenderer>().sharedMaterial.mainTexture = null;
            hasTexture[i] = false;
        }
        allHasTexture = false;
        //int posLen = shareData.position.Length;
        //int scaLen = shareData.scale.Length;
        //if (posLen != 0)
        //{
        //    gameObject.transform.position = shareData.position[0];
        //}
        //logTexture();
	}

    void setModel(Vector3 scale)
    {
        bottom.transform.localScale = scale;

    }


    // Update is called once per frame
    void Update()
    {
        //logTexture();
        if (Main.ShareInstance.texture != null
            && !allHasTexture
            )
        {
            int count = Main.ShareInstance.texture.Count;
            for (int i = 0; i < count; i++)
            {
                
                if (hasTexture[i])
                    continue;
                else
                {
                    Debug.Log("xfy---count: " + count + " hastexture[" + i + "]: " + hasTexture[i]);
                    hasTexture[i] = true;
                    //gameObject.transform.FindChild(getNameById(i)).GetComponent<MeshRenderer>().sharedMaterial.mainTexture
                    //    = Main.ShareInstance.texture[i];
                    GameObject go = getGameObjectById(i);
                    if (go != null)
                    {
                        go.GetComponent<MeshRenderer>().materials[0].mainTexture = Main.ShareInstance.texture[i];
                        //go.GetComponent<MeshRenderer>().materials[0].
                        //go.GetComponent<MeshRenderer>().sharedMaterial.mainTexture = Main.ShareInstance.texture[i];
                    }
                }
            }
            if (count == 6)
            {
                allHasTexture = true;
            }
        }
	}
    GameObject getGameObjectById(int id)
    {
        switch (id)
        {
            case 0: return left;
            case 1: return front;
            case 2: return right;
            case 3: return back;
            //case 4: return top;
            case 4: return bottom;
        }
        return null;
    }
    string getNameById(int id)
    {
        string[] str = new string[5]{
            "left",
            "front",
            "right",
            "back",
            //"top",
            "bottom"
        };
        Debug.Log("xfy---str: "+str[id]);
        return str[id];
    }
    void logTexture()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject go = getGameObjectById(i);
            if (go != null)
            {
                Debug.Log((house)i + " : " + go.GetComponent<MeshRenderer>().sharedMaterial.mainTexture);
            }
            else
            {
                Debug.Log("gameobject is null");
            }
        }
    }
}
