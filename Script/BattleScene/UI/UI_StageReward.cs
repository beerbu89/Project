using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using UnityEngine.SceneManagement;
using System;

public class UI_StageReward : MonoBehaviour
{
    public Text        stageText;
    //Hero이미지쪽
    public List<Image>     heroImg;
    public List<Text>      heroExpText;    
    public List<UI_ExpBar> heroExpBar;
    //보상
    public Text        rewardExpText;
    public Text        rewardMoneyText;
    public Text        rewardVigorText;

    const int nREWARDITEM_SIZE = 3;
    public Image[] rewardItem = new Image[nREWARDITEM_SIZE];

    public GameObject popup;

    int monsterNum = 1;

    public void SetVisible(bool action)
    {
        if(gameObject.activeSelf != action)
        {
            gameObject.SetActive(action);
        }

        if(gameObject.activeSelf == true)
        {
            SetData();
        }
    }

    public void SetData()
    {
        stageText.text = GameMng.Ins.strStageName;

        int size = BattleMng.Ins.heroDatas.Count;

        var stageReward = BattleMng.Ins.stageRewardData;
        
        if(stageReward == null) { Debug.LogError("data error"); return; }

        //Hero이미지쪽
        for (int i=0; i<heroImg.Count; ++i)
        {
            if(i >= size)
            {
                heroImg[i].gameObject.SetActive(false);
            }
            else
            {
                heroImg[i].gameObject.SetActive(true);

                string sPath        = string.Format(Path.IMG, BattleMng.Ins.heroList[i].eHero.ToString());
                heroImg[i].sprite   = Resources.Load<Sprite>(sPath) as Sprite;
                heroExpText[i].text = string.Format("EXP+{0:###0}", stageReward.nExp);
            }
        }

        //보상쪽
        rewardExpText.text = string.Format("EXP {0:###0}", stageReward.nExp);


        int ranMoney = UnityEngine.Random.Range(stageReward.nMinMoney, stageReward.nMaxMoney);

        rewardMoneyText.text = string.Format("{0}", ranMoney);
        rewardVigorText.text = string.Format("{0} 획득", stageReward.nVigor);

        Player.Ins.playerData.nMoney += ranMoney;

        //장비보상
        string sItemPath1 = string.Format(Path.IMG, stageReward.strItem1);
        string sItemPath2 = string.Format(Path.IMG, stageReward.strItem2);
        string sItemPath3 = string.Format(Path.IMG, stageReward.strItem3);

        rewardItem[0].sprite = Resources.Load<Sprite>(sItemPath1) as Sprite;
        rewardItem[1].sprite = Resources.Load<Sprite>(sItemPath2) as Sprite;
        rewardItem[2].sprite = Resources.Load<Sprite>(sItemPath3) as Sprite;

        ItemMng.Ins.AddItem(rewardItem);        

        //heroExp초기세팅
        var heroData = BattleMng.Ins.heroDatas.GetEnumerator();       

        int idx = 0;
        while(heroData.MoveNext())
        {
            var data = heroData.Current;

            int nMaxpExp = ExtensionMethod.GetHeroLevelData(data.nLevel);

            heroExpBar[idx].SetExp(nMaxpExp, data.nExp);

            ++idx;
        }
    }

    public void ClickLobby()
    {
        BattleMng.Ins.OutBattle();
        BattleMng.Ins.Clear();
        SceneManager.LoadScene((int)eScene.LobbyScene);
    }

    public void ClickWorldMap()
    {
        BattleMng.Ins.OutBattle();
        BattleMng.Ins.Clear();
        SceneManager.LoadScene((int)eScene.WorldMapScene);
    }

    public void ClickReStart()
    {
        BattleMng.Ins.sMonsterName = SceneMng.Ins.sMonsterName;

        BattleUIMng.Ins.topBattleUI.SetActive(false);
        BattleUIMng.Ins.bottomBattleUI.SetActive(false);
        SetVisible(false);

        Camera.main.transform.eulerAngles = BattleMng.Ins.waitCameraRotX;
        Camera.main.transform.position = BattleMng.Ins.waitCameraPos;

        for (int i = 0; i < BattleMng.Ins.heroDatas.Count; ++i)
        {
            BattleMng.Ins.heroList[i].gameObject.SetActive(true);
            BattleMng.Ins.heroList[i].transform.position = BattleMng.Ins.heroRespawn[i].position;
            BattleMng.Ins.heroList[i].Init();
            BattleMng.Ins.heroList[i].SetData();

            BattleMng.Ins.heroSpawnEffect[i].gameObject.SetActive(true);
        }

        BattleUIMng.Ins.hitText.Init();

        SkillMng.Ins.Clear();
        //hero hp
        BattleUIMng.Ins.heroinfo.SetInfo();

        BattleMng.Ins.StartCoroutine(BattleMng.Ins.StartDungeon());
    }

    public void NextFloor()
    {        
        int idx      = GameMng.Ins.currentTileIdx;
        int stageIdx = GameMng.Ins.StageNum;

        //GameMng.Ins.tile[idx].bStageClear[stageIdx] == false ||
        if (stageIdx >= GameMng.Ins.tile[GameMng.Ins.currentTileIdx].bStageClear.Count)
        {
            StartCoroutine(PopupText());
            //Debug.LogError("마지막 층");
            return;
        }

        //BattleUIMng.Ins.stageReward.SetVisible(false);
        SetVisible(false);

        //던전 재입장 카메라 위치
        Camera.main.transform.eulerAngles = BattleMng.Ins.waitCameraRotX;
        Camera.main.transform.position    = BattleMng.Ins.waitCameraPos;

        for (int i = 0; i < BattleMng.Ins.heroDatas.Count; ++i)
        {             
            BattleMng.Ins.heroList[i].gameObject.SetActive(true);
            BattleMng.Ins.heroList[i].transform.position = BattleMng.Ins.heroRespawn[i].position;
            BattleMng.Ins.heroList[i].Init();
            BattleMng.Ins.heroList[i].SetData();

            BattleMng.Ins.heroSpawnEffect[i].gameObject.SetActive(true);
        }

        BattleUIMng.Ins.hitText.Init();

        SkillMng.Ins.Clear();
        //hero hp
        BattleUIMng.Ins.heroinfo.SetInfo();
    
        //다음 스테이지 몬스터로 바꿔주고
        eMonster eMonster = (eMonster)Enum.Parse(typeof(eMonster), BattleMng.Ins.sMonsterName);

        int temp = (int)eMonster + monsterNum;
        eMonster = (eMonster)temp;

        BattleMng.Ins.sMonsterName = eMonster.ToString();
        
        //스테이지 보상 다시 셋팅
        BattleMng.Ins.stageRewardData = ExtensionMethod.GetStageRewardData(GameMng.Ins.StageNum);

        BattleMng.Ins.StartCoroutine(BattleMng.Ins.StartDungeon());
    }

    IEnumerator PopupText()
    {
        popup.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        popup.SetActive(false);
    }


    public IEnumerator OpenReward()
    {
        yield return new WaitForSeconds(1.5f);

        SetVisible(true);
    }
}
