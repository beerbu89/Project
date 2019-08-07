using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class HeroLevelData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.HeroLevel;
    public override string sFileName => "HeroLevel.json";

    public int nID     { get; private set; }
    public int nLevel  { get; private set; }
    public int nMaxExp { get; private set; }

    public HeroLevelData() { }

    public HeroLevelData(string s00, string s01, string s02)
    {
        SetData(s00, s01, s02);
    }

    public HeroLevelData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02)
    {
        nID     = int.Parse(s00);
        nLevel  = int.Parse(s01);
        nMaxExp = int.Parse(s02);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2]);
    }
}

public class HeroLevelTabel : Table<int, HeroLevelData>
{
}
