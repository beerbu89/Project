using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class GameMng : MonoBehaviour
{
    #region SINGLETON

    static GameMng _instance = null;

    public static GameMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(GameMng)) as GameMng;
                if (_instance == null)
                {
                    _instance = new GameObject("GameMng", typeof(GameMng)).GetComponent<GameMng>();
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

    public int                         StageNum       = 0;
    public int                         currentTileIdx = -1;
    public string                      strStageName   = "";
    public List<StageData>             stageData      = new List<StageData>();    
    public List<Tile>                  tile           = new List<Tile>();

    //public Dictionary<eStage, bool>       dli_Quest = new Dictionary<eStage, bool>();
    public Dictionary<eQuestType, eStage> dli_CurrenQuest = new Dictionary<eQuestType, eStage>();

    int left  = 1;
    int right = 1;
    int up    = 10;
    int down  = 10;

    //전체 스테이지 클리어
    public void StageClear()
    {
        if (tile[currentTileIdx].bFinalClear == false) { return; }
        
        if(tile[currentTileIdx].bFinalClear == true)
        {
            if (currentTileIdx - left > 0 && tile[currentTileIdx -left].bOpen == false)
            {   
                tile[currentTileIdx - left].bOpen          = true;
                tile[currentTileIdx - left].bStageClear[0] = true;
            }
            if (currentTileIdx + right < tile.Count && tile[currentTileIdx + right].bOpen == false)
            {
                tile[currentTileIdx + right].bOpen          = true;
                tile[currentTileIdx + right].bStageClear[0] = true;
            }
            if (currentTileIdx - down > 0 && tile[currentTileIdx -down].bOpen == false)
            {
                tile[currentTileIdx - down].bOpen          = true;
                tile[currentTileIdx - down].bStageClear[0] = true;
            }
            if (currentTileIdx + up < tile.Count && tile[currentTileIdx + up].bOpen == false)
            {
                tile[currentTileIdx + up].bOpen          = true;
                tile[currentTileIdx + up].bStageClear[0] = true;
            }
        }

        Quest();
    }

    void Quest()
    {
        eQuestType eType = (eQuestType)tile[currentTileIdx].eTile;

        //bool flag = dli_CurrenQuest.ContainsKey(eType);

        eStage eStage = (eStage)tile[currentTileIdx].stageData.nID;

        if(eStage == eStage.Fire1 || eStage == eStage.Forest1 || eStage == eStage.Ice1)
        {
            dli_CurrenQuest.Add(eType, eStage);
        }
        else if (Player.Ins.questFlagList[tile[currentTileIdx].stageData.nID-1] == true)
        {
            dli_CurrenQuest.Add(eType, eStage);
            
        } 
        
    }

    public void PlayerExpUp()
    {
        if(Player.Ins.playerData.nExp > BattleMng.Ins.levelData.nMaxExp)
        {
            ++Player.Ins.playerData.nLevel;
        }
    }
}

