using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using G_Define;


public class WorldUIMng : MonoBehaviour
{
    #region SINGLETON
    static WorldUIMng _instance = null;

    public static WorldUIMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(WorldUIMng)) as WorldUIMng;
                if (_instance == null)
                {
                    _instance = new GameObject("WorldUIMng", typeof(WorldUIMng)).GetComponent<WorldUIMng>();
                }
            }
            return _instance;
        }
    }

    #endregion

    public UI_Inventory inven;
    public UI_StageInfo StageInfo;
    public UI_Vigor     vigor;
    public LevelData    levelData;
    public UI_Message   message;

    public void Clear()
    {
        levelData = null;
    }

    private void Start()
    {
        levelData = ExtensionMethod.GetLevelTb(Player.Ins.playerData.nLevel);
        vigor.VigorText(levelData);
        inven.Init();
    }

    public void BackScene()
    {
        Clear();
        WorldMapMng.Ins.Clear();
        SceneManager.LoadScene((int)eScene.LobbyScene);
    }

    
}
