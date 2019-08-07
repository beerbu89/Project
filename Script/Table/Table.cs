using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITable
{
    void AddList(List<object> a_refLi);
}

public class Table<K, V> : ITable //key, value
                            where V : TableBase<K>
{
    Dictionary<K, V> m_mapTb = new Dictionary<K, V>();

    public void AddList(List<object> a_refLi)
    {
        foreach (var val in a_refLi)
        {
            V v = (V)val;
            AddTb(v.key, v);
        }
    }

    public Dictionary<K, V> GetTable()
    {
        return m_mapTb;
    }

    public void AddTb(K a_key, V a_val)
    {
        try
        {
            m_mapTb.Add(a_key, a_val);
        }
        catch
        {
            Debug.LogError("fatal error!!! ----- check table");
        }
    }

    public V GetTb(K a_key)
    {
        V returnVal;

        m_mapTb.TryGetValue(a_key, out returnVal);

        return returnVal;
    }
}
