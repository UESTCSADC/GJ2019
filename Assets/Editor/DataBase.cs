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

    public static string excelPath = "/DataBase.xlsx";

    [MenuItem("Assets/Export")]
    public static void GameReadExcel()
    {
        MyDataBase mdb = ScriptableObject.CreateInstance<MyDataBase>();
        mdb.m_EventList = new List<EventData>();

        FileStream stream = File.Open(Application.dataPath + excelPath, FileMode.Open, FileAccess.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        DataSet result = excelReader.AsDataSet();
        int columns = result.Tables[0].Columns.Count;//获取列数
        int rows = result.Tables[0].Rows.Count;//获取行数

        //从第二行开始读
        for (int i = 2; i < rows; i++)
        {
            string skill = result.Tables[0].Rows[i][0].ToString();
            string skillBase = result.Tables[0].Rows[i][1].ToString();
            for (int k = 0; k < 3; ++k)
            {
                EventData ed = new EventData();

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
                ed.needRate = int.Parse(need);

                ed.needStageFlag = stringToFlag(needStage);
                ed.eventTypeFlag = stringToFlag(eventType);

                mdb.m_EventList.Add(ed);
            }
        }
    }
}



