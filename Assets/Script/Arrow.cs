using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arrow : MonoBehaviour {

    // Use this for initialization
    int i = 0;
    public AudioSource au;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 v = new Vector3(-3.5f, -1.8f,0);
            transform.position = v;
            i = 0;
            au.Play();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 v = new Vector3(-3.5f, -3.2f, 0);
            transform.position = v;
            i = 1;
            au.Play();
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

    public void EnterGame()
    {
        SceneManager.LoadScene("play");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
