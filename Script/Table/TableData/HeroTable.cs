using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class HeroData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.Hero;
    public override string sFileName => "Hero.json";

    public int    nID           { get; private set; }
    public int    eHeroProperty { get; set; }
    public string strName       { get; private set; }
    public string strPrefab     { get; private set; }
    public int    nAtk          { get; private set; }
    public int    nHp           { get; private set; }
    public int    nDef          { get; private set; }
    public int    nAtkRange     { get; private set; }
    public int    nLevel        { get; private set; }
    public int    nExp          { get; private set; }
    public int  fSpeed        { get; private set; }


    public HeroData() { }

    public HeroData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07, string s08, string s09,string s10)
    {
        SetData(s00, s01, s02, s03, s04, s05, s06, s07, s08, s09,s10);
    }

    public HeroData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07, string s08, string s09,string s10)
    {
        nID           = int.Parse(s00);
        eHeroProperty = int.Parse(s01);
        strName       = s02;
        strPrefab     = s03;
        nAtk          =  int.Parse(s04);
        nHp           = int.Parse(s05);
        nDef          = int.Parse(s06);
        nAtkRange     = int.Parse(s07);
        nLevel        = int.Parse(s08);
        nExp          = int.Parse(s09);
        fSpeed        = int.Parse(s10);

    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7], s[8], s[9],s[10]);
    }
}

public class HeroTable : Table<int, HeroData>
{
}
