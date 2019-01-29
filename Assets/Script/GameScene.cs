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

    private List<GameObject> MenuList;

    public static bool isMovingPerson;
    public static int MovePoint;

    public Sprite H0;
    public Sprite H1;

	// Use this for initialization
    void Awake()
    {
        m_SocialRadius = GameObject.Find("SocialRadius");
        m_Player = GameObject.Find("Player");
        m_Player.GetComponent<PersonMono>().m_person = Player.getInstance();
        m_Player.GetComponent<PersonMono>().m_person.setMono(m_Player.GetComponent<PersonMono>());
        m_Camera = GameObject.Find("MainCamera").GetComponent<Camera>();
        m_Information = GameObject.Find("Information");
        MenuList = new List<GameObject>();

        for (int i = 0; i < (int)Person.SkillList.None; ++i)
        {
            MenuList.Add(GameObject.Find(((Person.SkillList) i).ToString()));
        }
    }

    void Start () {
        m_Information.SetActive(false);
	    mousePosRecord = Input.mousePosition;

        Person.AddPerson("JK",24);
        Person.showPerson("JK");
        Person.buildRelationShip(m_Player,GameObject.Find("JK"),300);

        Person.updateTalk();
        MovePoint = 6;
    }

    public static void NextYeah()
    {
        for (int i =0;i<Person.persons.Count;++i)
        {
            Person p = Person.persons[i];
            p.Skills.Sort();

            //增长所有人的年龄
            p.p_age += 1;
            //增加技能上限
            for (int j = 0;j<p.Skills.Count;++j)
            {
                Person.Skill sk = p.Skills[j];
                sk.maxSkill += 100;
                p.Skills[j] = sk;
            }
            //增加前三个技能的数值
            for (int j = 0; j < 3; ++j)
            {
                Person.Skill sk = p.Skills[j];
                float t = 0;
                switch (sk.mainType)
                {
                    case Person.SkillType.体能:
                        t = p.b_stamina;
                        break;
                    case Person.SkillType.才艺:
                        t = p.b_acqierement;
                        break;
                    case Person.SkillType.知识:
                        t = p.b_intelligence;
                        break;
                    case Person.SkillType.社交:
                        t = p.b_social;
                        break;
                }

                sk.skill += t;
                p.Skills[j] = sk;
            }
            //增加后续技能的数值
            for (int j = 3; j < p.Skills.Count; ++j)
            {
                Person.Skill sk = p.Skills[j];
                float t = 0;
                switch (sk.mainType)
                {
                    case Person.SkillType.体能:
                        t = p.b_stamina;
                        break;
                    case Person.SkillType.才艺:
                        t = p.b_acqierement;
                        break;
                    case Person.SkillType.知识:
                        t = p.b_intelligence;
                        break;
                    case Person.SkillType.社交:
                        t = p.b_social;
                        break;
                }

                sk.skill += t;
                p.Skills[j] = sk;
            }
        }
        //距离疏远和自动增加技能点
        m_Player.GetComponent<PersonMono>().m_person.UpdateRelationShip();

        //添加新人物
        string newName = "小" + (char) Random.Range('A', 'Z');
        Person.AddPerson(newName,Random.Range(Player.playerInstance.p_age,50));
        Person.showPerson(newName);
        Person.buildRelationShip(m_Player,GameObject.Find(newName),800);
        MovePoint = 6;
    }

    // Update is called once per frame
	void Update () {
		//doInput();
        
	    UpdateMenu();
        PersonMono.UpdatePersonInformation();
	}

    void LateUpdate()
    {
        UpdateSocialRadius();
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

    void UpdateMenu()
    {
        for (int i = 0; i < MenuList.Count; i++)
        {
            //名字
            MenuList[i].transform.Find("Name").gameObject.GetComponent<Text>().text =
                m_Player.GetComponent<PersonMono>().m_person.Skills[i].skillName;
            MenuList[i].transform.Find("Name").gameObject.GetComponent<Text>().color =
                Person.GetSkillColor(m_Player.GetComponent<PersonMono>().m_person.Skills[i].mainType);
            //数值
            MenuList[i].transform.Find("Number").gameObject.GetComponent<Text>().text =
                ((int)m_Player.GetComponent<PersonMono>().m_person.Skills[i].skill).ToString();
            MenuList[i].transform.Find("Number").gameObject.GetComponent<Text>().color =
                Person.GetSkillColor(m_Player.GetComponent<PersonMono>().m_person.Skills[i].mainType);
            //显示条
            float skillRate = m_Player.GetComponent<PersonMono>().m_person.Skills[i].skill /
                              m_Player.GetComponent<PersonMono>().m_person.Skills[i].maxSkill;
            MenuList[i].transform.Find("Image").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_SkillRate", skillRate);
            MenuList[i].transform.Find("Image").gameObject.GetComponent<SpriteRenderer>().material.SetColor("_FillColor",
                Person.GetSkillColor(m_Player.GetComponent<PersonMono>().m_person.Skills[i].mainType));
            //四维
            m_Information.transform.Find("4Base").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Soc",m_Player.GetComponent<PersonMono>().m_person.b_social * 0.01f);
            m_Information.transform.Find("4Base").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Int", m_Player.GetComponent<PersonMono>().m_person.b_intelligence * 0.01f);
            m_Information.transform.Find("4Base").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Acq", m_Player.GetComponent<PersonMono>().m_person.b_acqierement * 0.01f);
            m_Information.transform.Find("4Base").gameObject.GetComponent<SpriteRenderer>().material.SetFloat("_Sta", m_Player.GetComponent<PersonMono>().m_person.b_stamina * 0.01f);
        }

        GameObject hp = GameObject.Find("Heart");
        hp.transform.Find("Heart0").gameObject.GetComponent<Image>().sprite = MovePoint > 0 ? H0 : H1;
        hp.transform.Find("Heart1").gameObject.GetComponent<Image>().sprite = MovePoint > 1 ? H0 : H1;
        hp.transform.Find("Heart2").gameObject.GetComponent<Image>().sprite = MovePoint > 2 ? H0 : H1;
        hp.transform.Find("Heart3").gameObject.GetComponent<Image>().sprite = MovePoint > 3 ? H0 : H1;
        hp.transform.Find("Heart4").gameObject.GetComponent<Image>().sprite = MovePoint > 4 ? H0 : H1;
        hp.transform.Find("Heart5").gameObject.GetComponent<Image>().sprite = MovePoint > 5 ? H0 : H1;
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
        if(m_Information.activeInHierarchy)
            m_Information.SetActive(false);
    }
}
