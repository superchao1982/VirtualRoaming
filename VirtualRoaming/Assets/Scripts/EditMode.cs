using UnityEngine;
using System.Collections;
enum direction
{
    forward,
    back,
    left,
    right,
}
public class EditMode : MonoBehaviour {
    public GameObject baseBox;
    public GameObject parent;
    public GameObject modeChangeButton;
    public UILabel modeButtonLabel;
    public GameObject firstPersonController;
    public GameObject edit;			//模型中的EDIT容器；
    public GameObject pickPictureButton;//模型中的pickPicture按钮；
    public GameObject takePhotoButton;	//模型中的takePhoto按钮；
    public GameObject mainCamera;		//模型中的Camera；
    public float zoomSpeed;				//缩放速度；
    public float moveSpeed;				//移动速度；
    public GameObject joy;					//虚拟摇杆；
    public static string pickedModelName;
    public static string pickedSideName;
    public static EditMode shareInstance ;	//共享实例
    public static bool canMove = true;	//能否移动；
    public static bool isInEditMode = false;//是否在编辑模式中；
    public Joystick left;
    public Joystick right;
    public static bool isTouchJoy = false;
    public Camera UICamera;

    private static Vector3 lastPlayerPos;
    private int aspectX;
    private int aspectY;
    private const string SAVEMODE = "save";
    private const string EDITMODE = "edit mode";
    private const float accuracy = 0.01f;
    //private static Joystick left, right;
    private float[] scale = new float[]{1.5f,1.333f,1.778f,1.6f,0.667f,0.75f,0.5625f,0.625f};
    
    public bool isTest;
    private string testText = "test";
    public Texture2D testTexture;
    public Transform testTransform;
    public Texture2D targetTexture;
    public string name;
    public string side;
    public static bool haseTexture;
	void Start () {
        shareInstance = this;
        //isInEditMode = false;
#if !UNITY_EDITOR
        //moveSpeed = 0.1f;
        //zoomSpeed = 0.1f;
        //left = joy.transform.FindChild("LeftJoystick").GetComponent<Joystick>();
        //right = joy.transform.FindChild("RightJoystick").GetComponent<Joystick>();
        mainCamera.transform.GetComponent<MouseLook>().enabled = false;
#endif
    }
	
