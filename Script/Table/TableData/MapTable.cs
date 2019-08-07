using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class MapData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.Map;
    public override string sFileName => "Map.json";

    public int     nID     { get; private set; }
    public int     nWidth  { get; private set; }
    public int     nHeight { get; private set; }
    public string  sData   { get; private set; }


    public MapData() { }

    public MapData(string s00, string s01, string s02, string s03)
    {
        SetData(s00, s01, s02,s03);
    }

    public MapData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03)
    {
        nID     = int.Parse(s00);
        nWidth  = int.Parse(s01);
        nHeight = int.Parse(s02);
        sData   = s03;
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2],s[3]);
    }
}

public class MapTable : Table<int,MapData>
{
    
}
