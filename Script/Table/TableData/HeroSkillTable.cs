using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class HeroSkillData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.HeroSkill;
    public override string sFileName => "HeroSkill.json";

    public int    nID           { get; private set; }    
    public string strSkill      { get; private set; }    

    public HeroSkillData() { }

    public HeroSkillData(string s00, string s01)
    {
        SetData(s00, s01);
    }

    public HeroSkillData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01)
    {
        nID           = int.Parse(s00);        
        strSkill      = s01;      
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1]);
    }
}

public class HeroSkillTable : Table<int, HeroSkillData>
{
    
}
