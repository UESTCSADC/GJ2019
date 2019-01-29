using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PersonMono : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    private static GameObject m_PersonInformation;
    public Person m_person;

    public UnityEvent leftClick;
    public UnityEvent rightClick;

    private bool isMoving;

    private static GameObject PersonTalkPanel;
    private static GameObject PlayerTalkPanel;
    private static List<GameObject> PlayerSelection;

    public static void clickNothing()
    {
        PersonTalkPanel.SetActive(false);
        PlayerTalkPanel.SetActive(false);
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
            PersonTalkPanel.SetActive(false);
        }

        if (PlayerTalkPanel == null)
        {
            PlayerTalkPanel = GameObject.Find("PlayerTalkPanel");
            PlayerTalkPanel.GetComponent<TalkPanel>().Target = GameScene.m_Player;
            PlayerTalkPanel.SetActive(false);
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
	    if (isMoving)
	    {
	        Vector3 pPro = GameScene.m_Camera.WorldToScreenPoint(GameScene.m_Player.transform.position);
	        float px = pPro.x;
	        float py = pPro.y;

            Vector3 dir = Vector3.Normalize(new Vector3(Input.mousePosition.x - px,Input.mousePosition.y - py,0));
	        transform.localPosition = m_person.getDistance(Player.getInstance()) * dir;
	    }
	}

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if(m_person.b_name != "Player")
            isMoving = true;
            GameScene.isMovingPerson = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isMoving = false;
            GameScene.isMovingPerson = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
            if (eventData.button == PointerEventData.InputButton.Left)
                leftClick.Invoke();
            else if (eventData.button == PointerEventData.InputButton.Right)
                rightClick.Invoke();
    }

    public void TalkWith()
    {
        //生成对话框
        PersonTalkPanel.SetActive(true);
        PersonTalkPanel.GetComponent<TalkPanel>().Target = gameObject;

        PlayerTalkPanel.SetActive(true);
        //生成选项
    }

    private void ButtonLeftClick()
    {
        TalkWith();
    }

    private void ButtonRightClick()
    {
        if (m_person.b_name != "Player")
        {
            if (!m_PersonInformation.activeInHierarchy)
            {
                m_PersonInformation.SetActive(true);
            }

            float x = Input.mousePosition.x < Screen.width * 0.5 ? 100 : -100;
            float y = Input.mousePosition.y < Screen.height * 0.5 ? 125 : -125;
            m_PersonInformation.transform.parent = transform;
            m_PersonInformation.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}
