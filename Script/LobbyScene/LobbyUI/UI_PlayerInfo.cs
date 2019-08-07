using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerInfo : MonoBehaviour
{
    public Text  levelText;
    public Text  playerName;
    public Image expBar;

    public void PlayerChangeInfo(PlayerData playerData,LevelData levelData)
    {
        levelText.text    = string.Format("Lv.{0}", playerData.nLevel);
        playerName.text   = string.Format("{0}", playerData.strName);
        expBar.fillAmount = (float)playerData.nExp / levelData.nMaxExp /1;        
    }
}
