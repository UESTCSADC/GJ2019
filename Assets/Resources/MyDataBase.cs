using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDataBase : ScriptableObject
{
    public List<EventData> m_EventList;
    public List<EventReward> m_RewardList;
    public List<Drama> m_DramaList;
}

public class EventData : ScriptableObject
{
    public int EventID;
    public bool isComplet;

    public Person.SkillType MainSkillBase;
    public Person.SkillList MainSkillType;
    public string EventName;

    public float needRate;
    public int needStageFlag;
    public int eventTypeFlag;
}

public class EventReward : ScriptableObject
{
    public enum RelationShipChangeTarget 
    {
        Age = 0,
        Profession = 1,
        Sexual = 2,
        All = 3,
        None = 4
    }
    public int EventID;
    public Person.SkillList eventP_Skill;
    public int eventP_Age;

    public Person.SkillType eventBaseType;
    public float eventBase;

    public RelationShipChangeTarget relationTarget;
    public int targetKey1, targetKey2;
    public float relationshipChangeValue;

    public float coreAreaChangge,baseAreaChangge;
    public int Goal;
    public string Drama;
}

public class Drama : ScriptableObject
{
    public Person.SkillList skillType;
    public List<Dialogue> dialoqueList;
}

public class Dialogue : ScriptableObject
{
    public enum Talker
    {
        player = 0,
        person = 1,
        back = 2,
        none = 3
    }

    public Talker talker;
    public string words;
}