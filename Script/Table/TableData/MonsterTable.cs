using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class MonsterData : TableBase<int>
{
    public override int    key       => nID;
    public override eTable eTb       => eTable.Monster;
    public override string sFileName => "Monster.json";

    public int    nID        { get; private set; }
    public int    nMonster   { get; private set; }
    public string strName    { get; private set; }
    public int    nAtk       { get; private set; }
    public int    nHp        { get; private set; }
    public int    nDef       { get; private set; }
    public int    nAtkRange  { get; private set; }
    public float  fSpeed     { get; private set; }


    public MonsterData() { }

    public MonsterData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        SetData(s00, s01, s02, s03, s04, s05, s06,s07);
    }

    public MonsterData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        nID       = int.Parse(s00);
        nMonster  = int.Parse(s01);
        strName   = s02;
        nAtk      = int.Parse(s03);
        nHp       = int.Parse(s04);
        nDef      = int.Parse(s05);
        nAtkRange = int.Parse(s06);
        fSpeed    = float.Parse(s07);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5], s[6],s[7]);
    }
}


public class MonsterTable : Table<int, MonsterData>
{  
}
