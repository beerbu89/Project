using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class TableMng : MonoBehaviour
{
    #region SINGLETON
    static TableMng _instance = null;

    public static TableMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(TableMng)) as TableMng;
                if (_instance == null)
                {
                    _instance = new GameObject("TableMng", typeof(TableMng)).GetComponent<TableMng>();
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

    public LoginTable           loginTb           { get; } = new LoginTable();
    public PlayerTable          playerTb          { get; } = new PlayerTable();
    public PlayerFirstTable     playerFirstTb     { get; } = new PlayerFirstTable();
    public LevelTable           levelTb           { get; } = new LevelTable();
    public ShopTable            shopTb            { get; } = new ShopTable();
    public SummonsTable         summonsTb         { get; } = new SummonsTable();
    public SummonsUnitTable     summonsUnitTb     { get; } = new SummonsUnitTable();
    public HeroTable            heroTb            { get; } = new HeroTable();
    public QuestTable           questTb           { get; } = new QuestTable();
    public MapTable             mapTb             { get; } = new MapTable();
    public StageTable           stageTb           { get; } = new StageTable();
    public MonsterTable         monsterTb         { get; } = new MonsterTable();
    public StageRewardTable     stageRewardTb     { get; } = new StageRewardTable();
    public HeroLevelTabel       heroLevelTb       { get; } = new HeroLevelTabel();
    public QuestRewardTable     questRewardTb     { get; } = new QuestRewardTable();
    public HeroSkillTable       heroSkillTb       { get; } = new HeroSkillTable();
    public HeroSkillDamageTable heroSkillDamageTb { get; } = new HeroSkillDamageTable();
    public ItemTable            itemTb            { get; } = new ItemTable();
    

    public int LoginIdx = 0; 

    public ITable GetITable(eTable a_eTable)
    {
        switch (a_eTable)
        {
            case eTable.Login:
                return loginTb;
            case eTable.Player:
                return playerTb;
            case eTable.PlayerFirstInit:
                return playerFirstTb;
            case eTable.Level:
                return levelTb;
            case eTable.Shop:
                return shopTb;
            case eTable.Summons:
                return summonsTb;
            case eTable.SummonsUnit:
                return summonsUnitTb;
            case eTable.Hero:
                return heroTb;
            case eTable.Quest:
                return questTb;
            case eTable.Map:
                return mapTb;
            case eTable.Stage:
                return stageTb;
            case eTable.Monster:
                return monsterTb;
            case eTable.StageReward:
                return stageRewardTb;
            case eTable.HeroLevel:
                return heroLevelTb;
            case eTable.QuestReward:
                return questRewardTb;
            case eTable.HeroSkill:
                return heroSkillTb;
            case eTable.SkillDamage:
                return heroSkillDamageTb;
            case eTable.Item:
                return itemTb;
        }
        return null;
    }

    //로그인
    public LoginData GetValue(string id)
    {
        var tb = loginTb.GetTable();
        LoginData data = null;

        LoginIdx = 0;

        if (tb == null)
        {
            Debug.LogError("TableMngLoginDataError");
            return null;
        }

        foreach(var val in tb)
        {
            if(val.Value.strName == id)
            {   
                data = val.Value;
            }
            ++LoginIdx;
        }

        return data;
    }

    public V GetValue<K,V>(eTable eTable, K key) where V : TableBase<K>
    {
        var tb = GetITable(eTable) as Table<K, V>;

        if (tb == null)
        {
            Debug.LogError("arg error");
            return default(V);
        }

        return tb.GetTb(key);
    }









    public PlayerData GetPlayrtValue(string id)
    {
        var tb = playerTb.GetTable();
        PlayerData data = null;

        if (tb == null)
        {
            Debug.LogError("TableMngPlayerDataError");
            return null;
        }
        
        foreach(var val in tb)
        {
            if(val.Value.strName == id)
            {
                data = val.Value;
            }            
        }

        return data;
    }

    public LevelData GetLevelValue(int level)
    {
        var tb = levelTb.GetTable();
        LevelData data = null;

        if(tb == null)
        {
            Debug.LogError("TableMngLevelDataError");
            return null;
        }

        foreach(var val in tb)
        {
            if(val.Value.nLevel == level)
            {
                data = val.Value;
            }
        }

        return data;

    }

    public SummonsUnitData GetSummonsUnitValue(int eID)
    {
        var tb = summonsUnitTb.GetTable();
        SummonsUnitData data = null;

        if (tb == null)
        {
            Debug.LogError("TableMngSummonsDataError");
            return null;
        }

        foreach(var val in tb)
        {
            if(val.Value.eID == eID)
            {
                data = val.Value;
            }
        }

        return data;
    }

    public void GetQuestValue(int nQuestType)
    {
        var tb = questTb.GetTable();

        //int idx = 0;

        if (tb == null)
        {
            Debug.LogError("TableMngSummonsDataError");
            return;
        }

        foreach (var val in tb)
        {
            if (val.Value.eQuestType == nQuestType)
            {   
                LobbyUIMng.Ins.quest.d_QuestList.Add(val.Value.nID, val.Value.strQuest);
                //++idx;
            }           
        }
    }

    public StageData GetStageValue(eTile tile)
    {
        var tb = stageTb.GetTable();
        StageData data = null;

        int nID = 0;

        if (tb == null)
        {
            Debug.LogError("TableMngStageDataError");
            return null;
        }

        foreach(var val in tb)
        {
            if(val.Value.eTile == (int)tile)
            {
                nID = val.Value.nID;
                break;
            }
        }

        nID = UnityEngine.Random.Range(nID, nID + 3);

        foreach(var val in tb)
        {
            if(val.Value.nID == nID)
            {
                return data = val.Value;
            }
        }

        return null;
    }

    public MonsterData GetMonsterValue(eMonster eMonster)
    {
        var tb = monsterTb.GetTable();
        MonsterData data = null;

        if(tb == null) { Debug.LogError("TableError"); return null; }

        foreach(var val in tb)
        {
            if(eMonster == (eMonster)val.Value.nMonster)
            {
                return data = val.Value;
            }
        }

        return null;
    }

    public StageRewardData GetStageRewardValue(int stageNum)
    {
        var tb = stageRewardTb.GetTable();
        StageRewardData data = null;

        if (tb == null) { Debug.LogError("TableError"); return null; }

        foreach(var val in tb)
        {
            if(val.Value.nID == stageNum)
            {
                return data = val.Value;
            }
        }

        return null;
    }

    public int GetHeroLevelValue(int level)
    {
        var tb = heroLevelTb.GetTable();
        int nMaxExp = 0;

        if (tb == null) { Debug.LogError("TableError"); return -1; }

        foreach(var val in tb)
        {
            if(val.Value.nLevel == level)
            {
                return nMaxExp = val.Value.nMaxExp;
            }
        }

        return -1;
    }

    public QuestData GetQuestData(int nID)
    {
        var tb = questTb.GetTable();
        QuestData data = null;

        if (tb == null) { Debug.LogError("TableError"); return null; }

        foreach (var val in tb)
        {
            if (val.Value.nID == nID)
            {
                return data = val.Value;
            }
        }

        return null;
    }

    public QuestRewardData GetQuestRewardData(int stage)
    {
        var tb = questRewardTb.GetTable();
        QuestRewardData data = null;

        if (tb == null) { Debug.LogError("TableError"); return null; }

        foreach (var val in tb)
        {
            if (val.Value.nID == stage)
            {
                return data = val.Value;
            }
        }

        return null;
    }

}