	void Update () {
        //if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    zoom(true);
        //}
        //if (Input.GetKey(KeyCode.LeftControl))
        //{
        //    zoom(false);
        //}
        if (haseTexture)
        {
            //Debug.LogWarning("----------xfy----------has texture");
            setTexture(targetTexture, name, side);
            haseTexture = false;
        }
        if (isInEditMode)
        {
#if UNITY_EDITOR
            if (Input.GetKey(KeyCode.W))
            {
                move(direction.forward);
            }
            if (Input.GetKey(KeyCode.S))
            {
                move(direction.back);
            }
            if (Input.GetKey(KeyCode.A))
            {
                move(direction.left);
            }
            if (Input.GetKey(KeyCode.D))
            {
                move(direction.right);
            }
            MousePick();

#else
            //Debug.LogWarning("in edit mode");
            if (!edit.activeInHierarchy)
            {
                if (left.IsFingerDown())
                {
                    //Debug.LogWarning("left finger down");
                    move(mainCamera);
                }
                else
                    MobilePick();
                if (right.IsFingerDown())
                {
                    //Debug.LogWarning("left finger down");
                    rotate();
                }
                else
                    MobilePick();
            }
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    if (edit.activeInHierarchy)
                        edit.SetActive(false);
                }
            
#endif
        }
    }

    #region 点击屏幕选择物体
    void MobilePick()
    {
        if (Input.touchCount != 1)
            return;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray uiRay = UICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit uiHit;
            if (Physics.Raycast(uiRay, out uiHit))
            {
                //Debug.Log(uiHit.transform.name);
                return;
            }

            RaycastHit hit;
            Ray ray = mainCamera.transform.GetComponent<Camera>().ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                //testText = hit.transform.name;
                //Debug.Log(testText);
                //Debug.LogWarning("**********"+hit.transform.name+"++++++++++++");
                if (isTouchJoy)
                    return;
                else
                {
                    pickedSideName = hit.transform.name;
                    pickedModelName = hit.transform.parent.name;
                    if(    pickedSideName.Equals("back")
                        || pickedSideName.Equals("bottom")
                        || pickedSideName.Equals("front")
                        || pickedSideName.Equals("left")
                        || pickedSideName.Equals("right")
                        )
                        edit.SetActive(true);
                }
            }
        }
    }

    void MousePick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray uiRay = UICamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit uiHit;
            if (Physics.Raycast(uiRay, out uiHit))
            {
                testTransform = uiHit.transform;
                Debug.Log(uiHit.transform.name);
                return;
            }
            Ray ray = mainCamera.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //testText = hit.transform.name;
                //Debug.Log(testText);
                pickedSideName = hit.transform.name;
                Debug.Log(pickedSideName);
                pickedModelName = hit.transform.parent.name;
                if (pickedSideName.Equals("back")
                        || pickedSideName.Equals("bottom")
                        || pickedSideName.Equals("front")
                        || pickedSideName.Equals("left")
                        || pickedSideName.Equals("right")
                        )
                edit.SetActive(true);
            }
        }
    }
    #endregion

    #region 移动缩放
    void move(GameObject go)
    {
        float x = left.position.x;
        float y = left.position.y;

        if (x != 0)
        {
            go.transform.Translate(Vector3.right * x * moveSpeed*Time.deltaTime);
        }
        if (y != 0)
        {
            go.transform.Translate(Vector3.forward * y * moveSpeed * Time.deltaTime);
        }
    }
    void rotate()
    {
        float x = right.position.x;
        float y = right.position.y;

        if (x != 0)
        {
            mainCamera.transform.Rotate(Vector3.up * x * zoomSpeed * Time.deltaTime, Space.Self);
        }
        if (y != 0)
        {
            mainCamera.transform.Rotate(Vector3.left * y * zoomSpeed * Time.deltaTime, Space.Self);
        }
    }
    void move(direction d)
    {
        Vector3 moveV = Vector3.forward;
        switch (d)
        {
            case direction.forward: moveV = Vector3.forward; break;
            case direction.back: moveV = Vector3.back; break;
            case direction.left: moveV = Vector3.left; break;
            case direction.right: moveV = Vector3.right; break;
        }
        mainCamera.transform.Translate(moveV * moveSpeed);
    }
    /*
    void zoom(bool up)
    {
        //Debug.Log("zoom");
        Vector3 cPos = mainCamera.transform.position;
        Vector3 offset = new Vector3(0, 0, zoomSpeed);
        if (up)
            mainCamera.transform.position = cPos + offset;
        else
            mainCamera.transform.position = cPos - offset;
    }*/
    #endregion

    #region 改变模式方法
    private void inEditMode(){
        intoEditMode(true);
        isInEditMode = true;
        lastPlayerPos = firstPersonController.transform.position;
        //left = joy.transform.FindChild("LeftJoystick").GetComponent<Joystick>();
        //right = joy.transform.FindChild("RightJoystick").GetComponent<Joystick>();
    }
    private void inSaveMode()
    {
        //Debug.LogWarning("*******xfy****** last player position:" + lastPlayerPos);
        if (lastPlayerPos.y < -6)
            lastPlayerPos.y = -6;
        firstPersonController.transform.position = lastPlayerPos;
        intoEditMode(false);
        isInEditMode = false;
        
    }
    private void intoEditMode(bool active)
    {
        firstPersonController.SetActive(!active);
        mainCamera.SetActive(active);
        //Debug.LogWarning("******xfy***** first persion controller:" + firstPersonController.activeInHierarchy + " camera:" + mainCamera.activeInHierarchy);
        if (!active)
        {
            modeButtonLabel.text = EDITMODE;
            edit.SetActive(false);
        }
        else
        {
            modeButtonLabel.text = SAVEMODE;
        }
        joy.SetActive(active);
    }
    #endregion

    #region 设置贴图 的方法
    public void setTexture(Texture t, string name, string side)
    {
        //Debug.LogWarning("=============set=========");
        if (name.Equals("base"))
        {
            //Debug.LogWarning("=============set base=========");
            baseBox.transform.GetComponent<Box>().setTexture(t, side);
        }
        else
        {
            parent.transform.FindChild(name).GetComponent<Box>().setTexture(t, side);
        }
        inSaveMode();
        
    }
    public void setPictureSize(int width, int height)
    {
        float crown = (float)width / height;
        //Debug.LogWarning("======set======== crown:" + crown);
        setAspect(crown);
        //Debug.LogWarning("======set======== aspectX:" + aspectX);
        baseBox.transform.GetComponent<Box>().setBoxByAspect(aspectX,aspectY);
        int count = parent.transform.childCount;
        //Debug.LogWarning("======set======== count:" + count);
        for (int i = 0; i < count; i++)
        {
            parent.transform.GetChild(i).transform.GetComponent<Box>().setBoxByAspect(aspectX, aspectY);
            //Debug.LogWarning(i);
        }
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
            case SCALE.L3_2: aspectX = 3; aspectY = 2; break;
            case SCALE.L4_3: aspectX = 4; aspectY = 3; break;
            case SCALE.L16_9: aspectX = 16; aspectY = 9; break;
            case SCALE.L16_10: aspectX = 16; aspectY = 10; break;
            case SCALE.P2_3: aspectX = 2; aspectY = 3; break;
            case SCALE.P3_4: aspectX = 3; aspectY = 4; break;
            case SCALE.P9_16: aspectX = 9; aspectY = 16; break;
            case SCALE.P10_16: aspectX = 10; aspectY = 16; break;
        }
    }
    #endregion

    #region button method
    public void onModeButtonClick()
    {
        if (modeButtonLabel.text.Equals(SAVEMODE))
        {
            modeButtonLabel.text = EDITMODE;
            inSaveMode();
        }
        else if (modeButtonLabel.text.Equals(EDITMODE))
        {
            modeButtonLabel.text = SAVEMODE;
            inEditMode();
        }

    }

    public void onPickPictureClick()
    {
        string name = pickedModelName + "*" + pickedSideName;
        NotifyCenter.openGallery(name);
    }

    public void onTakePhotoClick() {
        string name = pickedModelName + "*" + pickedSideName;
        NotifyCenter.openCamera(name);
    }
    #endregion

    void OnGUI()
    {
        if (isTest)
        {
            GUI.Box(new Rect(10, 100, 100, 50), testText);
            if (GUI.Button(new Rect(10, 300, 100, 50), "set"))
            {
                baseBox.transform.GetComponent<Box>().setTexture(testTexture, "front");
            }
            if (GUI.Button(new Rect(10, 400, 100, 50), "rotate"))
            {
                mainCamera.transform.Rotate(Vector3.up * 1 * zoomSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
