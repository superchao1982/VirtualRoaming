﻿using UnityEngine;
using System.Collections;

public class SetCence : MonoBehaviour {
    public GameObject baseBox;
    public Transform parent;
    public GameObject prefab;
    public GameObject fpc;
	// Use this for initialization
	void Start () {
        if (shareData.baseScale != Vector3.zero)
        {
            //Debug.Log("h");
            baseBox.GetComponent<Box>().setBox(shareData.baseScale);
            Vector3 p = new Vector3(shareData.basePosition.x, 0, shareData.basePosition.y);
            baseBox.transform.position = p;
            fpc.transform.position = p;
        }
        if (shareData.positions != null)
        {
            for (int i = 0; i < shareData.positions.Count; i++)
            {
                Vector3 p = new Vector3(shareData.positions[i].x, 0, shareData.positions[i].y);
                setBox(i.ToString(), p, shareData.scales[i]);
            }
        }
	}
    void setBox(string name,Vector3 position, Vector3 scale)
    {
        GameObject prego = Instantiate(prefab) as GameObject;
        prego.name = name;
        prego.transform.parent = parent;
        prego.transform.localPosition = position;
        prego.transform.localRotation = parent.localRotation;
        prego.GetComponent<Box>().setBox(scale);
    }
	// Update is called once per frame
	void Update () {
	
	}
    void OnGUI()
    {
    }
}
