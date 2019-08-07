using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;
using System;

public class BattleMng : MonoBehaviour
{
    #region SINGLETON
    static BattleMng _instance = null;

    public static BattleMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BattleMng)) as BattleMng;
                if (_instance == null)
                {
                    _instance = new GameObject("BattleMng", typeof(BattleMng)).GetComponent<BattleMng>();
                }
            }
            return _instance;
        }
    }
    #endregion

    HeroStatCalc heroStatCalc = new HeroStatCalc();

    const int nHERO_SIZE = 4;

    public string           sMonsterName    = "";
    public LevelData        levelData;      
    public int              nVigor          = 0;
    public LinkedList<Data> heroDatas       = new LinkedList<Data>();    
    //던전 입장시
    public Transform[]      heroRespawn     = new Transform[nHERO_SIZE];
    //던전 Battle시
    public Transform[]      heroRespawn1    = new Transform[nHERO_SIZE];
    public Transform        monsterRespawn;
    public Transform        heroRoot;
    public Transform        monsterRoot;
    public GameObject[]     heroSpawnEffect = new GameObject[nHERO_SIZE];
    public Monster          monster         = null;
    public bool             battleStart     = false;
    public List<Hero>       heroList        = new List<Hero>();
    public List<Hero>       inGameHeroList  = new List<Hero>();
    public StageRewardData  stageRewardData = null;
    //hero의 레벨이 2단계업 이상시 expbar돌리기위해
    public int              repeatCount      = 0;
    //전투입장 카메라 위치
    public Vector3          waitCameraPos    = new Vector3(2.3f, 7.5f, 15.4f);
    public Vector3          waitCameraRotX   = new Vector3(57.5932f, 0, 0);
    public Transform        skillRoot;
    //전투에 참여하는 heroIndex
    public List<int>        heroIdxList      = new List<int>();

    Pool         pool           = new Pool();
    Transform[]  heroObjList    = new Transform[nHERO_SIZE];
    //전투씬 카메라 위치
    Vector3      cameraPos      = new Vector3(2.3f, 2.32f, 42.3f);
    Vector3      cameraRotX     = new Vector3(30, 0, 0);
    
    
    public void Clear()
    {
        sMonsterName    = "";
        levelData       = null;
        nVigor          = 0;
        monster         = null;
        battleStart     = false;
        stageRewardData = null;
        repeatCount     = 0;

        heroList.Clear();
        heroDatas.Clear();
        inGameHeroList.Clear();
        heroIdxList.Clear();

        for (int i=0; i< heroSpawnEffect.Length; ++i)
        {
            heroSpawnEffect[i].SetActive(false);
            heroObjList[i] = null;
        }
    }

    private void Start()
    {
        Init();
    }

    void Init()
    {
        if (SceneMng.Ins.sMonsterName == "")
        {
            Debug.LogError("MonsterName Error");
            return;
        }

        sMonsterName = SceneMng.Ins.sMonsterName;

        levelData = ExtensionMethod.GetLevelTb(Player.Ins.playerData.nLevel);

        if (levelData == null) { return; }

        BattleUIMng.Ins.vigor.VigorText(levelData);

        BattleUIMng.Ins.battleWait.Init();
        BattleUIMng.Ins.battleWait.gameObject.SetActive(true);

        nVigor = SceneMng.Ins.nVigor;

        pool.SetPool(heroRoot, monsterRoot);

        stageRewardData = ExtensionMethod.GetStageRewardData(GameMng.Ins.StageNum);

        if (stageRewardData == null) { Debug.LogError("Reward Table Error"); return; }
    }

    //던전입장
    public void BattleEntrance()
    {
        if(monster == null)
        {
            sMonsterName = SceneMng.Ins.sMonsterName;
        }

        var list = heroDatas.GetEnumerator();

        int idx     = 0;       

        while (list.MoveNext())
        {
            var data = list.Current;

            eHero eHero = (eHero)Enum.Parse(typeof(eHero), data.strPrefab);

            for(int i=0; i<pool.eHeroList.Count; ++i)
            {
                if(eHero == pool.eHeroList[i])
                {
                    if(pool.heroPoolList[i].gameObject.activeSelf == false)
                    {
                        pool.heroPoolList[i].gameObject.SetActive(true);
                        pool.heroPoolList[i].position = heroRespawn[idx].position;

                        heroObjList[idx] = pool.heroPoolList[i];

                        heroList.Add(pool.heroPoolList[i].GetComponent<Hero>());
                        inGameHeroList.Add(heroList[idx]);
                        
                        heroSpawnEffect[idx].SetActive(true);

                        break;
                    }
                }
            }
            ++idx;
        }


        StartCoroutine(StartDungeon());        
    }

    public void BattleStart()
    {
        int heroIdx = 0;

        for (int i = 0; i < heroList.Count; ++i)
        {
            heroIdx = heroIdxList[i];
            heroStatCalc.AddHeroStat(heroIdx, heroList[i]);
        }

        BattleUIMng.Ins.topBattleUI.SetActive(true);
        BattleUIMng.Ins.bottomBattleUI.SetActive(true);

        Camera.main.transform.eulerAngles = cameraRotX;
        Camera.main.transform.position    = cameraPos;
        
        for (int i = 0; i < pool.monsterPoolList.Count; ++i)
        {
            eMonster eMonster = (eMonster)Enum.Parse(typeof(eMonster), sMonsterName);

            if (eMonster == pool.eMonsterList[i])
            {
                pool.monsterPoolList[i].gameObject.SetActive(true);
                pool.monsterPoolList[i].position = monsterRespawn.position;

                monster = pool.monsterPoolList[i].GetComponent<Monster>();
                monster.Init();
                monster.SetData();
                break;
            }
        }

        for (int i=0; i<heroObjList.Length; ++i)
        {
            if(heroObjList[i] != null)
            {
                heroObjList[i].transform.position = heroRespawn1[i].position;
                monster.currentTarget.Add(inGameHeroList[i]);              
            }
        }

        battleStart = true;        
    }

    public IEnumerator StartDungeon()
    {
        yield return new WaitForSeconds(3.0f);

        for(int i=0; i<heroSpawnEffect.Length; ++i)
        {
            heroSpawnEffect[i].SetActive(false);
        }

        BattleUIMng.Ins.bg.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        BattleUIMng.Ins.bg.SetActive(false);

        BattleStart();
    }

    public void HeroDie(Hero hero)
    {
        if(monster.currentHp > 0 && inGameHeroList.Count > 0)
        {
            monster.target = null;

            inGameHeroList.Remove(hero);
        }
        if (inGameHeroList.Count == 0)
        {
            Debug.Log("영웅 다 죽음");
            Time.timeScale = 0;
        }
    }

    public void MonsterDie()
    {
        //Debug.Log("Win");
        battleStart = false;

        BattleUIMng.Ins.bottomBattleUI.SetActive(false);
        BattleUIMng.Ins.topBattleUI.SetActive(false);
        
        for(int i=0; i<heroList.Count; ++i)
        {
            heroList[i].gameObject.SetActive(false);
        }

        monster.gameObject.SetActive(false);
        monster = null;

        //BattleUIMng.Ins.stageReward.StartCoroutine(BattleUIMng.Ins.stageReward.OpenReward());
        BattleUIMng.Ins.stageReward.SetVisible(true);

        HeroLevel();

        Player.Ins.playerData.nVigor += stageRewardData.nVigor;
        Player.Ins.playerData.nExp   += stageRewardData.nExp / 2;

        GameMng.Ins.PlayerExpUp();

        ++GameMng.Ins.StageNum;

        if (GameMng.Ins.StageNum >= GameMng.Ins.tile[GameMng.Ins.currentTileIdx].bStageClear.Count)
        {
            GameMng.Ins.tile[GameMng.Ins.currentTileIdx].bFinalClear = true;
            GameMng.Ins.StageClear();
        }

        if(GameMng.Ins.StageNum < GameMng.Ins.tile[GameMng.Ins.currentTileIdx].bStageClear.Count)
        {
            GameMng.Ins.tile[GameMng.Ins.currentTileIdx].bStageClear[GameMng.Ins.StageNum] = true;
        }
        
    }

    void HeroLevel()
    {
        var heroData = heroDatas.GetEnumerator();
        
        if(stageRewardData == null) { Debug.LogError("error"); return; }

        int idx = 0;

        while(heroData.MoveNext())
        {
            var data = heroData.Current;

            int nHeroMaxExp = ExtensionMethod.GetHeroLevelData(data.nLevel);            
            
            int stageRewardExp = stageRewardData.nExp;            ;
            
            repeatCount = 0;
            data.nExp += stageRewardExp;

            //현재 경험치에 스테이지 보상 경험치를 더한 값이 현재 레벨에 맥스 경험치보다 같거나 크면
            if (data.nExp >= nHeroMaxExp)
            {
                ++data.nLevel;
                ++repeatCount;
                
                while(data.nExp >  nHeroMaxExp)
                {
                    data.nExp   -= nHeroMaxExp;
                    nHeroMaxExp  = ExtensionMethod.GetHeroLevelData(data.nLevel);

                    if(data.nExp >= nHeroMaxExp)
                    {
                        ++data.nLevel;
                        ++repeatCount;
                    }          
                }
            }

            var expUI = BattleUIMng.Ins.stageReward.heroExpBar;
            
            expUI[idx].ExpUp(repeatCount, nHeroMaxExp, data.nExp, stageRewardExp);

            ++idx;
        }
    }

    public void AddHero(int idx)
    {
        //Debug.Log(idx);

        var list = Player.Ins.heroDataList.GetEnumerator();

        while(list.MoveNext())
        {
            var data = list.Current;

            if(idx == 0)
            {
                heroDatas.AddLast(data);
                break;
            }
            --idx;
        }
    }

    public void OutBattle()
    {
        for(int i=0; i<heroList.Count; ++i)
        {
            heroStatCalc.RemoveHeroStat(heroList[i]);
        }
    }
}
