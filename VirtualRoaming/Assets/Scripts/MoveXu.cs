using UnityEngine;
using System.Collections;
public enum ORIENTATION
{
    VERTICAL,
    HORIZONTAL,
    ALL,
}
public class MoveXu : MonoBehaviour {
    public ORIENTATION orientation;
    public Camera UICamera;
    //public Vector3 moveToPos;
    private Vector3 screenPosition;
    private bool isChange;

    private Vector3 ScreenPosition
    {
        get { return screenPosition; }
        set {
            screenPosition = value;
            isChange = true;
        }
    }
	// Use this for initialization
	void Start () {
        //ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        //Debug.Log(orientation + " local: " + transform.position+" screen:"+UICamera.WorldToScreenPoint(transform.position));
        //moveTo();
        //Debug.Log(orientation + " after; " + Camera.main.WorldToScreenPoint(transform.position));
	}
	
	// Update is called once per frame
	void Update () {
        //if (isChange)
        //{

        //    Debug.Log("screen:" + ScreenPosition);
        //    Debug.Log("world" + UICamera.ScreenToWorldPoint(ScreenPosition));
        //    isChange = false;
        //}
        //else
        //{
        //    if (ScreenPosition != UICamera.WorldToScreenPoint(transform.position))
        //        ScreenPosition = UICamera.WorldToScreenPoint(transform.position);
        //}
	}
    public void moveTo(Vector3 moveToPos)
    {
        ScreenPosition = UICamera.WorldToScreenPoint(transform.position);
        //Debug.Log("moveto position:"+moveToPos);
        float offset;
        
        if (orientation == ORIENTATION.HORIZONTAL)
        {
            if (moveToPos.x != 0)
            {
                offset = moveToPos.x;
                screenPosition.x = offset;

                transform.position =UICamera.ScreenToWorldPoint(screenPosition);
                //Debug.Log("move to:" + screenPosition + " local:" + transform.position);
            }
        }
        else
        {
            if (moveToPos.y != 0)
            {
                offset = moveToPos.y;
                screenPosition.y = offset;
                transform.position = UICamera.ScreenToWorldPoint(screenPosition);
                //Debug.Log("test"+Camera.main.ScreenToWorldPoint(new Vector3(0,55f,0)));
                //Debug.Log("move to:" + screenPosition + " local:" + transform.position);
            }
        }
        
    }
}
