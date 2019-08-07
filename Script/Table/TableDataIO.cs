using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;
using G_Define;

public interface ITableIO
{
    void Req(string sKey,eTable eVal, TableWWW a_refWWW, System.Action<bool> a_refCallback);
    void LocalReq(string sKey,eTable eVal, System.Action<bool> a_refCallback);

    void FileWrite(string s);
    void FileRead(string s);

    List<object> liList { get; }
}

public class TableDataIO<Tb> : ITableIO
                                where Tb : IMemberList, new()
{
    private readonly List<object> m_liTable = new List<object>();
    public List<object> liList => m_liTable;

    public void Req(string sKey,eTable eVal, TableWWW a_refWWW, Action<bool> a_refCallback)
    {
        a_refWWW.Req<Tb>(sKey,int.Parse(eVal.ToDesc()), m_liTable, a_refCallback);
    }

    public void LocalReq(string sKey,eTable eVal, Action<bool> a_refCallback)
    {
        TableWWW.LocalReq<Tb>(sKey,int.Parse(eVal.ToDesc()), m_liTable, a_refCallback);
    }

    public void FileWrite(string a_sFileName)
    {
        FileStream fs = new FileStream(a_sFileName, FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        for (int i = 0; i < m_liTable.Count; i++)
        {
            var v = (Tb)m_liTable[i];

            sw.WriteLine(v.ToStringList());
        }

        sw.Close(); sw = null;
        fs.Close(); fs = null;
    }

    public void FileRead(string a_sFileName)
    {
        string sRead;
        FileStream fs = new FileStream(a_sFileName, FileMode.Open);
        StreamReader sr = new StreamReader(fs);

        while ((sRead = sr.ReadLine()) != null)
        {
            Tb t = new Tb();
            t.SetData(a_sFileName.Split(','));
            m_liTable.Add(t);
        }

        sr.Close(); sr = null;
        fs.Close(); fs = null;
    }





}
