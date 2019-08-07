using System.Collections;
using System.Collections.Generic;
using G_Define;

public class Tile 
{
    public eTile eTile = eTile.None;
    public StageData stageData = null;
    //타일 활성화인지 체크
    public bool bOpen = false;
    //최종 스테이지 클리어 체크 true면 인접한 타일 활성화되게끔
    public bool bFinalClear = false;
    //각 스테이지의 층 체크 
    public List<bool> bStageClear = new List<bool>();
}
