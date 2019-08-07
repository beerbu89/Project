using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class Map 
{
    public const char cMAP_SEPERATOR = '/';

    public MapData     mapData    = null;
                                
    public int nX = 0;  //맵 가로 타일 수 
    public int nY = 0;  //맵 세로 타일 수 

    public List<eTile> tileList = new List<eTile>();

    public void Clear()
    {
        mapData = null;

        nX = 0;
        nY = 0;

        tileList.Clear();
    }

    public void Init()
    {
        Clear();

        var tb = TableMng.Ins.mapTb.GetTable();

        if(tb == null) { Debug.LogError("Table Error"); return; }

        foreach(var val in tb)
        {
            mapData = val.Value;
        }

        nX = mapData.nWidth;
        nY = mapData.nHeight;

        string sMap = mapData.sData;
        string[] sDataArray = sMap.Trim().Split(cMAP_SEPERATOR);

        for(int i=0; i<sDataArray.Length; ++i)
        {
            int nTile;

            if(int.TryParse(sDataArray[i],out nTile) == false)
            {
                Debug.LogError("MapData Error");
                return;
            }

            tileList.Add((eTile)nTile);
        }
    }

}
