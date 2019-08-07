using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//소모품(행동력, 돈, 젬)관련
public class UI_Expendables : MonoBehaviour
{
    public Text vigorText;
    public Text moneyText;
    public Text gemText;

    public void ChangeText(LevelData levelData)
    {
        if(Player.Ins.playerData.nMoney < 0)
        {
            Player.Ins.playerData.nMoney = 0;
        }

        if (Player.Ins.playerData.nGem < 0)
        {
            Player.Ins.playerData.nGem = 0;
        }

        if(Player.Ins.playerData.nVigor < 0)
        {
            Player.Ins.playerData.nVigor = 0;
        }

        vigorText.text = string.Format("{0:##,##0}/{1}", Player.Ins.playerData.nVigor, levelData.nMaxVigor);
        moneyText.text = string.Format("{0:##,##0}", Player.Ins.playerData.nMoney);
        gemText.text   = string.Format("{0:##,##0}", Player.Ins.playerData.nGem);
        //Console.WriteLine(String.Format("{0:##,##0}", price));
    }

}
