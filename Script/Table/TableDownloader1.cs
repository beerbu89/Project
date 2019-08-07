using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using G_Define;

public partial class TableDownloader1 : MonoBehaviour
{
    //Dictionary<eTable, Tuple<ITable, object, string>> m_mapDownloadList = new Dictionary<eTable, Tuple<ITable, object, string>>();
    Dictionary<eTable, Tuple<ITable, object, string, string>> m_mapDownloadList = new Dictionary<eTable, Tuple<ITable, object, string, string>>();

    public float fPercent { get { return (float)m_nSuccessCount / (int)eTable.Max; } }
    public int nAllCount { get { return m_mapDownloadList.Count + 1; } }

    public bool Download()
    {
        if (m_TbWWW == null)
        {
            m_TbWWW = _instance.gameObject.AddComponent<TableWWW>();
        }

        if (m_mapDownloadList.Count == 0 ||
            (m_mapDownloadList.Count + 1) != m_nSuccessCount)
        {
            m_mapDownloadList.Clear();

            //DB            
            string sTbKey = eTableType.GoogleFormDB.ToDesc();
            AddTable<int, LoginData>(sTbKey);
            sTbKey = eTableType.GooglePlayerDB.ToDesc();
            AddTable<int, PlayerData>(sTbKey);

            //SetTables
            string sTableKey = eTableType.Table.ToDesc();
            AddTable<int, PlayerFirstData>(sTableKey);
            AddTable<int, LevelData>(sTableKey);
            AddTable<int, ShopData>(sTableKey);
            AddTable<int, SummonsData>(sTableKey);
            AddTable<int, SummonsUnitData>(sTableKey);
            AddTable<int, HeroData>(sTableKey);
            AddTable<int, QuestData>(sTableKey);
            AddTable<int, MapData>(sTableKey);
            AddTable<int, StageData>(sTableKey);
            AddTable<int, MonsterData>(sTableKey);
            AddTable<int, StageRewardData>(sTableKey);
            AddTable<int, HeroLevelData>(sTableKey);
            AddTable<int, QuestRewardData>(sTableKey);
            AddTable<int, HeroSkillData>(sTableKey);
            AddTable<int, HeroSkillDamageData>(sTableKey);
            AddTable<int, ItemData>(sTableKey);

            //Download Start
            AllRequest();
            return true;
        }

        return false;
    }

    void AddTable<K, V>(string sTableKey) where V : TableBase<K>, new()
    {
        V v = new V();
        var tb = TableMng.Ins.GetITable(v.eTb);
        object obj = new TableDataIO<V>();
        string s = v.sFileName;
        string s2 = sTableKey;
        
        m_mapDownloadList.Add(v.eTb, Tuple.Create<ITable, object, string, string>(tb, obj, s, s2));
    }

    IEnumerator Down()
    {
        AllRequest();
        return null;
    }
  
      

    
}
