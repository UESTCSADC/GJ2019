using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;
using Random = System.Random;

public class Person
{
    //固定人物
    public Person(float so, float i, float st, float ac, bool se, string name,int age)
    {
        b_social = so;
        b_acqierement = ac;
        b_intelligence = i;
        b_stamina = st;
        b_sexual = se;
        b_name = name;
        p_age = age;
        friendList = new List<RelationShip>();
    }

    //随机人物
    public Person(int age,string name)
    {
        friendList = new List<RelationShip>();
        p_age = age;
        b_name = name;
    }

    public void Awake()
    {
        
    }


    //基本属性
    protected float b_social;             //社交
    protected float b_intelligence;       //智力
    protected float b_stamina;            //体能
    protected float b_acqierement;        //才艺
    protected bool b_sexual;             //性别

    public string b_name;                 //名字

    //常量系数
    protected const float c_socialSpeed_B = 1.0f;
    protected const float c_socialSpeed_E = 1.0f;

    //技能列表
    public enum Skills
    {
        
    }

    //人物关系的结构
    public struct RelationShip
    {
        public Person p1;
        public Person p2;
        public float Distance;
        public GameObject LineRender;
    }

    //游戏内属性
    protected Dictionary<Skills, float> p_skillList;
    protected List<RelationShip> friendList;
    protected int p_age;
    private PersonMono m_PersonMono;

    //游戏内的所有关系
    static private List<RelationShip> relationShips = new List<RelationShip>();
    static private List<Person> persons = new List<Person>();

    //方法
    public void init()
    {
        relationShips.Clear();
        persons.Clear();
        persons.Add(Player.getInstance());
    }

    public float getSocialSpeed(Person p)
    {
        return (b_social + p.b_social) * c_socialSpeed_B * 
               Mathf.Exp(c_socialSpeed_E * getDistance(p));
    }

    public float getDistance(Person p)
    {

        foreach (var relationShip in friendList)
        {
            if (p == relationShip.p1 || p == relationShip.p2)
                return relationShip.Distance;
        }

        return 0;
    }

    public void setMono(PersonMono pm)
    {
        m_PersonMono = pm;
    }

    public static GameObject showPerson(string name)
    {
        foreach (var p in persons)
        {
            if (p.b_name == name && p.m_PersonMono == null)
            {
                GameObject pmono = GameObject.Instantiate(Resources.Load("Person") as GameObject);
                pmono.transform.parent = GameObject.Find("Scene").transform;
                pmono.transform.localScale = new Vector3(1,1,1);
                
                pmono.transform.localPosition = new Vector3(UnityEngine.Random.value,UnityEngine.Random.value,0);
                pmono.GetComponent<PersonMono>().m_person = p;
                p.setMono(pmono.GetComponent<PersonMono>());
                pmono.name = name;
                return pmono;
            }
        }

        return null;
    }

    public static void DrawRelationShip()
    {
        foreach (var rs in relationShips)
        {
            if (rs.p1.m_PersonMono != null && rs.p2.m_PersonMono != null)
            {
                LineRenderer lr = rs.LineRender.GetComponent<LineRenderer>();
                lr.SetPosition(0, rs.p1.m_PersonMono.gameObject.transform.position);
                lr.SetPosition(1, rs.p2.m_PersonMono.gameObject.transform.position);
            }
        }
    }

    public static void AddPerson(string name,int age)
    {
        Person p = new Person(age,name);
        persons.Add(p);
    }

    static public void buildRelationShip(GameObject p1, GameObject p2, float dis)
    {
        Person pp1 = p1.GetComponent<PersonMono>().m_person;
        Person pp2 = p2.GetComponent<PersonMono>().m_person;

        RelationShip R = new RelationShip();
        R.p1 = pp1;
        R.p2 = pp2;
        R.Distance = dis;

        if (p1.name == "Player")
        {
            p2.transform.localPosition = Vector3.Normalize(p2.transform.localPosition - p1.transform.localPosition) * dis;
        }

        R.LineRender = new GameObject();
        R.LineRender.AddComponent<LineRenderer>();
        LineRenderer lr = R.LineRender.GetComponent<LineRenderer>();
        lr.startWidth = 0.1f;
        lr.endWidth = 0.1f;
        R.LineRender.layer = 5;

        p1.GetComponent<PersonMono>().m_person.friendList.Add(R);
        p2.GetComponent<PersonMono>().m_person.friendList.Add(R);
        relationShips.Add(R);
        
    }
}

