using UnityEngine;
using System.Collections;

public class PalyAni : MonoBehaviour {

    Animator ani;
	// Use this for initialization
	void Start () {

        ani = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.A))
            ani.SetInteger("weapon", 1);
        else if (Input.GetKey(KeyCode.B))
            ani.SetInteger("weapon", 0);

        if (Input.GetKey(KeyCode.Alpha0))
            ani.SetFloat("speed", 0f);
        else if (Input.GetKey(KeyCode.Alpha1))
            ani.SetFloat("speed", 5f);
        else if (Input.GetKey(KeyCode.Alpha2))
            ani.SetFloat("speed", 8f);
	
	}
}
