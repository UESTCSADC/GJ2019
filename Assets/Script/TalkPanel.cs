using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkPanel : MonoBehaviour
{
    public GameObject Target;
    public Person p;

    public Person.SkillList s1;
    public Person.SkillList s2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.localPosition = Target.transform.localPosition +  new Vector3(150,100,0);
	    Vector3 vpos = GameScene.m_Camera.WorldToScreenPoint(Target.transform.position);
	}

    public void clickSelection0()
    {
        //计算s1的收益，消耗体力，重新生成对话
        Person player = GameScene.m_Player.GetComponent<PersonMono>().m_person;
        s1 = player.nextTalk;
        float nSkill = player.getSkill(s1.ToString()) + Mathf.Sqrt(p.getSkill(s1.ToString()) * player.getBase(Person.getSkillBase(s1))) / player.getDistance(p);
        player.setSkill(s1.ToString(),nSkill);

        nSkill = p.getSkill(s1.ToString()) + Mathf.Sqrt(player.getSkill(s1.ToString()) * p.getBase(Person.getSkillBase(s1)));
        p.setSkill(s1.ToString(),nSkill);

        for (int i = 0; i < player.friendList.Count; ++i)
        {
            if (player.friendList[i].p2 == p)
            {
                Person.RelationShip rs = player.friendList[i];
                float minus = (Mathf.Pow(2, (rs.Distance / 6.0f * 0.01f)) - 0.8f) * 100.0f;
                rs.Distance -= minus;
                rs.Distance = Mathf.Max(100, rs.Distance);
                player.friendList[i] = rs;
            }
        }

        player.Skills.Sort();

        --GameScene.MovePoint;
        if(GameScene.MovePoint == 0)
            GameScene.NextYeah();
    }

    public void clickSelection1()
    {
        //计算s2的收益，消耗体力，重新生成对话
        Person player = GameScene.m_Player.GetComponent<PersonMono>().m_person;
        s2 = p.nextTalk;
        float nSkill = player.getSkill(s2.ToString()) + Mathf.Sqrt(p.getSkill(s2.ToString()) * player.getBase(Person.getSkillBase(s2)));
        player.setSkill(s2.ToString(), nSkill);

        nSkill = p.getSkill(s2.ToString()) + Mathf.Sqrt(player.getSkill(s2.ToString()) * p.getBase(Person.getSkillBase(s2)));
        p.setSkill(s2.ToString(), nSkill);

        for (int i = 0; i < player.friendList.Count; ++i)
        {
            if (player.friendList[i].p2 == p)
            {
                Person.RelationShip rs = player.friendList[i];
                float minus = (Mathf.Pow(2, (rs.Distance / 6.0f * 0.01f)) - 0.8f) * 100.0f;
                rs.Distance -= minus;
                rs.Distance = Mathf.Max(100, rs.Distance);
                player.friendList[i] = rs;
            }
        }

        --GameScene.MovePoint;
        if (GameScene.MovePoint == 0)
            GameScene.NextYeah();

        player.Skills.Sort();
    }
}
