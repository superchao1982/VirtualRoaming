using UnityEngine;
using System.Collections;

public class CharacterCollisionDetection : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnCollisionEnter(Collision collisionInfo)
    {
        Debug.Log("碰撞:" + collisionInfo.gameObject.name);
    }
}
