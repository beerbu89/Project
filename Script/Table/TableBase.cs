using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using G_Define;

public interface ITableBase<T>
{
    T key { get; }
    eTable eTb { get; }
    string sFileName { get; }
}

public interface IMemberList
{
    string ToStringList();
    void SetData(string[] s);
}

public abstract class TableBase<T> : ITableBase<T>, IMemberList
{
    abstract public T key { get; }
    abstract public eTable eTb { get; }
    abstract public string sFileName { get; }

    abstract public void SetData(string[] s);

    public string ToStringList()
    {
        StringBuilder strReturn = new StringBuilder();

        var arProp = this.GetType().GetProperties();

        for (int i = 0; i < arProp.Length; ++i)
        {
            if (arProp[i].PropertyType.IsEnum == true)
            {
                int nVal = (int)(arProp[i].GetValue(this, null));
                strReturn.Append(nVal);
            }
            else if (arProp[i].PropertyType == typeof(bool))
            {
                var val = arProp[i].GetValue(this, null);
                strReturn.Append((val.ToString() == "TRUE") ? 1 : 0);

            }
            else
            {
                strReturn.Append(arProp[i].GetValue(this, null));
            }

            if (i != (arProp.Length - 1))
            {
                strReturn.Append(",");
            }
        }

        return strReturn.ToString();
    }

    public string[] PostData()//프로퍼티 값중 타임스템프 제외, string배열로 만들기 위한 함수
    {
        List<string> liS = new List<string>();

        var arProp = this.GetType().GetProperties();

        for (int i = 1; i < arProp.Length - 3; ++i)// 최초 스탬프제외, -3개 ( TableBase의 인터페이스 3개 프로퍼티 )
        {
            if (arProp[i].PropertyType.IsEnum == true)
            {
                int nVal = (int)(arProp[i].GetValue(this, null));
                liS.Add(nVal.ToString());
            }
            else if (arProp[i].PropertyType == typeof(bool))
            {
                var val = arProp[i].GetValue(this, null);
                liS.Add(((val.ToString() == "TURE") ? 1 : 0).ToString());
            }
            else
            {
                liS.Add(arProp[i].GetValue(this, null).ToString());
            }
        }

        return liS.ToArray();
    }
}