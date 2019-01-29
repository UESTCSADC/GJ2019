using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkPanel : MonoBehaviour
{
    public GameObject Target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.localPosition = Target.transform.localPosition +  new Vector3(150,100,0);
	    Vector3 vpos = GameScene.m_Camera.WorldToScreenPoint(Target.transform.position);
	}
}
