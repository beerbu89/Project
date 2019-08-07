using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class LevelData : TableBase<int>
{
    public override int key          => nLevel;
    public override eTable eTb       => eTable.Level;
    public override string sFileName => "Level.json";

    public int nLevel    { get; private set; }
    public int nMaxExp   { get; private set; }
    public int nMaxVigor { get; private set; }
    

    public LevelData() { }

    public LevelData(string s00, string s01, string s02)
    {
        SetData(s00, s01, s02);
    }

    public LevelData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02)
    {
        nLevel    = int.Parse(s00);
        nMaxExp   = int.Parse(s01);
        nMaxVigor = int.Parse(s02);
        
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2]);
    }
}

public class LevelTable : Table<int,LevelData>
{
}
