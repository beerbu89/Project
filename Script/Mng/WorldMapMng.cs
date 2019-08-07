using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using G_Define;

public class WorldMapMng : MonoBehaviour
{
    #region SINGLETON
    static WorldMapMng _instance = null;

    public static WorldMapMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(WorldMapMng)) as WorldMapMng;
                if (_instance == null)
                {
                    _instance = new GameObject("WorldMapMng", typeof(WorldMapMng)).GetComponent<WorldMapMng>();
                }
            }

            return _instance;
        }
    }

    #endregion

    public Map       map        = new Map();
    public Board     board;
    public Tile      refTile    = null;
    
    public void Clear()
    {
        map       = null;
        board     = null;
        refTile   = null;    
    }   
    
    private void Start()
    {    
        if(GameMng.Ins.currentTileIdx == -1)
        {
            map.Init();
            board.SetTile();
        }
        else
        {
            map.Init();
            board.ChangeTile();
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Tile"))
                    {
                        int  idx  = 0;
                        bool flag = board.tileObjList.TryGetValue(hit.transform, out idx);

                        if(flag)
                        {
                            refTile = board.boardTileList[idx];
                            GameMng.Ins.currentTileIdx = idx;
                        }
                        else { Debug.Log("flag error"); return; }
                        
                        if (refTile == null) { Debug.LogError("refTile Error"); return; }
                        
                        if(refTile.bOpen == true)
                        {
                            WorldUIMng.Ins.StageInfo.SetVisible(true);
                        }
                    }
                }
            }            
        }
    }

    public void BattleClick(int idx)
    {
        if(WorldUIMng.Ins.message.gameObject.activeSelf == true) { return; }
        string sMonster = WorldUIMng.Ins.StageInfo.stageData[idx];

        SceneMng.Ins.sMonsterName = sMonster;
        SceneMng.Ins.nVigor       = refTile.stageData.nVigor;
        GameMng.Ins.StageNum      = idx;

        SceneManager.LoadScene((int)eScene.BattleScene);
    }
}
