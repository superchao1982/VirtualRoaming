using UnityEngine;
using System.Collections;

public class CubeScript : MonoBehaviour {
    Material cubeM = null;
	// Use this for initialization
	void Start () {
        cubeM = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
	}
	
	// Update is called once per frame
	void Update () {
        if (Main.ShareInstance.texture != null)
        {
            //cubeM.mainTexture = Main.ShareInstance.texture;
            //cubeM.
        }
	}
}
