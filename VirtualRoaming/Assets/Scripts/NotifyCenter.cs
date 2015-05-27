using UnityEngine;
using System.Collections;

public class NotifyCenter {
    private static AndroidJavaObject _plugin;

    static NotifyCenter()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // find the plugin instance
        using (var pluginClass = new AndroidJavaClass("com.xfy.virtualroaming.MUnityActivity"))
            _plugin = pluginClass.GetStatic<AndroidJavaObject>("mInstance");
    }

    //public static void Reset()
    //{

    //    if (Application.platform != RuntimePlatform.Android)
    //        return;
    //    _plugin.Call("TakePhoto", "Reset");
    //}
    public static void openCamera()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        _plugin.Call("getPhoto", "takePhoto");
    }
    public static void openCamera(string name)
    {
        Debug.LogWarning("xfy======== model name when open camera:" + name);
        if (Application.platform != RuntimePlatform.Android)
            return;
        _plugin.Call("getPhoto", "takePhoto",name);
    }
    public static void openGallery(string name)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        _plugin.Call("getPhoto", "pickPhoto", name);
    }
}
