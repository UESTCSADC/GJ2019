using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Click : MonoBehaviour {

    // Use this for initialization
    //public Sprite s;
    public Animator an;
    public AudioSource au;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
            an.Play("Click");
            au.Play();
            
        }
	}

  
    
}
