package com.xfy.virtualroaming;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.Date;

import com.unity3d.player.UnityPlayer;

import android.app.Activity;
import android.content.Intent;
import android.content.SharedPreferences;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.net.Uri;
import android.os.Bundle;
import android.os.Environment;
import android.provider.MediaStore;
import android.util.Log;
import android.view.KeyEvent;
import android.widget.ImageView;

public class ViewActivity extends Activity {
	ImageView imageView = null;  
	
    public static final int NONE = 0;
    public static final int PHOTOHRAPH = 1;// 拍照
    public static final int PHOTOZOOM = 2; // 读取
    public static final int PHOTORESOULT=3;
    public static final String TAKEPHOTO= "takePhoto";
    public static final String PICKPHOTO= "pickPhoto";

    private static final String IMAGE_UNSPECIFIED = "image/*";  
    private enum SCALE{
    	L3_2	{public float realValue(){return 1.5f;}},
    	L4_3	{public float realValue(){return 1.333f;}},
    	L16_9	{public float realValue(){return 1.778f;}},
    	L16_10	{public float realValue(){return 1.6f;}},
    	P2_3	{public float realValue(){return 0.667f;}},
    	P3_4	{public float realValue(){return 0.75f;}},
    	P9_16	{public float realValue(){return 0.5625f;}},
    	P10_16	{public float realValue(){return 0.625f;}};
    	public abstract float realValue();
    }
    private static String FILE_NAME = "";
//    static int fileNameNum = 0;
    private final static String DATA_URL = "/data/data/";
    private final static String tag = "xfy";
    private int aspectX = 9;
	private int aspectY = 16;
	private int outputX = 450;
	private int outputY = 800;	//800
	private Uri imageUri;
	private String modelName;
	private String path = "/mnt/sdcard/Android/data/com.xfy.virtualroaming/files";
	
	SharedPreferences sp = null;
	
	@Override
	protected void onCreate(Bundle savedInstanceState) {
//		Log.i(tag, "on create");
		super.onCreate(savedInstanceState);
		
//		setContentView(R.layout.main);
		sp = this.getSharedPreferences(getPackageName(), 0);
		
		imageUri = Uri.parse("file:///sdcard/tempBitMap.jpg");
//		imageView = (ImageView) this.findViewById(R.id.imageID);
 
		String type = this.getIntent().getStringExtra("type");
		modelName = this.getIntent().getStringExtra("name");
		Log.w(tag, "modelName"+modelName);
		//在这里判断是打开本地相册还是直接照相
		if(type.equals("takePhoto"))
		{
			openSysCamera();
		}else
		{
			openGallery();
			//清空 初始化
//			sp.edit().clear();
//			fileNameNum = 0;
//			File f = new File(path);
//			deleteFile(f);
//			finish();
		}
 
    }  

