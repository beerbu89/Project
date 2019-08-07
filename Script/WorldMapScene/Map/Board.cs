using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

public class Board : MonoBehaviour
{
    public List<Tile>                 boardTileList   = new List<Tile>();
    public Dictionary< Transform,int> tileObjList     = new Dictionary<Transform,int>();

    Tile tile  = null;
    Map refMap = null;

    int stageNum    = 4;
    int defalutTile = 0;
    int dliIdx      = 0;
    //44

    GameObject tileRoot = null;

    public void SetTile()
    {
        tileRoot = new GameObject();

        refMap = WorldMapMng.Ins.map;

        int mapX   = refMap.nX;
        int mapY   = refMap.nY;
        int height = 3;

        for (int i = 0; i < refMap.tileList.Count; ++i)
        {
            eTile eTile = refMap.tileList[i];            

            int nX = i % mapX;
            int nY = i / mapX;

            Instantiate(eTile, nX, height, nY);
            boardTileList.Add(tile = new Tile());

            boardTileList[i].eTile = eTile;

            for (int j=0; j<stageNum; ++j)
            {   
                boardTileList[i].bStageClear.Add(false);                
            }
            
            boardTileList[i].stageData = ExtensionMethod.GetStageTb(eTile);
            //Debug.LogError(boardTileList[i].stageData.strStage);

            GameMng.Ins.stageData.Add(boardTileList[i].stageData);
            GameMng.Ins.tile.Add(boardTileList[i]);
        }

        //초기 타일 
        boardTileList[defalutTile].bOpen          = true;
        boardTileList[defalutTile].bStageClear[0] = true;

        GameMng.Ins.currentTileIdx       = defalutTile;
    }

    void Instantiate(eTile eTile, int nX, int heigth, int nY)
    {
        string sTileName = string.Format(Path.TILE, eTile.ToString());
        GameObject tile = (GameObject)Instantiate(Resources.Load(sTileName));

        tile.transform.parent        = tileRoot.transform;
        tile.transform.localPosition = new Vector3(nX, heigth, nY);

        tileObjList.Add(tile.transform, dliIdx);
        ++dliIdx;
    }
    public void ChangeTile()
    {
        tileRoot = new GameObject();

        refMap = WorldMapMng.Ins.map;

        int mapX = refMap.nX;
        int mapY = refMap.nY;
        int height = 3;

        for (int i = 0; i < refMap.tileList.Count; ++i)
        {
            eTile eTile = refMap.tileList[i];

            int nX = i % mapX;
            int nY = i / mapX;

            Instantiate(eTile, nX, height, nY);
            boardTileList = GameMng.Ins.tile;

            boardTileList[i].stageData   = GameMng.Ins.stageData[i];
            boardTileList[i].bOpen       = GameMng.Ins.tile[i].bOpen;
            boardTileList[i].eTile       = eTile;
            boardTileList[i].bStageClear = GameMng.Ins.tile[i].bStageClear;
        }
    }
}
