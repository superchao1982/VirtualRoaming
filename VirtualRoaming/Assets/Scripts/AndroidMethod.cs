using UnityEngine;
using System.Collections;
using System.IO;
using System;

public class AndroidMethod : MonoBehaviour
{

    void SetImage(string str)
    {
        //Debug.Log("crown "+str);
        string[] s = str.Split('*');
        Main.ShareInstance.realWidth = Int32.Parse(s[0]);
        Main.ShareInstance.reakHeight = Int32.Parse(s[1]);
        Main.ShareInstance.setPicture();
        //Debug.Log("set image . width: " + Main.ShareInstance.realWidth + " height: " + Main.ShareInstance.reakHeight);
    }
    void LoadImage()
    {
        string[] str = new string[6];
        SetImage(PlayerPrefs.GetString("imagesize"));
        for (int i = 0; i < 6; i++)
        {
            string key = "filename" + i;
            if (PlayerPrefs.HasKey(key))
            {
                str[i] = PlayerPrefs.GetString("filename" + i);
                Debug.Log("xfy-----    filename: " + str[i]);
            }
        }
        //在Android插件中通知Unity开始去指定路径中找图片资源
        //Debug.Log("Android send message");
        StartCoroutine(LoadTexture(str));

    }

    IEnumerator LoadTexture(string[] name)
    {
        string path = "file://" + Application.persistentDataPath + "/";
        
        for (int i = 0; i < name.Length; i++) {
            string pathname;
            pathname = path + name[i];

            Debug.Log("xfy----   load pathname: "+pathname);
            WWW www = new WWW(pathname);
            while (!www.isDone)
            {

            }
            yield return www;
            Main.ShareInstance.texture.Add(www.texture);
        }

        
        //为贴图赋值
        
    }
}