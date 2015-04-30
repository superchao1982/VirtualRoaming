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
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!hit.gameObject.name.Equals("bottom"))
            Debug.Log("碰撞1:" + hit.gameObject.name);
    }
}
