using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    private GameObject m_SocialRadius;
    public static GameObject m_Player;
    public static Camera m_Camera;
    private GameObject m_Information;

    private Vector3 mousePosRecord;
    private bool isInformationOn;
    private bool isOffsetting;

    public static bool isMovingPerson;

	// Use this for initialization
    void Awake()
    {
        m_SocialRadius = GameObject.Find("SocialRadius");
        m_Player = GameObject.Find("Player");
        m_Player.GetComponent<PersonMono>().m_person = Player.getInstance();
        m_Player.GetComponent<PersonMono>().m_person.setMono(m_Player.GetComponent<PersonMono>());
        m_Camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        m_Information = GameObject.Find("Information");
    }

    void Start () {
        m_Information.SetActive(false);
	    mousePosRecord = Input.mousePosition;

        Person.AddPerson("JK",24);
        Person.showPerson("JK");
        Person.buildRelationShip(m_Player,GameObject.Find("JK"),300);
	}

    void FixedStart()
    {

    }

    // Update is called once per frame
	void Update () {
		doInput();
        UpdateSocialRadius();
        
	}

    void LateUpdate()
    {
        Person.DrawRelationShip();
    }

    void doInput()
    {
        if (Input.GetMouseButton(0) && !isMovingPerson)
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

    public void ClickNothing()
    {
        PersonMono.clickNothing();
    }
}
