using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PersonMono : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler,IDragHandler
{
    private static GameObject m_PersonInformation;
    public Person m_person;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    private bool isMoving;

    private static GameObject PersonTalkPanel;
    private static GameObject PlayerTalkPanel;
    private static List<GameObject> PlayerSelection;

    private bool moved;

    public static void clickNothing()
    {
        PersonTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        PlayerTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        m_PersonInformation.SetActive(false);
    }

    // Use this for initialization
    void Awake()
    {
        if (m_PersonInformation == null)
        {
            m_PersonInformation = GameObject.Find("PersonInformation");
            m_PersonInformation.SetActive(false);
        }

        if (PersonTalkPanel == null)
        {
            PersonTalkPanel = GameObject.Find("PersonTalkPanel");
            PersonTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        }

        if (PlayerTalkPanel == null)
        {
            PlayerTalkPanel = GameObject.Find("PlayerTalkPanel");
            PlayerTalkPanel.GetComponent<TalkPanel>().Target = GameScene.m_Player;
            PlayerTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        }

        leftClick = new UnityEvent();
        rightClick = new UnityEvent();
    }

    void Start()
    {
        leftClick.AddListener(new UnityAction(ButtonLeftClick));
        rightClick.AddListener(new UnityAction(ButtonRightClick));

        isMoving = false;
    }

    // Update is called once per frame
	void Update ()
	{
	   
	}

    public static void UpdatePersonInformation()
    {
        if (m_PersonInformation.activeInHierarchy)
        {
            m_PersonInformation.transform.Find("PersonBase4").GetComponent<SpriteRenderer>().material.
                SetFloat("_Soc", m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_social * 0.01f);
            m_PersonInformation.transform.Find("PersonBase4").GetComponent<SpriteRenderer>().material.
                SetFloat("_Int", m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_intelligence * 0.01f);
            m_PersonInformation.transform.Find("PersonBase4").GetComponent<SpriteRenderer>().material.
                SetFloat("_Acq", m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_acqierement * 0.01f);
            m_PersonInformation.transform.Find("PersonBase4").GetComponent<SpriteRenderer>().material.
                SetFloat("_Sta", m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_stamina * 0.01f);
            m_PersonInformation.transform.Find("Name").GetComponent<Text>().text =
                m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_name;
            m_PersonInformation.transform.Find("Sexual").GetComponent<Text>().text =
                m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.b_sexual ? "女" : "男";
            m_PersonInformation.transform.Find("Age").GetComponent<Text>().text =
                m_PersonInformation.transform.parent.GetComponent<PersonMono>().m_person.p_age.ToString();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(m_person.b_name != "Player")
            isMoving = true;
            GameScene.isMovingPerson = true;
            moved = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMoving = false;
            GameScene.isMovingPerson = false;
            if(!moved)
                ButtonLeftClick();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) ;
        //leftClick.Invoke();
        else if (eventData.button == PointerEventData.InputButton.Right)
            rightClick.Invoke();
    }

    public void TalkWith()
    {
        //生成对话框
        PersonTalkPanel.transform.localPosition = transform.localPosition + new Vector3(150, 100, 0);
        PersonTalkPanel.GetComponent<TalkPanel>().Target = gameObject;
        PersonTalkPanel.GetComponent<TalkPanel>().OpenTalkPanel(m_person.nextTalk.ToString() + "来不来");

        PlayerTalkPanel.GetComponent<TalkPanel>().OpenTalkPanel(GameScene.m_Player.GetComponent<PersonMono>().m_person.nextTalk.ToString() + "来不来");
        //生成选项
        PlayerTalkPanel.transform.Find("Selection0").Find("Text").GetComponent<Text>().text = m_person.nextTalk.ToString();
        PlayerTalkPanel.transform.Find("Selection1").Find("Text").GetComponent<Text>().text =
            GameScene.m_Player.GetComponent<PersonMono>().m_person.nextTalk.ToString();
        PlayerTalkPanel.transform.Find("Selection2").Find("Text").GetComponent<Text>().text = "告辞";
        PlayerTalkPanel.GetComponent<TalkPanel>().p = m_person;
        
    }

    private void ButtonLeftClick()
    {
        if(m_PersonInformation.activeInHierarchy)
            m_PersonInformation.SetActive(false);
        if(m_person.b_name != "Player")
            TalkWith();
    }

    private void ButtonRightClick()
    {
        if(PlayerTalkPanel.GetComponent<TalkPanel>().show)
            PlayerTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        if(PersonTalkPanel.GetComponent<TalkPanel>().show)
            PersonTalkPanel.GetComponent<TalkPanel>().CloseTlkPanel();
        if (m_person.b_name != "Player")
        {
            if (!m_PersonInformation.activeInHierarchy)
            {
                m_PersonInformation.SetActive(true);
            }

            float x = Input.mousePosition.x < Screen.width * 0.5 ? 125 : -125;
            float y = Input.mousePosition.y < Screen.height * 0.5 ? 180 : -180;
            m_PersonInformation.transform.parent = transform;
            m_PersonInformation.transform.localPosition = new Vector3(x, y, 0);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isMoving)
        {
            moved = true;
            Vector3 pPro = GameScene.m_Camera.WorldToScreenPoint(GameScene.m_Player.transform.position);
            float px = pPro.x;
            float py = pPro.y;

            Vector3 dir = Vector3.Normalize(new Vector3(Input.mousePosition.x - px, Input.mousePosition.y - py, 0));
            transform.localPosition = m_person.getDistance(Player.getInstance()) * dir;
        }

    }
}
