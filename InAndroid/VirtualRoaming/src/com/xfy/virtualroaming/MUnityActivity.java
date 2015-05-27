package com.xfy.virtualroaming;

import android.content.Context;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

import com.unity3d.player.UnityPlayerActivity;

public class MUnityActivity extends UnityPlayerActivity {
	public static MUnityActivity mInstance;
	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		mInstance = this;
	}
 
	//Unity中会调用这个方法，用于区分打开摄像机 开始本地相册
	 public void getPhoto(String request,String name)
	 {
		 Log.w("xfy android", name);
	         Intent intent = new Intent(this,ViewActivity.class);
	         intent.putExtra("type", request);
	         intent.putExtra("name", name);
	         this.startActivity(intent);
	 }
	 @Override
	 protected void onPause() {
		 super.onPause();
	 }

	 @Override
	 protected void onResume() {
		 super.onResume();
	 }
}
