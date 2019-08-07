using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class SummonsUnitData : TableBase<int>
{
    public override int key          => eID;
    public override eTable eTb       => eTable.SummonsUnit;
    public override string sFileName => "SummonsUnit.json";

    public int    eID       { get; private set; }
    public string strName   { get; private set; }
    public string strPrefab { get; private set; }


    public SummonsUnitData() { }

    public SummonsUnitData(string s00, string s01, string s02)
    {
        SetData(s00, s01, s02);
    }

    public SummonsUnitData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02)
    {
        eID     = int.Parse(s00);
        strName = s01;
        strName = s02;

    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2]);
    }
}

public class SummonsUnitTable : Table<int, SummonsUnitData>
{
  
}
