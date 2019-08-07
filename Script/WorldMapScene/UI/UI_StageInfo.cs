using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;


public class UI_StageInfo : MonoBehaviour
{
    const int slotSize = 4;

    public Text             stageTitle;
    public Image[]          monsterImg = new Image[slotSize];
    public Text[]           vigorText  = new Text[slotSize];
    public Button[]         btnList    = new Button[slotSize];
    public List<GameObject> content;

    public string[] stageData  = new string[slotSize];

    public void SetVisible(bool active)
    {
        if (active == true)
        {
            bool flag = SceneMng.Ins.PopupSetActive(this.gameObject);

            if (!flag)
            {
                return;
            }
        }

        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }

        if(gameObject.activeSelf == true)
        {
            Init();
        }
    }

    void Init()
    {
        StageData data = WorldMapMng.Ins.refTile.stageData;
        var tile = WorldMapMng.Ins.refTile;

        stageData[0] = data.monster1;
        stageData[1] = data.monster2;
        stageData[2] = data.monster3;
        stageData[3] = data.monster4;

        for(int i=0; i<monsterImg.Length; ++i)
        { 
            string img = string.Format(Path.IMG, stageData[i]);

            monsterImg[i].sprite = Resources.Load<Sprite>(img) as Sprite;

            vigorText[i].text = string.Format("X {0}", data.nVigor);
        }

        stageTitle.text = string.Format("{0}", data.strStage);
        GameMng.Ins.strStageName = stageTitle.text;

        for(int i=0; i<tile.bStageClear.Count; ++i)
        {
            if(tile.bStageClear[i] == false)
            {
                ChangeImg(btnList[i],"Lock",Color.black);

                btnList[i].enabled = false;
                content[i].SetActive(false);               
            }
            else
            {
                ChangeImg(btnList[i],"None",Color.white);

                btnList[i].enabled = true;
                content[i].SetActive(true);
            }
        }
    }

    void ChangeImg(Button btn, string imgName,Color color)
    {
        string imgPath = string.Format(Path.IMG, imgName);
        var img        = btn.GetComponent<Image>();

        img.sprite = Resources.Load<Sprite>(imgPath) as Sprite;
        img.color  = color;
    }

    
}
