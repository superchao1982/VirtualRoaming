using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Drawing;
using System.Drawing.Imaging;

public class AndroidMethod : MonoBehaviour
{
    private const string FILENAMEKEY = "filename";
    private const string SIZEKEY = "imagesize";
    private const string MODELNAMEKEY = "name";
    private int width;
    private int height;
    void SetImage(string str)
    {
        string[] s = str.Split('*');
        width = Int32.Parse(s[0]);
        height = Int32.Parse(s[1]);
        //Debug.LogWarning("crown " + width+" "+height);
        //EditMode.shareInstance.setPictureSize(width, height);
        //Debug.LogWarning("=====set image . width:=====");
    }
    public void LoadImage()
    {
        SetImage(PlayerPrefs.GetString(SIZEKEY));
        //Debug.LogWarning("xfy============out");
        if (PlayerPrefs.HasKey(FILENAMEKEY))
        {
            //Debug.LogWarning("xfy============in");
            string fileName = PlayerPrefs.GetString(FILENAMEKEY);
            string modelName = PlayerPrefs.GetString(MODELNAMEKEY);
            //Debug.LogWarning("xfy============ file name:" + fileName + " modelname:" + modelName);
            StartCoroutine(LoadTexture(fileName, modelName));
            //loadImage(fileName, modelName);
        }
    }

    IEnumerator LoadTexture(string fileName,string modelName)
    {
        string[] name = modelName.Split('*');
        string path = "file://" + Application.persistentDataPath + "/";
        string pathname;
        pathname = path + fileName;
        WWW www = new WWW(pathname);
        while (!www.isDone)
        {
                //Debug.LogError("load failed");
        }
        yield return www;
        //Debug.LogWarning("xfy---- texture:" + www.texture + "parent: " + name[0] + " side: " + name[1]);
        EditMode.shareInstance.setTexture(www.texture, name[0], name[1]);
        //EditMode.shareInstance.targetTexture = www.texture;
        //EditMode.shareInstance.name = name[0];
        //EditMode.shareInstance.side = name[1];
        //EditMode.haseTexture = true;
        //Debug.LogWarning("------xfy---- has texture:" + EditMode.haseTexture);
    }
    void loadImage(string fileName, string modelName)
    {
        Debug.LogWarning("1");
        string[] name = modelName.Split('*');
        string path = "file://" + Application.persistentDataPath + "/";
        string pathname;
        pathname = path + fileName;
        Debug.LogWarning("2");
        FileStream fs = new FileStream(pathname, FileMode.Open, FileAccess.Read);
        Debug.LogWarning("3");
        Image img = Image.FromStream(fs);
        Debug.LogWarning("4");
        MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        Debug.LogWarning("5");
        Texture2D tex2 = new Texture2D(width,height);
        tex2.LoadImage(ms.ToArray());
        Debug.LogWarning("6");
        Debug.LogWarning("***************xfy**************");
        EditMode.shareInstance.setTexture(tex2, name[0], name[1]);
        Debug.LogWarning("~~~~~~~~~~~~~~~xfy**************");
    }
}