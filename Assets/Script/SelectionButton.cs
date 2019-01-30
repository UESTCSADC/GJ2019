using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static TalkPanel PlayerTalkPanel;
    private static TalkPanel Selection;
    public Person.SkillList s;
    public Person talkTarget;
    private static MyDataBase myData;

    void Awake()
    {
        if (myData == null)
            myData = Resources.Load<MyDataBase>("MyDataBase");
        if (PlayerTalkPanel == null)
            PlayerTalkPanel = GameObject.Find("PlayerTalkPanel").GetComponent<TalkPanel>();
        if (Selection == null)
            Selection = GameObject.Find("Selection").GetComponent<TalkPanel>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(s!=Person.SkillList.None)
            PlayerTalkPanel.OpenTalkPanel(s.ToString() + "来不来");
        else
            PlayerTalkPanel.OpenTalkPanel("小老弟你怎么回事啊？");
    }

    public void clickSelection()
    {
        if (s == Person.SkillList.None)
        {
            //关闭对话
            PersonMono.clickNothing();
            return;
        }

        //计算s1的收益，消耗体力，重新生成对话
        Person player = GameScene.m_Player.GetComponent<PersonMono>().m_person;
        float nSkill = player.getSkill(s.ToString()) + Mathf.Sqrt(talkTarget.getSkill(s.ToString()) * player.getBase(Person.getSkillBase(s))) / player.getDistance(talkTarget);
        player.setSkill(s.ToString(), nSkill);

        nSkill = talkTarget.getSkill(s.ToString()) + Mathf.Sqrt(player.getSkill(s.ToString()) * talkTarget.getBase(Person.getSkillBase(s)));
        talkTarget.setSkill(s.ToString(), nSkill);

        for (int i = 0; i < player.friendList.Count; ++i)
        {
            if (player.friendList[i].p2 == talkTarget)
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
        if (GameScene.MovePoint == 0)
            GameScene.NextYeah();

        PersonMono.clickNothing();

        PlayerTalkPanel.OpenTalkPanel("超爽");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(Selection.show)
            PlayerTalkPanel.CloseTlkPanel();
    }
}
