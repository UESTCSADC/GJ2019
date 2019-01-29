﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click : MonoBehaviour {

    // Use this for initialization
    public Sprite s;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            GameObject g = new GameObject();
            g.AddComponent<SpriteRenderer>();
            g.GetComponent<SpriteRenderer>().sprite = s;
            g.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
            Destroy(g);
        }
	}

    
}
