using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class StageRewardData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.StageReward;
    public override string sFileName => "StageReward.json";

    public int nID         { get; private set; }
    public int nMinMoney   { get; private set; }
    public int nMaxMoney   { get; private set; }
    public int nVigor      { get; private set; }
    public int nExp        { get; private set; }
    public string strItem1 { get; private set; }
    public string strItem2 { get; private set; }
    public string strItem3 { get; private set; }

    public StageRewardData() { }

    public StageRewardData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        SetData(s00, s01, s02, s03, s04, s05, s06, s07);
    }

    public StageRewardData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        nID       = int.Parse(s00);
        nMinMoney = int.Parse(s01);
        nMinMoney = int.Parse(s02);
        nVigor    = int.Parse(s03);
        nExp      = int.Parse(s04);
        strItem1  = s05;
        strItem2  = s06;
        strItem3  = s07;
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2],s[3],s[4], s[5], s[6], s[7]);
    }
}


public class StageRewardTable :Table<int,StageRewardData>
{
}



