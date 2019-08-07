using System.Collections;
using System.Collections.Generic;
using G_Define;
using UnityEngine;

public class QuestData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.Quest;
    public override string sFileName => "Quest.json";

    public int nID         { get; private set; }
    public int eQuestType  { get; private set; }
    public string strQuest { get; private set; }

    public QuestData() { }
    
    public QuestData(string s00, string s01, string s02)
    {
        SetData(s00, s01, s02);
    }

    public QuestData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02)
    {
        nID        = int.Parse(s00);
        eQuestType = int.Parse(s01);
        strQuest   = s02;
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2]);
    }
}

public class QuestTable : Table<int,QuestData>
{  
}
