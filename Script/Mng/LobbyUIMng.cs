using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using G_Define;

public class LobbyUIMng : MonoBehaviour
{
    #region SINGLETON
    static LobbyUIMng _instance = null;

    public static LobbyUIMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(LobbyUIMng)) as LobbyUIMng;
                if (_instance == null)
                {
                    _instance = new GameObject("LobbyUIMng", typeof(LobbyUIMng)).GetComponent<LobbyUIMng>();
                }
            }

            return _instance;
        }
    }

    #endregion

    public UI_PlayerInfo    playerInfo;
    public UI_Expendables   expendables;
    public UI_Shop          shop;
    public UI_Summons       summons;
    public UI_Inventory     inven;
    public LevelData        levelData = null;
    public UI_Quest         quest;
    public UI_Message       message;
    public List<Text>       logoQuestText;

    public void Clear()
    {   
        levelData = null;  
    }

    void Start()
    {
        levelData = ExtensionMethod.GetLevelTb(Player.Ins.playerData.nLevel);

        if (levelData == null)
        {
            Debug.LogError("levelDataNull");
            levelData = new LevelData();
            
        }

        playerInfo.PlayerChangeInfo(Player.Ins.playerData, levelData);
        expendables.ChangeText(levelData);

        ShopInit();
        inven.Init();

        eStage eStage = eStage.Forest1;        

        for(int i=0; i<logoQuestText.Count; ++i)
        {
            var data = ExtensionMethod.GetQuestData((int)eStage);

            if (data == null) { Debug.LogError("data error"); return; }

            logoQuestText[i].text = string.Format("{0}",data.strQuest);

            eStage += (int)eStage.Forest4;
        }

        inven.idx = Player.Ins.heroDataList.Count;
    }

    void ShopInit()
    {
        var shopTb = TableMng.Ins.shopTb.GetTable();

        int startIdx = 0;
        int middleIdx = 4;
        int endIdx = 7;
        foreach (var val in shopTb)
        {
            if (middleIdx > endIdx)
            {
                break;
            }
            shop.gemTextList[startIdx].text  = string.Format("{0:##,##0}", val.Value.addMoney);
            shop.gemTextList[middleIdx].text = string.Format("{0:##,##0}", val.Value.priceGem);

            ++startIdx;
            ++middleIdx;
        }

        middleIdx = 4;
        foreach (var val in shopTb)
        {
            if (middleIdx > endIdx)
            {
                startIdx = 0;
                middleIdx = 4;
            }
            shop.goldTextList[startIdx].text  = string.Format("{0:##,##0}", val.Value.addGem);
            shop.goldTextList[middleIdx].text = string.Format("{0:##,##0}", val.Value.priceMoney);

            ++startIdx;
            ++middleIdx;
        }
    }
    
    public void WorldSceneMove()
    {
        Clear();      
        SceneManager.LoadScene((int)eScene.WorldMapScene);        
    }
        
    public void ClickSave()
    {
        JsonMng.Ins.PlayerSave();
        JsonMng.Ins.HeroDataSave();
        JsonMng.Ins.QuestDataSave();
        JsonMng.Ins.TileDataSave();
        JsonMng.Ins.EquipSave();
        JsonMng.Ins.InvenItemSave();
    }

}

