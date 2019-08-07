using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    #region SINGLETON

    static Player _instance = null;

    public static Player Ins
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType(typeof(Player)) as Player;
                if(_instance == null)
                {
                    _instance = new GameObject("Player", typeof(Player)).GetComponent<Player>();
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

    const string sBASICIVENSLOT_NAME = "Basic Outline 10px - Stroke 8px";

    public PlayerData        playerData    = null;
    //1레벨일때 한번만 씀                   
    public PlayerFirstData   initData      = new PlayerFirstData();
    public LinkedList<Data>  heroDataList  = new LinkedList<Data>();
    public List<Image>       heroImgList   = new List<Image>();
    //영웅 슬롯
    public List<Image>       invenSlotList = new List<Image>();
    //퀘스트 완료인지
    public List<bool>        questFlagList = new List<bool>();
    public List<bool>        questOpenList = new List<bool>();

    //착용장비    
    public Dictionary<int, List<string>> heroEquipList = new Dictionary<int, List<string>>();    

    public void Init(string id,int nID)
    {
        var tb     = ExtensionMethod.GetPlayerTb(id);      
        var initTb = TableMng.Ins.playerFirstTb.GetTable();
        
        if(initTb == null) { Debug.LogError("Init Table Error"); return; }

        if(tb == null)
        {
            playerData = new PlayerData();

            foreach(var val in initTb)
            {
                playerData.nID     = nID;
                playerData.strName = id;
                playerData.nLevel  = val.Value.nLevel;
                playerData.nVigor  = val.Value.nVigor;
                playerData.nMoney  = val.Value.nMoney;
                playerData.nGem    = val.Value.nGem;
                playerData.nExp    = val.Value.nExp;                
            }

            TableDownloader1.Ins.m_TbWWW.PostPlayerDB(eTableType.GooglePlayerForm.ToDesc(), typeof(PlayerData), playerData.PostData());
            //JsonMng.Ins.PlayerSave();

            for(int i=1; i<(int)eStage.Max; ++i)
            {
                questFlagList.Add(false);                
                questOpenList.Add(false);
            }

            //속성별 첫 퀘스트 오픈 questOpenList 인덱스 0부터 시작 eStage는 1부터 시작
            questOpenList[(int)eStage.Forest1 - 1] = true;
            questOpenList[(int)eStage.Fire1 - 1]   = true;
            questOpenList[(int)eStage.Ice1 - 1]    = true;
        }
        else
        {
           //PlayerDataLoad---
            playerData = tb;         
            playerData = JsonMng.Ins.PlayerDataSearch(playerData.nID);

            if(playerData == null)
            {
                playerData = tb;
            }
            //-----;
            
            //HeroDataLoad--
            var DBHeroData = JsonMng.Ins.HeroDataSearch();
            
            if (DBHeroData != null)
            {
                for (int i = 0; i < DBHeroData.Count; ++i)                           
                {
                    heroDataList.AddLast(DBHeroData[i]);
                    Color color = SetHeroProperty((eHeroProperty)DBHeroData[i].eHeroProperty);
                    
                    //hero 속성 보여주는 부분
                    string sHeroColorPath = Path.HEROCOLOR;
                    Image heroColorPrefab = Resources.Load<Image>(sHeroColorPath) as Image;
                    Image heroProperty = Instantiate(heroColorPrefab);
                    heroProperty.color = color;
                    
                    invenSlotList.Add(heroProperty);
                    
                    //hero image
                    string sHeroImagePath = Path.HEROIMAGE;
                    Image heroImagePrefab = Resources.Load<Image>(sHeroImagePath) as Image;
                    Image heroImage = Instantiate(heroImagePrefab);
                    string changeImg = string.Format(Path.IMG, DBHeroData[i].strName);
                    heroImage.sprite = Resources.Load<Sprite>(changeImg) as Sprite;
                    
                    heroImgList.Add(heroImage);
                }
            }
            
            ///------;
            
            //Quest-----
            var DBQuestFlagData = JsonMng.Ins.QuestFlagDataSearch();
            
            if (DBQuestFlagData != null)
            {
                questFlagList = DBQuestFlagData;                
            }
            else
            {
                for (int i = 1; i < (int)eStage.Max; ++i)
                {
                    questFlagList.Add(false);
                }
            }
            var DBQuestOpenData = JsonMng.Ins.QuestOpenDataSearch();
            
            if (DBQuestOpenData != null)
            {
                questOpenList = DBQuestOpenData;                
            }
            else
            {
                for (int i = 1; i < (int)eStage.Max; ++i)
                {
                    questOpenList.Add(false);
                }

                questOpenList[(int)eStage.Forest1 - 1] = true;
                questOpenList[(int)eStage.Fire1 - 1]   = true;
                questOpenList[(int)eStage.Ice1 - 1]    = true;
            }
            //------;

            //Tile------
            var DBTileData = JsonMng.Ins.TileDataSearch();
            
            if (DBTileData != null)
            {
                GameMng.Ins.tile = DBTileData;                
            }        
            
            var DBStageData = JsonMng.Ins.StageDataSearch();
            
            if(DBStageData != null)
            {
                GameMng.Ins.stageData = DBStageData;                
            }           
            
            var DBTileIdx = JsonMng.Ins.TileIdxSearch();   
            
            if(DBTileData == null)
            {
                DBTileIdx = -1;
            }

            if (DBTileIdx != -1)
            {
                GameMng.Ins.currentTileIdx = DBTileIdx;
            }
            //------;

            //heroEquipList------
            var DBHeroEquipKey   = JsonMng.Ins.EquipKeySearch();
            var DBHeroEquipValue = JsonMng.Ins.EquipValueSearch();
            
            if(DBHeroEquipKey != null && DBHeroEquipValue != null)
            {
                for (int i = 0; i < DBHeroEquipKey.Count; ++i)
                {
                    heroEquipList.Add(DBHeroEquipKey[i], DBHeroEquipValue[i]);
                }
            }
            //------;

            //InvenSlotItem------
            var DBInvenSlotItem = JsonMng.Ins.InvenItemSearch();

            if(DBInvenSlotItem != null)
            {
                for (int i = 0; i < DBInvenSlotItem.Count; ++i)
                {
                    var path = string.Format(Path.IMG, DBInvenSlotItem[i]);
                    var img = Resources.Load<Sprite>(path) as Sprite;

                    var path1 = string.Format(Path.IMG, sBASICIVENSLOT_NAME);
                    var img1 = Resources.Load<Sprite>(path1) as Sprite;

                    if (img != img1)
                    {
                        ItemMng.Ins.li_Item.Add(DBInvenSlotItem[i]);
                    }
                }
            }
        }
    }


    Color SetHeroProperty(eHeroProperty eProperty)
    {
        Color blue  = Color.blue;
        Color red   = Color.red;
        Color green = Color.green;
    
        switch (eProperty)
        {
            case eHeroProperty.Ice:
                return blue;
            case eHeroProperty.Fire:
                return red;
            case eHeroProperty.Forest:
                return green;      
        }
    
        return Color.white;
    }
    
    public void AddMoney(int minusGem, int plusMoney)
    {
        playerData.nGem   -= minusGem;
        playerData.nMoney += plusMoney;
        LobbyUIMng.Ins.expendables.ChangeText(LobbyUIMng.Ins.levelData);
    }

    public void AddGem(int minusMoney, int plusGem)
    {
        playerData.nMoney -= minusMoney;
        playerData.nGem   += plusGem;
        LobbyUIMng.Ins.expendables.ChangeText(LobbyUIMng.Ins.levelData);
    }

    public void AddGem(int plusGem)
    {
        playerData.nGem += plusGem;
        
        if(SceneManager.GetActiveScene().buildIndex == (int)eScene.LobbyScene)
        {
            LobbyUIMng.Ins.expendables.ChangeText(LobbyUIMng.Ins.levelData);
        }
    }

    public void MinusMoney(int price)
    {
        playerData.nMoney -= price;
        LobbyUIMng.Ins.expendables.ChangeText(LobbyUIMng.Ins.levelData);
    }

    public void MinusVigor(int nVigor)
    {
        playerData.nVigor -= nVigor;
        BattleUIMng.Ins.vigor.VigorText(BattleMng.Ins.levelData);      
    }

}
