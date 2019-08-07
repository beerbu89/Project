using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
using G_Define;

public class LoginData : TableBase<int>
{
    public string tStamp { get; private set; }

    [Description("entry.2061238152")]
    public int nID { get; set; }

    [Description("entry.1704834252")]
    public string strName { get; set; }

    [Description("entry.1145778047")]
    public string strPw { get; set; }

    public override int key          => nID;
    public override eTable eTb       => eTable.Login;
    public override string sFileName => "";

    public LoginData() { }

    public LoginData(string s00, string s01, string s02, string s03)
    {
        SetData(s00, s01, s02,s03);
    }

    public LoginData(string[] a_Val)
    {
        SetData(a_Val);
    }

    public void SetData(string s00, string s01, string s02, string s03)
    {
        tStamp  = s00;
        nID     = int.Parse(s01);
        strName = s02;
        strPw   = s03;
    }

    public override void SetData(string[] s)
    {
        SetData(s[0], s[1], s[2], s[3]);
    }

}

public class LoginTable : Table<int,LoginData>
{    
}
