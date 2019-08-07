using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class StageData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.Stage;
    public override string sFileName => "Stage.json";

    public int    nID      { get; private set; }
    public int    eTile    { get; private set; }
    public string strStage { get; private set; }
    public string monster1 { get; private set; }
    public string monster2 { get; private set; }
    public string monster3 { get; private set; }
    public string monster4 { get; private set; }
    public int    nVigor   { get; private set; }

    public StageData() { }

    public StageData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        SetData(s00, s01, s02, s03, s04, s05,s06, s07);
    }

    public StageData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07)
    {
        nID      = int.Parse(s00);
        eTile    = int.Parse(s01);
        strStage = s02;
        monster1 = s03;
        monster2 = s04;
        monster3 = s05;
        monster4 = s06;
        nVigor   = int.Parse(s07);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5], s[6],s[7]);
    }

}


public class StageTable : Table<int,StageData>
{
   
}