	private void deleteFile(File file){
		if(file.exists()){
			if(file.isFile()){
				file.delete();
			}
			else if(file.isDirectory()){
				File files[] = file.listFiles();
				for(int i=0;i<file.length();i++){
					deleteFile(files[i]);
				}
			}
		}
	}
	private void openGallery(){
		Intent intent = new Intent(Intent.ACTION_PICK, null);
        intent.setDataAndType(MediaStore.Images.Media.EXTERNAL_CONTENT_URI, IMAGE_UNSPECIFIED);
        startActivityForResult(intent, PHOTOZOOM);
	}
	private void openSysCamera(){
		Intent intent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        intent.putExtra(MediaStore.EXTRA_OUTPUT, Uri.fromFile(new File(Environment.getExternalStorageDirectory(), "temp.jpg")));
        startActivityForResult(intent, PHOTOHRAPH);
	}
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == NONE)
            return;
        // 拍照
        if (requestCode == PHOTOHRAPH) {
//        	Log.i("xfy","take photo done");
            //设置文件保存路径这里放在跟目录下
            File picture = new File(Environment.getExternalStorageDirectory() + "/temp.jpg");
            setImageOutputSize(Uri.fromFile(picture));
            startPhotoZoom(Uri.fromFile(picture));
        }  
 
        if (data == null)
            return;  
 
        // 读取相册缩放图片
        if (requestCode == PHOTOZOOM) {
//        	Log.i("xfy","pick photo done! uri: "+data.getData());
        	setImageOutputSize(data.getData());
            startPhotoZoom(data.getData());
        }
        // 处理结果
        if (requestCode == PHOTORESOULT) {
            //Bundle extras = data.getExtras();
        	Bitmap tempBitmap = null;
        	try{
        		tempBitmap = BitmapFactory.decodeStream(getContentResolver().openInputStream(imageUri));
        	}
        	catch(FileNotFoundException e){
        		e.printStackTrace();
        	}
        	
//            Log.i("xfy","zoom done!tempBitmap: "+tempBitmap);
            if (tempBitmap != null) {  
 
                //Bitmap photo = extras.getParcelable("data");
//                Log.i("xfy","photo: "+photo);
//		        imageView.setImageBitmap(tempBitmap);  

            	try {
            		SaveBitmap(tempBitmap);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
            }  
            if(!sp.contains("imagesize"))
            	sp.edit().putString("imagesize", outputX+"*"+outputY).commit();
            sp.edit().putString("filename", FILE_NAME).commit();
            sp.edit().putString("name", modelName).commit();
//            fileNameNum++;
//            if(fileNameNum<6){
//            	//继续拍照
////            	TestActivity.mInstance.TakePhoto("takePhoto");
//            	openSysCamera();
//            }
//            else
//            {
//            	//返回到unity
            	UnityPlayer.UnitySendMessage("AndroidMethod","LoadImage",FILE_NAME);//unity端做了适配，
            	finish();
//            }
//            UnityPlayer.UnitySendMessage("AndroidMethod","SetImage",outputX+"*"+outputY);
			//当用户点击返回键是 通知Unity开始在"/mnt/sdcard/Android/data/com.xfy.virtualroaming/files";路径中读取图片资源，并且现在在Unity中
			//Log.i(tag, "Unity Send Message");
//			UnityPlayer.UnitySendMessage("AndroidMethod","LoadImage",FILE_NAME);
			
        }  
 
//        super.onActivityResult(requestCode, resultCode, data);
    }
    private void setAspect(float crown){
    	SCALE result = SCALE.P9_16;
    	float accuracy = 0.01f;
    	for(SCALE s:SCALE.values()){
    		if(Math.abs(crown-s.realValue()) < accuracy)
    			result = s;
    	}
    	switch(result){
    	case L3_2:	aspectX = 3; 	aspectY = 2; break;
    	case L4_3:	aspectX = 4; 	aspectY = 3; break;
    	case L16_9:	aspectX = 16; 	aspectY = 9; break;
    	case L16_10:aspectX = 16; 	aspectY = 10; break;
    	case P2_3:	aspectX = 2; 	aspectY = 3; break;
    	case P3_4:	aspectX = 3; 	aspectY = 4; break;
    	case P9_16:	aspectX = 9; 	aspectY = 16; break;
    	case P10_16:aspectX = 10; 	aspectY = 16; break;
    	}
//    	Log.i(tag, "crown: "+aspectX+":"+aspectY);
    	//return result;
    }
    private void setImageOutputSize(Uri uri){
    	//Log.i("xfy","uri: "+uri);
    	//Log.i("xfy", "uri path: "+uri.getPath());
    	Bitmap p = BitmapFactory.decodeFile(uri.getPath());
    	//try{getContentResolver().openInputStream(uri);}catch(FileNotFoundException e){e.printStackTrace();}
    	//Log.i(tag, "bitmap: "+p);
    	if(p!=null){
    		outputX = p.getWidth();
    		outputY = p.getHeight();
    		float crown = (float)outputX/outputY;
    		setAspect(crown);
//    		Log.i(tag,"outputX: "+outputX+" outputY: "+outputY+" crown: "+crown);
    	}
    	else{
    		Log.i(tag, "bitmap is null,uri: "+uri);
    	}
    }
    public void startPhotoZoom(Uri uri) {
//    	Log.i(tag, "aspectx: "+aspectX+" y: "+aspectY+" outputx: "+outputX+" y: "+outputY);
    	//setImageOutputSize(uri.getPath());
        Intent intent = new Intent("com.android.camera.action.CROP");
        intent.setDataAndType(uri, IMAGE_UNSPECIFIED);
        intent.putExtra("crop", "true");
        // aspectX aspectY 是宽高的比例
        intent.putExtra("aspectX", aspectX);
        intent.putExtra("aspectY", aspectY);
        // outputX outputY 是裁剪图片宽高
        intent.putExtra("outputX", outputX);
        intent.putExtra("outputY", outputY);
        intent.putExtra("scale", true);
        intent.putExtra("return-data", false);
        intent.putExtra(MediaStore.EXTRA_OUTPUT, imageUri);
        intent.putExtra("outputFormat", Bitmap.CompressFormat.JPEG.toString());
        startActivityForResult(intent, PHOTORESOULT);
    }

	public void SaveBitmap(Bitmap bitmap) throws IOException {
//		Log.i("xfy","save bit map");
		FileOutputStream fOut = null;
 
		try {
			  //查看这个路径是否存在，
			  //如果并没有这个路径，
			  //创建这个路径
			  File destDir = new File(path);
			  if (!destDir.exists())
			  {
 
				  destDir.mkdirs();
				  
			  }
			setPNGName();
//			Log.i(tag, path + "/" + FILE_NAME);
//			File testFile = new File(path + "/" + FILE_NAME);
//			Log.i(tag, "canRead: "+testFile.canRead()+" canWrite: "+testFile.canWrite()+" canExecute: "+testFile.canExecute());
			fOut = new FileOutputStream(path + "/" + FILE_NAME) ;
			
		} catch (FileNotFoundException e) {
			e.printStackTrace();
		}
		//将Bitmap对象写入本地路径中，Unity在去相同的路径来读取这个文件
//		boolean isSave = 
		bitmap.compress(Bitmap.CompressFormat.PNG, 100, fOut);
//		Log.i(tag, ""+isSave);
		try {
			fOut.flush();
		} catch (IOException e) {
			e.printStackTrace();
		}
		try {
			fOut.close();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	private void setPNGName(){
		SimpleDateFormat sdf = new SimpleDateFormat("yyyy-MM-dd-HH-mm-ss");  
        String dateNowStr = sdf.format(new Date());  
        FILE_NAME = "image"+dateNowStr+".png";
	}
	@Override
	public boolean onKeyDown(int keyCode, KeyEvent event)
	{
		if (keyCode == KeyEvent.KEYCODE_BACK && event.getRepeatCount() == 0)
		{
////			UnityPlayer.UnitySendMessage("AndroidMethod","SetImage",outputX+"*"+outputY);
////			//当用户点击返回键是 通知Unity开始在"/mnt/sdcard/Android/data/com.xfy.virtualroaming/files";路径中读取图片资源，并且现在在Unity中
////			Log.i(tag, "Unity Send Message");
			UnityPlayer.UnitySendMessage("AndroidMethod","LoadImage","");
			finish();
		}
		return super.onKeyDown(keyCode, event);
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
