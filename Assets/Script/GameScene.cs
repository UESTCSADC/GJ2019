using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    private GameObject m_SocialRadius;
    private GameObject m_Player;
    public static Camera m_Camera;
    private GameObject m_Information;

    private Vector3 mousePosRecord;
    private bool isInformationOn;
    private bool isOffsetting;

    public static bool isMovingPerson;

	// Use this for initialization
	void Start () {
		m_SocialRadius = GameObject.Find("SocialRadius");
        m_Player = GameObject.Find("Player");
	    m_Camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        m_Information = GameObject.Find("Information");

        m_Information.SetActive(false);
	    mousePosRecord = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		doInput();
        UpdateSocialRadius();
	}

    void doInput()
    {
        if (Input.GetMouseButton(0))
        {
            transform.localPosition += Input.mousePosition - mousePosRecord;
        }
        mousePosRecord = Input.mousePosition;
    }

    void UpdateSocialRadius()
    {
        if (m_SocialRadius != null)
        {
            Material mat = m_SocialRadius.GetComponent<Image>().material;
            mat.SetVector("_Center",m_Camera.WorldToScreenPoint(m_Player.transform.position));
        }
    }

    void AddPerson()
    {
        Person J = new Person(5,5,5,5,true,"JK",24);
    }

    public void ClickMenu()
    {
        if (isInformationOn)
        {
            m_Information.SetActive(false);
        }
        else
        {
            m_Information.SetActive(true);
        }

        isInformationOn = !isInformationOn;
    }
}
