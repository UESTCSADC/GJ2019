using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour {

    // Use this for initialization
    int i = 0;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 v = new Vector3(-1.6f, -1.45f,0);
            transform.position = v;
            i = 0;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 v = new Vector3(-1.6f, -2.3f, 0);
            transform.position = v;
            i = 1;
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(i==0)
            {
                SceneManager.LoadScene("play");
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
