using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalkPanel : MonoBehaviour
{
    public GameObject Target;
    public Person p;

    public Person.SkillList s1;
    public Person.SkillList s2;

    private float openRate;
    public bool  show;
    private float textRate;

    private string showString;

	// Use this for initialization
	void Start ()
	{
	    Target = GameScene.m_Player;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    transform.localPosition = Target.transform.localPosition + new Vector3(150, 100, 0);
	    transform.localScale = new Vector3(openRate, openRate, 1);
	    Vector3 vpos = GameScene.m_Camera.WorldToScreenPoint(Target.transform.position);

        if (showString!=null)
	    {
	        transform.Find("Text").GetComponent<Text>().text = showString.Substring(0, (int) textRate);
	    }
	}

    void FixedUpdate()
    {
        if (show && openRate < 1.0f)
        {
            openRate += 0.2f;
            openRate = Mathf.Min(openRate, 1.0f);
        }

        if (show && openRate == 1.0f && textRate < showString.Length)
        {
            textRate += 0.2f;
        }

        if (!show && openRate > 0)
        {
            openRate -= 0.2f;
            openRate = Mathf.Max(openRate, 0.0f);
        }
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

    public void OpenTalkPanel(string text)
    {
        show = true;
        showString = text;
        textRate = 0;
        openRate = 0;
    }

    public void CloseTlkPanel()
    {
        show = false;
    }
}
