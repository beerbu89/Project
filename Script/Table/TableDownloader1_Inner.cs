using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TableDownloader1 : MonoBehaviour
{
    #region SINGLETON
    public static bool destroyThis = false;

    static TableDownloader1 _instance = null;

    public static TableDownloader1 Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TableDownloader1)) as TableDownloader1;
                if (_instance == null)
                {
                    _instance = new GameObject("TableDownloader1", typeof(TableDownloader1)).GetComponent<TableDownloader1>();

                }
            }

            return _instance;
        }
    }

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    #endregion

    public TableWWW m_TbWWW = null;

    
    int m_nSuccessCount = 0;

    void AllRequest()
    {
        foreach (var val in m_mapDownloadList)
        {
            ITableIO io = (ITableIO)val.Value.Item2;
            io.Req(val.Value.Item4,val.Key, m_TbWWW,
            (bResult) =>
            {
                val.Value.Item1.AddList(io.liList);
                ++m_nSuccessCount;
            });
        }
    }

    void AllFileSave()
    {
        foreach (var val in m_mapDownloadList.Values)
        {
            ITableIO io = (ITableIO)val.Item2;
            io.FileWrite(val.Item3);
        }
    }

    void AllFileRead()
    {
        foreach (var val in m_mapDownloadList.Values)
        {
            ITableIO io = (ITableIO)val.Item2;
            io.FileRead(val.Item3);
        }
    }
}

