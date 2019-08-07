using System.Collections;
using System.Collections.Generic;
using G_Define;
using UnityEngine;

public class ItemData : TableBase<int>
{
    public override int key => nID;
    public override eTable eTb => eTable.Item;
    public override string sFileName => "Item.json";

    public int nID        { get; private set; }
    public string strItem { get; private set; }
    public string eType   { get; private set; }
    public int nAtk       { get; private set; }
    public int nDef       { get; private set; }
    public int nHP        { get; private set; }

    public ItemData() { }

    public ItemData(string s00, string s01, string s02, string s03, string s04, string s05)
    {
        SetData(s00, s01, s02, s03, s04, s05);
    }

    public ItemData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05)
    {
        nID = int.Parse(s00);
        strItem = s01;
        eType = s02;
        nAtk = int.Parse(s03);
        nDef = int.Parse(s04);
        nHP  = int.Parse(s05);
    }


    public override void SetData(string[] s)
    {
        throw new System.NotImplementedException();
    }
}

public class ItemTable : Table<int,ItemData>
{   
}
