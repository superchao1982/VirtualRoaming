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
}
