using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDataBase : ScriptableObject
{
    public List<EventData> m_EventList;

}

public class EventData
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