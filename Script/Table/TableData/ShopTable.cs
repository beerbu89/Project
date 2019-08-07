using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class ShopData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.Shop;
    public override string sFileName => "Shop.json";

    public int nID        { get; private set; }
    public int eCategory  { get; private set; }
    public int priceGem   { get; private set; }
    public int priceMoney { get; private set; }
    public int addGem     { get; private set; }
    public int addMoney   { get; private set; }

    public ShopData() { }

    public ShopData(string s00, string s01, string s02, string s03, string s04, string s05)
    {
        SetData(s00, s01, s02, s03, s04, s05);
    }

    public ShopData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05)
    {
        nID        = int.Parse(s00);
        eCategory  = int.Parse(s01);
        priceGem   = int.Parse(s02);
        priceMoney = int.Parse(s03);
        addGem     = int.Parse(s04);
        addMoney   = int.Parse(s05);

    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5]);
    }
}


public class ShopTable : Table<int, ShopData>
{   
}
