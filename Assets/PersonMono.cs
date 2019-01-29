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

	// Use this for initialization
    void Awake()
    {
        if (m_PersonInformation == null)
        {
            m_PersonInformation = GameObject.Find("PersonInformation");
            m_PersonInformation.SetActive(false);
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


    private void ButtonLeftClick()
    {
        Debug.Log("Button Left Click");
    }

    private void ButtonRightClick()
    {
        if (!m_PersonInformation.activeInHierarchy)
        {
            m_PersonInformation.SetActive(true);
        }

        float x = Input.mousePosition.x > Screen.width * 0.5 ? 100 : -100;
        float y = Input.mousePosition.y < Screen.height * 0.5 ? 125 : -125;
        m_PersonInformation.transform.localPosition = transform.localPosition + new Vector3(x,y,0);
    }
}
