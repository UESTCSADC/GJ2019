using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;
using Random = System.Random;

public class Person
{
    //固定人物
    public Person(float so, float i, float st, float ac, bool se, string name, int age)
    {
        b_social = so;
        b_acqierement = ac;
        b_intelligence = i;
        b_stamina = st;
        b_sexual = se;
        b_name = name;
        p_age = age;
        friendList = new List<RelationShip>();
        InitSkillList();
    }

    //随机人物
    public Person(int age, string name)
    {
        friendList = new List<RelationShip>();
        p_age = age;
        b_name = name;
        RandomBase();
        InitSkillList();
    }

    private void RandomBase()
    {
        //随机四维
        int BaseCount = UnityEngine.Random.Range(150, 250);
        Vector4 base4 = new Vector4(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value,
            UnityEngine.Random.value);
        base4.Normalize();
        float overflow = 0;
        b_acqierement = Mathf.Pow(base4.x, 2) * BaseCount;
        if (b_acqierement > 100)
        {
            overflow = b_acqierement - 100;
            b_acqierement = 100;
        }

        b_intelligence = Mathf.Pow(base4.y, 2) * BaseCount + overflow;
        if (b_intelligence > 100)
        {
            overflow = b_intelligence - 100;
            b_intelligence = 100;
        }

        b_stamina = Mathf.Pow(base4.z, 2) * BaseCount + overflow;
        if (b_stamina > 100)
        {
            overflow = b_stamina - 100;
            b_stamina = 100;
        }

        b_social = Mathf.Pow(base4.w, 2) * BaseCount + overflow;
        if (b_social > 100)
        {
            overflow = b_social - 100;
            b_social = 100;
        }

        if (overflow != 0)
        {
            b_acqierement = Mathf.Min(100, b_acqierement + overflow);
        }
    }

    //基本属性
    public float b_social; //社交
    public float b_intelligence; //智力
    public float b_stamina; //体能
    public float b_acqierement; //才艺
    public bool b_sexual; //性别

    public string b_name; //名字

    public bool isParent; //是不是父母 

    public SkillList nextTalk; //这个人物的下一个话题

    public float getBase(SkillType st)
    {
        switch (st)
        {
            case SkillType.体能:
                return b_stamina;
            case SkillType.才艺:
                return b_acqierement;
            case SkillType.知识:
                return b_intelligence;
            case SkillType.社交:
                return b_social;
            default:
                return 0;
        }
    }

    public static void updateTalk()
    {
        for (int i = 0; i < persons.Count; ++i)
        {
            persons[i].Skills.Sort();
            int n = UnityEngine.Random.Range(0, 3);
            persons[i].nextTalk = (SkillList)Enum.Parse(typeof(SkillList), persons[i].Skills[n].skillName);
        }
    }

//常量系数
    protected const float c_socialSpeed_B = 1.0f;
    protected const float c_socialSpeed_E = 1.0f;

    //人物关系的结构
    public struct RelationShip
    {
        public Person p1;
        public Person p2;
        public float Distance;
        public GameObject LineRender;
    }

    public enum SkillType
    {
        知识 = 0,
        体能 = 1,
        才艺 = 2,
        社交 = 3,
        None = 4
    }

    public enum SkillList
    {
        外语 = 0,
        计算机 = 1,
        科学 = 2,
        人文 = 3,
        杂学 = 4,
        球类 = 5,
        健身 = 6,
        旅游 = 7,
        游戏 = 8,
        武术 = 9,
        音乐 = 10,
        棋艺 = 11,
        绘画 = 12,
        写作 = 13,
        动漫 = 14,
        口才 = 15,
        时尚 = 16,
        领导力 = 17,
        化妆 = 18,
        洞察力 = 19,
        None = 20
    };


    public struct Skill : IComparable<Skill>
    {
        public string skillName;
        public SkillType mainType;
        public SkillType subType;
        public float mainPower;
        public float skill;
        public float maxSkill;

        public Skill(string name, SkillType mt, SkillType st, float mp,float skill,float maxSkill)
        {
            skillName = name;
            mainPower = mp;
            mainType = mt;
            subType = st;
            this.skill = skill;
            this.maxSkill = maxSkill;
        }

        public int CompareTo(Skill s)
        {
            if (skill < s.skill)
                return 1;
            else if (skill == s.skill)
                return 0;
            else
                return -1;
        }
    }

    public List<Skill> Skills;

