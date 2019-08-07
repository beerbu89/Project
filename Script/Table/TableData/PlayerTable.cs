using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using G_Define;

public class PlayerData : TableBase<int>
{
    public string tStamp { get; private set; }

    [Description("entry.1405362872")]
    public int nID { get; set; }

    [Description("entry.854920923")]
    public string strName { get; set; }

    [Description("entry.683203615")]
    public int nLevel { get; set; }

    [Description("entry.122011877")]
    public int nVigor { get; set; }

    [Description("entry.1164371186")]
    public int nMoney { get; set; }

    [Description("entry.693441669")]
    public int nGem { get; set; }

    [Description("entry.1247187016")]
    public int nExp { get; set; }

    public override int key          => nID;
    public override eTable eTb       => eTable.Player;
    public override string sFileName => "";

    public PlayerData() { }

    public PlayerData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        SetData(s00, s01, s02, s03, s04, s05, s06,s07);
    }

    public PlayerData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        tStamp  = s00;
        nID     = int.Parse(s01);
        strName = s02;
        nLevel  = int.Parse(s03);
        nVigor  = int.Parse(s04);
        nMoney  = int.Parse(s05);
        nGem    = int.Parse(s06);
        nExp    = int.Parse(s07);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5], s[6],s[7]);
    }

}

public class PlayerTable : Table<int, PlayerData>
{
}
