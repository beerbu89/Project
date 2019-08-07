using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Vigor : MonoBehaviour
{
    public Text vigorText;

    public void VigorText(LevelData levelData)
    {
        if(Player.Ins.playerData.nVigor < 0)
        {
            Player.Ins.playerData.nVigor = 0;
        }

        vigorText.text = string.Format("{0:##,##0}/{1}", Player.Ins.playerData.nVigor, levelData.nMaxVigor);
    }
}
