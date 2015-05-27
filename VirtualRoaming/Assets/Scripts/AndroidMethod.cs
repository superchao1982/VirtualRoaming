using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class AndroidMethod : MonoBehaviour
{
    private const string FILENAMEKEY = "filename";
    private const string SIZEKEY = "imagesize";
    private const string MODELNAMEKEY = "name";
    void SetImage(string str)
    {
        string[] s = str.Split('*');
        int width = Int32.Parse(s[0]);
        int height = Int32.Parse(s[1]);
        //Debug.LogWarning("crown " + width+" "+height);
        //EditMode.shareInstance.setPictureSize(width,height);
        //Debug.LogWarning("=====set image . width:=====");
    }
    public void LoadImage()
    {
        Debug.LogWarning("=================load image " + "size:" + PlayerPrefs.GetString(SIZEKEY)
            +" filename:"+PlayerPrefs.GetString(FILENAMEKEY)
            +" modelname:"+PlayerPrefs.GetString(MODELNAMEKEY)
            + " haskey:" + PlayerPrefs.HasKey(FILENAMEKEY));
        SetImage(PlayerPrefs.GetString(SIZEKEY));
        //for (int i = 0; i < 6; i++)
        //{
        //    string key = "filename" + i;
        //    if (PlayerPrefs.HasKey(key))
        //    {
        //        str[i] = PlayerPrefs.GetString("filename" + i);
        //        Debug.Log("xfy-----    filename: " + str[i]);
        //    }
        //}
        //int i = 0;
        //string key = "filename" + i;
        //Debug.LogWarning("xfy============out");
        if (PlayerPrefs.HasKey(FILENAMEKEY))
        {
            //Debug.LogWarning("xfy============in");
            string fileName = PlayerPrefs.GetString(FILENAMEKEY);
            string modelName = PlayerPrefs.GetString(MODELNAMEKEY);
            //Debug.LogWarning("xfy============ file name:" + fileName + " modelname:" + modelName);
            StartCoroutine(LoadTexture(fileName, modelName));
        }

        //在Android插件中通知Unity开始去指定路径中找图片资源
        //Debug.Log("Android send message");
        

    }

    IEnumerator LoadTexture(string fileName,string modelName)
    {
        string[] name = modelName.Split('*');
        string path = "file://" + Application.persistentDataPath + "/";
        
        //for (int i = 0; i < name.Length; i++) {
            string pathname;
            pathname = path + fileName;

            //Debug.LogWarning("xfy----   load pathname: "+pathname);
            WWW www = new WWW(pathname);
            while (!www.isDone)
            {
                //Debug.LogError("load failed");
            }
            yield return www;
            //Main.ShareInstance.texture.Add(www.texture);
        //}
            Debug.LogWarning("xfy---- texture:" + www.texture + "parent: " + name[0] + " side: " + name[1]);
            EditMode.shareInstance.setTexture(www.texture,name[0],name[1]);
        
        //为贴图赋值
        
    }
}