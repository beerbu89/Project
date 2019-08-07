using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using G_Define;


public class JsonMng : MonoBehaviour
{
    #region SINGLETON
    static JsonMng _instance = null;

    public static JsonMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(JsonMng)) as JsonMng;
                if (_instance == null)
                {
                    _instance = new GameObject("JsonMng", typeof(JsonMng)).GetComponent<JsonMng>();
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

    //저장할 리스트들
    /*
    플레이어 데이터
    히어로   데이터
    퀘스트   데이터
    타일     데이터
    장비     데이터 
    장비스텟 데이터
    */

    public List<PlayerData> li_PlayerData = new List<PlayerData>();    
    PlayerData       refPlayerData = null;

    List<List<Data>> li_HeroData = new List<List<Data>>();
    List<Data>       li_Hero     = new List<Data>();

    int idx = -1;

    List<List<bool>> li_QuestFlag  = new List<List<bool>>();
    List<List<bool>> li_QuestOpen  = new List<List<bool>>();
    List<bool>       li_Flag       = new List<bool>();
    List<bool>       li_Open       = new List<bool>();

    List<List<Tile>>      li_TileData    = new List<List<Tile>>();
    List<List<StageData>> li_StageData   = new List<List<StageData>>();
    List<Tile>            li_Tile        = new List<Tile>();
    List<StageData>       li_Stage       = new List<StageData>();
    List<int>             li_TileIdx     = new List<int>();

    //장비 key값 
    List<List<int>>          li_EquipKey   = new List<List<int>>();
    //장비 value값
    List<List<List<string>>> li_EquipValue = new List<List<List<string>>>();
    List<int>                li_Key        = new List<int>();
    List<List<string>>       li_Value      = new List<List<string>>();

    //인벤토리 장비(미착용한 장비)
    List<List<string>> li_InvenItem = new List<List<string>>();
    List<string>       li_Item      = new List<string>();

    bool flag = false;

    #region SAVE
    public void PlayerSave()
    {
        //save할 데이터가 기존 데이터라면 flag = true 아니면 false
        for (int i = 0; i < li_PlayerData.Count; ++i)
        {
            idx = i;
            if (li_PlayerData[i].nID == Player.Ins.playerData.nID)
            {
                flag = true;

                li_PlayerData[i] = Player.Ins.playerData;

                break;
            }     
        }

        if (!flag)
        {
            li_PlayerData.Add(Player.Ins.playerData);            
        }

        JsonData json = JsonMapper.ToJson(li_PlayerData);
        File.WriteAllText(Application.dataPath + G_Define.Path.PLAYERDATA, json.ToString());
    }
    public void HeroDataSave()
    {
        li_Hero.Clear();

        var a = Player.Ins.heroDataList.GetEnumerator();
        
        while (a.MoveNext())
        {
            var b = a.Current;

            li_Hero.Add(b);
        }
    
        if (idx > li_PlayerData.Count) { return; }

        Save(li_HeroData,li_Hero);

        //if (flag) { li_HeroData[idx] = li_Hero; }
        //else
        //{
        //    li_HeroData.Add(li_Hero);
        //}

        JsonData json = JsonMapper.ToJson(li_HeroData);
        File.WriteAllText(Application.dataPath + G_Define.Path.HERODATA, json.ToString());
    }
    public void QuestDataSave()
    {
        li_Flag.Clear();
        li_Open.Clear();

        if (idx > li_PlayerData.Count) { return; }

        li_Flag = Player.Ins.questFlagList;
        li_Open = Player.Ins.questOpenList;

        Save(li_QuestFlag,li_Flag);
        Save(li_QuestOpen, li_Open);      

        JsonData json = JsonMapper.ToJson(li_QuestFlag);
        File.WriteAllText(Application.dataPath + G_Define.Path.QUESTFLAGEDATA, json.ToString());

        json = JsonMapper.ToJson(li_QuestOpen);
        File.WriteAllText(Application.dataPath + G_Define.Path.QUESTOPENDATA, json.ToString());
    }
    public void TileDataSave()
    {
        li_Tile.Clear();
        li_Stage.Clear();

        if (idx > li_PlayerData.Count) { return; }

        li_Tile  = GameMng.Ins.tile;
        li_Stage = GameMng.Ins.stageData;

        Save(li_TileData, li_Tile);
        Save(li_StageData, li_Stage);
        Save(li_TileIdx);  

        JsonData json = JsonMapper.ToJson(li_TileData);
        File.WriteAllText(Application.dataPath + G_Define.Path.TILEDATA, json.ToString());

        json = JsonMapper.ToJson(li_StageData);
        File.WriteAllText(Application.dataPath + G_Define.Path.STAGEDATA, json.ToString());

        json = JsonMapper.ToJson(li_TileIdx);
        File.WriteAllText(Application.dataPath + G_Define.Path.TILEIDX, json.ToString());

    }

    public void EquipSave()
    {
        li_Key.Clear();
        li_Value.Clear();

        if (idx > li_PlayerData.Count) { return; }

        if(Player.Ins.heroEquipList.Count == 0) { return; }

        foreach (var val in Player.Ins.heroEquipList )
        {
            li_Key.Add(val.Key);
            li_Value.Add(val.Value);
        }

        Save(li_EquipKey, li_Key);
        Save(li_EquipValue, li_Value);

        JsonData json = JsonMapper.ToJson(li_EquipKey);
        File.WriteAllText(Application.dataPath + G_Define.Path.EQUIPDATAKEY, json.ToString());

        json = JsonMapper.ToJson(li_EquipValue);
        File.WriteAllText(Application.dataPath + G_Define.Path.EQUIPDATAVALUE, json.ToString());
    }

    public void InvenItemSave()
    {
        li_Item.Clear();

        if (idx > li_PlayerData.Count) { return; }

        for(int i=0; i< LobbyUIMng.Ins.inven.invenItemSlotList.Count; ++i)
        {
            li_Item.Add(LobbyUIMng.Ins.inven.invenItemSlotList[i].sprite.name);
        }        

        Save(li_InvenItem, li_Item);

        JsonData json = JsonMapper.ToJson(li_InvenItem);
        File.WriteAllText(Application.dataPath + G_Define.Path.INVENITEM, json.ToString());
    }

    #endregion

    #region DATALIST
    //플레이어 전체 데이터 
    public void PlayerLoad()
    {
        string JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.PLAYERDATA);

        JsonData loadPlayerData = JsonMapper.ToObject(JsonString);
        
        if(loadPlayerData.Count != 0)
        {
            for (int i = 0; i < loadPlayerData.Count; ++i)
            {   
                var obj = JsonMapper.ToObject<PlayerData>(loadPlayerData[i].ToJson());
                li_PlayerData.Add(obj);
            }
        }
    }

    //Hero 전체 리스트
    public void HeroDataLoad()
    {
        string JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.HERODATA);

        JsonData loadHeroData = JsonMapper.ToObject(JsonString);

        if(loadHeroData.Count > 0)
        {
            for (int i = 0; i < loadHeroData.Count; ++i)
            {
                //Debug.LogError(loadHeroData[i]["nID"].ToString());
                var obj = JsonMapper.ToObject<List<Data>>(loadHeroData[i].ToJson());
                li_HeroData.Add(obj);
            }
        }
    }

    //Quest 전체 리스트
    public void QuestDataLoad()
    {
        string   JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.QUESTFLAGEDATA);

        JsonData questFlagData = JsonMapper.ToObject(JsonString);

        if(questFlagData.Count > 0)
        {
            for (int i = 0; i < questFlagData.Count; ++i)
            {
                var obj = JsonMapper.ToObject<List<bool>>(questFlagData[i].ToJson());
                li_QuestFlag.Add(obj);
            }
        }
        JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.QUESTOPENDATA);

        JsonData questOpenData = JsonMapper.ToObject(JsonString);

        if(questOpenData.Count > 0)
        {
            for (int i = 0; i < questOpenData.Count; ++i)
            {
                var obj = JsonMapper.ToObject<List<bool>>(questOpenData[i].ToJson());
                li_QuestOpen.Add(obj);
            }
        }
    }

    //Tile 전체 리스트
    public void TileDataLoad()
    {
        string JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.TILEDATA);

        JsonData tileData = JsonMapper.ToObject(JsonString);

        if(tileData.Count > 0)
        {
            for (int i = 0; i < tileData.Count; ++i)
            {
                var obj = JsonMapper.ToObject<List<Tile>>(tileData[i].ToJson());
                li_TileData.Add(obj);
            }
        }

        JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.STAGEDATA);

        JsonData stageData = JsonMapper.ToObject(JsonString);

        if(stageData.Count > 0)
        {
            for (int i = 0; i < stageData.Count; ++i)
            {
                var obj = JsonMapper.ToObject<List<StageData>>(stageData[i].ToJson());
                li_StageData.Add(obj);
            }
        }

        JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.TILEIDX);

        JsonData tileIdxData = JsonMapper.ToObject(JsonString);

        if(tileIdxData.Count > 0)
        {
            for (int i = 0; i < tileIdxData.Count; ++i)
            {
                int temp = int.Parse(tileIdxData[i].ToString());
                li_TileIdx.Add(temp);
            }
        }
    }

    //착용 장비 전체 리스트
    public void EquipDataLoad()
    {
        string JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.EQUIPDATAKEY);

        JsonData equipDataKey = JsonMapper.ToObject(JsonString);

        for(int i=0; i< equipDataKey.Count; ++i)
        {
            var obj = JsonMapper.ToObject<List<int>>(equipDataKey[i].ToJson());
            li_EquipKey.Add(obj);
        }

        JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.EQUIPDATAVALUE);
        JsonData equipDataValue = JsonMapper.ToObject(JsonString);

        for (int i = 0; i < equipDataValue.Count; ++i)
        {
            var obj = JsonMapper.ToObject<List<List<string>>>(equipDataValue[i].ToJson());
            li_EquipValue.Add(obj);
        }
    }

    //인벤 슬롯 장비 전체 리스트
    public void InvenItemDataLoad()
    {
        string JsonString = File.ReadAllText(Application.dataPath + G_Define.Path.INVENITEM);

        JsonData invenData = JsonMapper.ToObject(JsonString);

        for(int i=0; i< invenData.Count; ++i)
        {
            var obj = JsonMapper.ToObject<List<string>>(invenData[i].ToJson());
            li_InvenItem.Add(obj);
        }
    }
    #endregion

    #region DATASEARCH
    public PlayerData PlayerDataSearch(int nID)
    {
        if (li_PlayerData.Count == 0) { Debug.LogError("logic error"); return null; }

        for (int i = 0; i < li_PlayerData.Count; ++i)
        {
            idx = i;
            if (li_PlayerData[i].nID == nID)
            {               
                return refPlayerData = li_PlayerData[i];
            }
        }

        if(refPlayerData == null) { idx = -1; }

        return null;
    }

    public List<Data> HeroDataSearch()
    {
        if (li_HeroData.Count == 0) { return null; }

        if(idx == -1) { return null; }

        for (int i = 0; i < li_HeroData.Count; ++i)
        {
            if (idx == i)
            {
                var heroData = li_HeroData[i];                
                return heroData;
            }
        }

        return null;
    }

    public List<bool> QuestFlagDataSearch()
    {
        if(li_QuestFlag.Count == 0) { Debug.LogError("logic error"); return null; }

        for(int i=0; i<li_QuestFlag.Count; ++i)
        {
            if(idx == i)
            {
                var questFlagData = li_QuestFlag[i];
                return questFlagData;
            }
        }

        return null;
    }

    public List<bool> QuestOpenDataSearch()
    {
        if (li_QuestOpen.Count == 0) { Debug.LogError("logic error"); return null; }

        for (int i = 0; i < li_QuestOpen.Count; ++i)
        {
            if (idx == i)
            {
                var questOpenData = li_QuestOpen[i];
                return questOpenData;
            }
        }

        return null;
    }

    public List<Tile> TileDataSearch()
    {
        if (li_TileData.Count == 0) { Debug.LogError("logic error"); return null; }

        for(int i=0; i<li_TileData.Count; ++i)
        {
            if(idx == i)
            {
                var tileData = li_TileData[i];
                return tileData;
            }
        }

        return null;
    }

    public List<StageData> StageDataSearch()
    {
        if (li_StageData.Count == 0) { Debug.LogError("logic error"); return null; }

        for(int i=0; i<li_StageData.Count; ++i)
        {
            if(idx == i)
            {
                var stageData = li_StageData[i];
                return stageData;
            }
        }

        return null;
    }

    public int TileIdxSearch()
    {
        if (li_TileIdx.Count == 0) { return -1; }

        for(int i=0; i<li_TileIdx.Count; ++i)
        {
            if(idx == i)
            {
                var tileIdx = li_TileIdx[i];
                return tileIdx;
            }
        }

        return -1;
    }

    public List<int> EquipKeySearch()
    {
        if(li_EquipKey.Count == 0) { return null; }

        for(int i=0; i<li_EquipKey.Count; ++i)
        {
            if(idx == i)
            {
                return li_EquipKey[i];
            }
        }

        return null;
    }

    public List<List<string>> EquipValueSearch()
    {
        if(li_EquipValue.Count == 0) { return null; }

        for(int i=0; i<li_EquipValue.Count; ++i)
        {
            if(idx == i)
            {
                return li_EquipValue[i];
            }
        }

        return null;
    }

    public List<string> InvenItemSearch()
    {
        if(li_InvenItem.Count == 0) { return null; }

        for(int i=0; i<li_InvenItem.Count; ++i)
        {
            if(idx == i)
            {
                return li_InvenItem[i];
            }
        }

        return null;
    }

    #endregion

    public void JsonLoad()
    {
        PlayerLoad();
        HeroDataLoad();
        QuestDataLoad();
        TileDataLoad();
        EquipDataLoad();
        InvenItemDataLoad();
    }

    void Save<T>(List<List<T>> li,  List<T> temp)
    {
        if(li.Count == 0) { li.Add(temp); return; }

        if(flag) { li[idx] = temp; }
        else
        {
            li.Add(temp);
        }
    }

    void Save<T>(List<List<List<T>>> li, List<List<T>> temp)
    {
        if(li.Count == 0) { li.Add(temp); return; }

        if (flag) { li[idx] = temp; }
        else
        {
            li.Add(temp);
        }
    }



    //TileIdxSave
    void Save(List<int> tileIdx)
    {
        if(li_TileIdx.Count == 0) { li_TileIdx.Add(0); return; }

        if(!flag) { li_TileIdx.Add(0); }
    }
}
