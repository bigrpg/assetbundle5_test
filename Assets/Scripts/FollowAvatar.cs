using UnityEngine;
using System.Collections;

public class FollowAvatar : MonoBehaviour {

    public  GameObject avatar;
    Animator ani;

	// Use this for initialization
	void Start () {
        ani = avatar.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void LateUpdate () {

        transform.position = ani.bodyPosition + new Vector3(2, 1, 2);
	
	}
}
