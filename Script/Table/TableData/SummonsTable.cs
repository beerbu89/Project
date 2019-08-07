using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class SummonsData : TableBase<int>
{
    public override int key          => nID;
    public override eTable eTb       => eTable.Summons;
    public override string sFileName => "Summons.json";

    public int nID        { get; private set; }
    public int nPercent01 { get; private set; }
    public int nItem01    { get; private set; }
    public int nPercent02 { get; private set; }
    public int nItem02    { get; private set; }
    public int nPercent03 { get; private set; }
    public int nItem03    { get; private set; }
    public int nPercent04 { get; private set; }
    public int nItem04    { get; private set; }
    public int nPercent05 { get; private set; }
    public int nItem05    { get; private set; }
    public int nPercent06 { get; private set; }
    public int nItem06    { get; private set; }
    public int nPercent07 { get; private set; }
    public int nItem07    { get; private set; }
    public int nPercent08 { get; private set; }
    public int nItem08    { get; private set; }
    public int nPercent09 { get; private set; }
    public int nItem09    { get; private set; }
    public int nPercent10 { get; private set; }
    public int nItem10    { get; private set; }

    public SummonsData() { }

    public SummonsData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07, string s08, string s09
                        , string s10, string s11, string s12, string s13, string s14, string s15, string s16, string s17, string s18, string s19
                        , string s20)
    {
        SetData(s00, s01, s02, s03, s04, s05, s06, s07, s08, s09, s10, s11, s12, s13, s14, s15, s16, s17, s18, s19, s20);
    }

    public SummonsData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03, string s04, string s05, string s06, string s07, string s08, string s09
                        , string s10, string s11, string s12, string s13, string s14, string s15, string s16, string s17, string s18, string s19
                        , string s20)
    {
        nID        = int.Parse(s00);
        nPercent01 = int.Parse(s01);
        nItem01    = int.Parse(s02);
        nPercent02 = int.Parse(s03);
        nItem02    = int.Parse(s04);
        nPercent03 = int.Parse(s05);
        nItem03    = int.Parse(s06);
        nPercent04 = int.Parse(s07);
        nItem04    = int.Parse(s08);
        nPercent05 = int.Parse(s09);
        nItem05    = int.Parse(s10);
        nPercent06 = int.Parse(s11);
        nItem06    = int.Parse(s12);
        nPercent07 = int.Parse(s13);
        nItem07    = int.Parse(s14);
        nPercent08 = int.Parse(s15);
        nItem08    = int.Parse(s16);
        nPercent09 = int.Parse(s17);
        nItem09    = int.Parse(s18);
        nPercent10 = int.Parse(s19);
        nItem10    = int.Parse(s20);
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3], s[4], s[5], s[6], s[7], s[8], s[9], s[10], s[11], s[12], s[13], s[14], s[15], s[16], s[17], s[18], s[19], s[20]);
    }
}

public class SummonsTable : Table<int, SummonsData>
{

}