    private void InitSkillList()
    {
        float top = p_age * 100.0f;
 
        Skills = new List<Skill>()
        {
            new Skill("外语",SkillType.知识,SkillType.None,1.0f,0,top),
            new Skill("计算机",SkillType.知识,SkillType.None,1.0f,0,top),
            new Skill("科学",SkillType.知识,SkillType.None,1.0f,0,top),
            new Skill("人文",SkillType.知识,SkillType.None,1.0f,0,top),
            new Skill("杂学",SkillType.知识,SkillType.None,1.0f,0,top),
            new Skill("球类",SkillType.体能,SkillType.None,1.0f,0,top),
            new Skill("健身",SkillType.体能,SkillType.None,1.0f,0,top),
            new Skill("旅游",SkillType.体能,SkillType.None,1.0f,0,top),
            new Skill("游戏",SkillType.体能,SkillType.None,1.0f,0,top),
            new Skill("武术",SkillType.体能,SkillType.None,1.0f,0,top),
            new Skill("音乐",SkillType.才艺,SkillType.None,1.0f,0,top),
            new Skill("棋艺",SkillType.才艺,SkillType.None,1.0f,0,top),
            new Skill("绘画",SkillType.才艺,SkillType.None,1.0f,0,top),
            new Skill("写作",SkillType.才艺,SkillType.None,1.0f,0,top),
            new Skill("动漫",SkillType.才艺,SkillType.None,1.0f,0,top),
            new Skill("口才",SkillType.社交,SkillType.None,1.0f,0,top),
            new Skill("时尚",SkillType.社交,SkillType.None,1.0f,0,top),
            new Skill("领导力",SkillType.社交,SkillType.None,1.0f,0,top),
            new Skill("化妆",SkillType.社交,SkillType.None,1.0f,0,top),
            new Skill("洞察力",SkillType.社交,SkillType.None,1.0f,0,top)
        };

        int intSkill = UnityEngine.Random.Range(0, 5);
        int staSkill = UnityEngine.Random.Range(5, 10);
        int acqSkill = UnityEngine.Random.Range(10, 15);
        int socSkill = UnityEngine.Random.Range(15, 20);

        float minBase = Mathf.Min(new float[4]
        {
            b_acqierement, b_stamina, b_intelligence, b_social
        });

        //生成三个主要技能
        List<float> maxBase3 = new List<float>();
        if (minBase != b_acqierement)
        {
            Skill sk = Skills[acqSkill];
            sk.skill = b_acqierement * p_age;
            maxBase3.Add(b_acqierement);
            Skills[acqSkill] = sk;
        }
        if (minBase != b_intelligence)
        {
            Skill sk = Skills[intSkill];
            sk.skill = b_intelligence * p_age;
            maxBase3.Add(b_intelligence);
            Skills[intSkill] = sk;
        }
        if (minBase != b_social)
        {
            Skill sk = Skills[socSkill];
            sk.skill = b_social * p_age;
            maxBase3.Add(b_social);
            Skills[socSkill] = sk;
        }
        if (minBase != b_stamina)
        {
            Skill sk = Skills[staSkill];
            sk.skill = b_stamina * p_age;
            maxBase3.Add(b_stamina);
            Skills[staSkill] = sk;
        }
        maxBase3.Sort();
        Skills.Sort();

        //随机剩余技能
        for (int i = 3; i < Skills.Count; ++i)
        {
            Skill sk = Skills[i];
            sk.skill = UnityEngine.Random.Range(0, maxBase3[0]);
            Skills[i] = sk;
        }
    }

    public static Color GetSkillColor(SkillType st)
    {
        switch (st)
        {
            case SkillType.知识:
                return new Color(0.29f, 0.95f, 1.0f);
            case SkillType.体能:
                return new Color(1.0f,0.22f,0);
            case SkillType.才艺:
                return new Color(0,0.72f,0.22f);
            case SkillType.社交:
                return new Color(1.0f,0.0f,1.0f);
            default:
                return Color.black;
        }
    }

    public List<RelationShip> friendList;
    public int p_age;
    private PersonMono m_PersonMono;

    //游戏内的所有关系
    static public List<RelationShip> relationShips = new List<RelationShip>();
    static public List<Person> persons = new List<Person>();

    //方法
    public void UpdateRelationShip()
    {
        //自动增长技能
        for (int i = 0; i < Skills.Count; ++i)
        {
            Skill sk = Skills[i];
            float comu = 0;
            foreach (var relation in friendList)
            {
                if (relation.Distance < 320)
                {
                    comu += getSkill(sk.skillName);
                }
            }
            sk.skill += comu;
            Skills[i] = sk;
        }

        //距离疏远
        for (int i = 0; i < friendList.Count; ++i)
        {
            RelationShip r = friendList[i];
            r.Distance += Mathf.Pow(r.Distance * 0.2f * 0.01f, 2) * 100.0f;
            friendList[i] = r;
        }
    }

    public float getSkill(string name)
    {
        foreach (var sk in Skills)
        {
            if (sk.skillName == name)
                return sk.skill;
        }

        return 0;
    }

    public void setSkill(string name, float c)
    {
        for (int i = 0; i < Skills.Count; ++i)
        {
            if (Skills[i].skillName == name)
            {
                Person.Skill sk = Skills[i];
                sk.skill = c;
                Skills[i] = sk;
                break;
            }
        }
    }

    public static SkillType getSkillBase(SkillList sl)
    {
        if (sl <= SkillList.杂学)
            return SkillType.知识;
        else if (sl <= SkillList.武术)
            return SkillType.体能;
        else if (sl <= SkillList.动漫)
            return SkillType.才艺;
        else
            return SkillType.社交;
    }

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
        foreach (var rs in Player.playerInstance.friendList)
        {
            if (rs.p1.m_PersonMono != null && rs.p2.m_PersonMono != null)
            {
                rs.p2.m_PersonMono.transform.localPosition = rs.Distance * 
                                                        Vector3.Normalize(rs.p2.m_PersonMono.transform.localPosition - rs.p1.m_PersonMono.transform.localPosition);
                LineRenderer lr = rs.LineRender.GetComponent<LineRenderer>();
                lr.SetPosition(0, rs.p1.m_PersonMono.gameObject.transform.position);
                lr.SetPosition(1, rs.p2.m_PersonMono.gameObject.transform.position);
                lr.material.SetFloat("_Length",rs.Distance);
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
        R.LineRender.layer = 5;
        lr.material = Resources.Load<Material>("Line");

        p1.GetComponent<PersonMono>().m_person.friendList.Add(R);
        p2.GetComponent<PersonMono>().m_person.friendList.Add(R);
        relationShips.Add(R);
        
    }
}

