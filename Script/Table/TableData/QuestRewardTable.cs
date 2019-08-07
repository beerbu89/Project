﻿using System.Collections;
using System.Collections.Generic;
using G_Define;

public class QuestRewardData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.QuestReward;
    public override string sFileName => "QuestReward.json";

    public int    nID        { get; private set; }
    public int    eType      { get; private set; }
    public string strQuest   { get; private set; }
    public int    nRewardGem { get; private set; }

    public QuestRewardData() { }

    public QuestRewardData(string s00, string s01, string s02, string s03)
    {
        SetData(s00, s01, s02,s03);
    }

    public QuestRewardData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03)
    {
        nID        = int.Parse(s00);
        eType      = int.Parse(s01);
        strQuest   = s02;
        nRewardGem = int.Parse(s03);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2],s[3]);
    }
}

public class QuestRewardTable : Table<int, QuestRewardData>
{
}
