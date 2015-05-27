using UnityEngine;
using System.Collections;

public class Box : MonoBehaviour {
    private GameObject[] box;
    public GameObject left;
    public GameObject front;
    public GameObject right;
    public GameObject back;
    public GameObject bottom;

    private Vector3[] defaltSideLocalPos;
	// Use this for initialization
	void Start () {
        if (defaltSideLocalPos == null)
        {
            defaltSideLocalPos = new Vector3[4]{
                left.transform.localPosition,
                front.transform.localPosition,
                right.transform.localPosition,
                back.transform.localPosition
            };
        }
        if (box == null)
        {
            box = new GameObject[4]{
                left,
                front,
                right,
                back,
            };
        }
	}
    public void setBox(Vector3 scale)
    {
        //Debug.Log(scale);
        if (defaltSideLocalPos == null)
        {
            defaltSideLocalPos = new Vector3[4]{
                left.transform.localPosition,
                front.transform.localPosition,
                right.transform.localPosition,
                back.transform.localPosition
            };
        }
        if (box == null)
        {
            box = new GameObject[4]{
                left,
                front,
                right,
                back,
            };
        }
        Vector3 scaleX2Z = new Vector3();
        Vector3 scaleXX = new Vector3();
        scaleX2Z.Set(scale.z, 1, 1);
        scaleXX.Set(scale.x, 1, 1);

        bottom.transform.localScale = scale;
        left.transform.localScale = scaleX2Z;
        front.transform.localScale = scaleXX;
        right.transform.localScale = scaleX2Z;
        back.transform.localScale = scaleXX;

        for (int i = 0; i < 4; i++)
        {
            Vector3 tem = defaltSideLocalPos[i];
            //Debug.Log("tem before:" + tem);
            if (i == 0 || i == 2)
            {
                if (tem.x >= 0)
                    tem.x += 5 * (scale.x - 1);
                else
                    tem.x -= 5 * (scale.x - 1);
            }
            else
            {
                if (tem.z >= 0)
                    tem.z += 5 * (scale.z - 1);
                else
                    tem.z -= 5 * (scale.z - 1);
            }
            //Debug.Log("tem after:" + tem);
            box[i].transform.localPosition = tem;
        }
    }
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("碰撞 box:" + collisionInfo.gameObject.name);
    }
    public void setColliderActive(bool active)
    {
        foreach (GameObject side in box)
        {
            side.transform.GetComponent<MeshCollider>().enabled = active;
        }
    }

    public void setTexture(Texture t, string side) {
        Debug.LogWarning("-=-=-=-=-=-=-=-set Texture :" + t);
        GameObject go = getGameObjectByName(side);
        if (go != null)
            go.transform.GetComponent<MeshRenderer>().materials[0].mainTexture = t;
    }
    public void setBoxByAspect(int x, int y)
    {
        foreach (GameObject go in box)
        {
            float width = go.transform.localScale.x;
            float oldHeight = go.transform.localScale.z;
            float heigth = (float)y / x * width;
            Vector3 scale = new Vector3(width, 1.0f, heigth);
            go.transform.localScale = scale;
            Vector3 temV = go.transform.localPosition;
            temV.y -= 5 * (oldHeight - heigth);
            go.transform.localPosition = temV;
        }
    }
    private GameObject getGameObjectByName(string name)
    {
        if (name.Equals("left"))
            return left;
        if (name.Equals("front"))
            return front;
        if (name.Equals("back"))
            return back;
        if (name.Equals("right"))
            return right;
        if (name.Equals("bottom"))
            return bottom;
        return null;
    }
}
