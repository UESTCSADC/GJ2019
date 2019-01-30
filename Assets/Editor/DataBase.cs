using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Excel;
using System.Data;
using UnityEditor;
using UnityEngine.EventSystems;


public class DataReader
{ 
    public static int stringToFlag(string s)
    {
        int preN = 0;
        int r = 0;
        for (int i = 0; i < s.Length; ++i)
        {
            if (s[i] == ',')
            {
                string subString = s.Substring(preN, i - preN);
                r |= 1 << int.Parse(subString) - 1;
                preN = i + 1;
                ++i;
            }
        }

        return r;
    }

    public static string excelPath = "/Data.xlsx";

[MenuItem("Assets/Export")]
    public static void GameReadExcel()
    {
        MyDataBase mdb = ScriptableObject.CreateInstance<MyDataBase>();
        mdb.m_EventList = new List<EventData>();
        mdb.m_RewardList = new List<EventReward>();
        mdb.m_DramaList = new List<Drama>();

        FileStream stream = File.Open(Application.dataPath + excelPath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();

        //读表一
        int rows = result.Tables[0].Rows.Count;//获取行数
        for (int i = 2; i < rows; i++)
        {
            string skill = result.Tables[0].Rows[i][0].ToString();
            string skillBase = result.Tables[0].Rows[i][1].ToString();
            for (int k = 0; k < 3; ++k)
            {
                EventData ed = ScriptableObject.CreateInstance<EventData>();

                string eventID = result.Tables[0].Rows[i][k * 6 + 0 + 3].ToString();
                string eventName = result.Tables[0].Rows[i][k * 6 + 1 + 3].ToString();
                string need = result.Tables[0].Rows[i][k * 6 + 2 + 3].ToString();
                string needStage = result.Tables[0].Rows[i][k * 6 + 3 + 3].ToString();
                string preEvent = result.Tables[0].Rows[i][k * 6 + 4 + 3].ToString();
                string eventType = result.Tables[0].Rows[i][k * 6 + 4 + 3].ToString();

                ed.MainSkillBase = (Person.SkillType)Enum.Parse(typeof(Person.SkillType), skillBase);
                ed.MainSkillType = (Person.SkillList) Enum.Parse(typeof(Person.SkillList), skill);
                ed.EventID = int.Parse(eventID);
                ed.EventName = eventName;
                ed.isComplet = false;
                ed.needRate = float.Parse(need);

                ed.needStageFlag = stringToFlag(needStage);
                ed.eventTypeFlag = stringToFlag(eventType);

                mdb.m_EventList.Add(ed);
            }
        }

        //读表二
        rows = result.Tables[1].Rows.Count;//获取行数
        for (int i = 1; i < rows; ++i)
        {
            EventReward eR = ScriptableObject.CreateInstance<EventReward>();
            eR.EventID = int.Parse(result.Tables[1].Rows[i][0].ToString());

            string skillR = result.Tables[1].Rows[i][1].ToString();
            if (skillR != "")
            {
                eR.eventP_Skill = (Person.SkillList) Enum.Parse(typeof(Person.SkillList), skillR);
                eR.eventP_Age = int.Parse(result.Tables[1].Rows[i][2].ToString());
            }
            else
            {
                eR.eventP_Skill = Person.SkillList.None;
                eR.eventP_Age = 0;
            }

            string BaseChangge = result.Tables[1].Rows[i][3].ToString();
            if (BaseChangge != "")
            {
                eR.eventBaseType = (Person.SkillType)Enum.Parse(typeof(Person.SkillType), BaseChangge);
                eR.eventBase = float.Parse(result.Tables[1].Rows[i][4].ToString());
            }
            else
            {
                eR.eventBaseType = Person.SkillType.None;
                eR.eventBase = 0;
            }

            eR.relationTarget = (EventReward.RelationShipChangeTarget)int.Parse(result.Tables[1].Rows[i][5].ToString());
            if ((int) eR.relationTarget < 2)
            {
                eR.targetKey1 = int.Parse(result.Tables[1].Rows[i][6].ToString());
            }
            if ((int) eR.relationTarget == 0)
            {
                eR.targetKey2 = int.Parse(result.Tables[1].Rows[i][7].ToString());
            }
            if ((int) eR.relationTarget != 4)
            {
                eR.relationshipChangeValue = float.Parse(result.Tables[1].Rows[i][8].ToString());
            }

            eR.coreAreaChangge = float.Parse(result.Tables[1].Rows[i][9].ToString());
            eR.baseAreaChangge = float.Parse(result.Tables[1].Rows[i][10].ToString());
            eR.Goal = int.Parse(result.Tables[1].Rows[i][11].ToString());
            eR.Drama = result.Tables[1].Rows[i][12].ToString();

            mdb.m_RewardList.Add(eR);
        }

        //读表三
        rows = result.Tables[2].Rows.Count;//获取行数
        for (int i = 1; i < rows; ++i)
        {
            Drama d = ScriptableObject.CreateInstance<Drama>();
            d.dialoqueList = new List<Dialogue>();
            d.skillType = (Person.SkillList)Enum.Parse(typeof(Person.SkillList), result.Tables[2].Rows[i][0].ToString());

            string dialoques = result.Tables[2].Rows[i][1].ToString();
            for (int s = 0; s < dialoques.Length; ++s)
            {
                //寻找标签
                while (dialoques[s] != '[')
                {
                    ++s;
                }
                int ns = s;
                while (dialoques[ns] != ']')
                {
                    ++ns;
                }

                string talker = dialoques.Substring(s+1, ns - s - 1);
                Dialogue dia = ScriptableObject.CreateInstance<Dialogue>();
                switch (talker)
                {
                    case "pb":
                        dia.talker = Dialogue.Talker.back;
                        break;
                    case "player":
                        dia.talker = Dialogue.Talker.player;
                        break;
                    case "person":
                        dia.talker = Dialogue.Talker.person;
                        break;
                    default:
                        dia.talker = Dialogue.Talker.none;
                        break;
                }

                s = ns;
                while (dialoques[ns] != '[')
                {
                    ++ns;
                    if (ns == dialoques.Length)
                        break;
                }

                string words = dialoques.Substring(s + 1, ns - s - 1);
                dia.words = words;
                d.dialoqueList.Add(dia);

                s = ns - 1;
            }

            mdb.m_DramaList.Add(d);
        }

        AssetDatabase.CreateAsset(mdb, "Assets/Resources/MyDataBase.asset");
        Selection.activeObject = mdb;
    }
}



