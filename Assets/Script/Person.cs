using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

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
    }

    //随机人物
    public Person(int age)
    {

    }

    public Person()
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
    }

    //游戏内属性
    protected Dictionary<Skills, float> p_skillList;
    protected List<RelationShip> friendList;
    protected int p_age;

    //游戏内的所有关系
    static private List<RelationShip> relationShips;

    //方法
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

    static public void buildRelationShip(Person p1, Person p2, float dis)
    {
        RelationShip R = new RelationShip();
        R.p1 = p1;
        R.p2 = p2;
        R.Distance = dis;
        p1.friendList.Add(R);
        p2.friendList.Add(R);
        relationShips.Add(R);
    }
}

public class Player : Person
{
    //单例
    public static Player playerInstance
    {
        get
        {
            if (playerInstance == null)
            {
                playerInstance = new Player();
            }

            return playerInstance;
        }
        set { playerInstance = value; }
    }

    //方法
    float getCoreSocialRadius()
    {
        return b_social * 2.0f;
    }

    float getBaseSocialRadius()
    {
        return b_social * 0.5f;
    }


}