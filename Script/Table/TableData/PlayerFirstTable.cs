using System.Collections;
using System.Collections.Generic;
using G_Define;
using UnityEngine;


public class PlayerFirstData : TableBase<int>
{
    public override int key          => nLevel;
    public override eTable eTb       => eTable.PlayerFirstInit;
    public override string sFileName => "PlayerFirstInit.json";

    public int nLevel { get; private set; }
    public int nVigor { get; private set; }
    public int nMoney { get; private set; }
    public int nGem   { get; private set; }
    public int nExp   { get; private set; }

    public PlayerFirstData() { }

    public PlayerFirstData(string s00, string s01, string s02, string s03,string s04)
    {
        SetData(s00, s01, s02, s03,s04);
    }

    public PlayerFirstData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04)
    {
        nLevel = int.Parse(s00);
        nVigor = int.Parse(s01);
        nMoney = int.Parse(s02);
        nGem   = int.Parse(s03);
        nExp   = int.Parse(s04);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3],s[4]);
    }
}

public class PlayerFirstTable : Table<int, PlayerFirstData>
{
}
