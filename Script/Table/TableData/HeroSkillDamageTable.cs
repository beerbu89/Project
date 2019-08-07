using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class HeroSkillDamageData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.SkillDamage;
    public override string sFileName => "SkillDamage.json";

    public int    nID     { get; private set; }
    public string skill   { get; private set; }
    public int    nDamage { get; private set; }

    public HeroSkillDamageData() { }

    public HeroSkillDamageData(string s00, string s01, string s02)
    {
        SetData(s00, s01, s02);
    }

    public HeroSkillDamageData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02)
    {
        nID     = int.Parse(s00);
        skill   = s01;
        nDamage = int.Parse(s02);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2]);
    }
}

public class HeroSkillDamageTable : Table<int, HeroSkillDamageData>
{
}
