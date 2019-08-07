using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using G_Define;

public class BattleUIMng : MonoBehaviour
{
    #region SINGLETON
    static BattleUIMng _instance = null;

    public static BattleUIMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(BattleUIMng)) as BattleUIMng;
                if (_instance == null)
                {
                    _instance = new GameObject("BattleUIMng", typeof(BattleUIMng)).GetComponent<BattleUIMng>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public UI_Inventory   inven;    
    public UI_Vigor       vigor;
    public BattleWait     battleWait;
    public GameObject     topUI;
    public GameObject     topBattleUI;
    public GameObject     bottomBattleUI;
    public GameObject     battleMap;
    public GameObject     bg;
    public UI_Info        heroinfo;
    public UI_HPBar       monsterHPBar;
    public UI_StageReward stageReward;
    public UI_Message     message;
    public HitText        hitText;
    public UI_HitText     hitTextUI;

    private void Awake()
    {
        inven.Init();        
    }
}
