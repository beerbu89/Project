using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;

//소환
public class UI_Summons : MonoBehaviour
{
    public Text       heroCountText;
    public Image      oneHeroBGColor;
    public Image      oneHero;
    public Image      tenHeroBG;
    public Image[]    tenHeroSlot = new Image[Define.nSUMMONSSLOT_SIZE];
    public Image[]    tenHero     = new Image[Define.nSUMMONSSLOT_SIZE];
    public GameObject popup;
    public GameObject popupSlotError;
    public Button     summonsBtn;

    Color         color;
    eSummonsUnit  eUnit     = eSummonsUnit.None;
    //eHeroProperty eHeroType = eHeroProperty.None;

    int idx = 0;
 
    void Init()
    {
        oneHeroBGColor.color = Color.white;
        oneHero.sprite       = null;

        for(int i=0; i<tenHeroSlot.Length; ++i)
        {
            tenHeroSlot[i].color = Color.white;
            tenHero[i].sprite    = null;
        }

        HeroCountText();
    }
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
            oneHeroBGColor.gameObject.SetActive(true);
            tenHeroBG.gameObject.SetActive(false);
        }
        //else { Init(); }
        
    }

    //1회소환인건지 10회 소환인건지
    public void SetVisible(GameObject obj, bool active)
    {
        if (obj.gameObject.activeSelf != active)
        {
            Init();
            obj.gameObject.SetActive(active);
        }
    }

    public void OneClick()
    {
        //영웅수
        int count    = LobbyUIMng.Ins.inven.heroCount;
        int seeCount = LobbyUIMng.Ins.inven.seeSlotSize;        

        SetVisible(tenHeroBG.gameObject, false);
        SetVisible(oneHeroBGColor.gameObject, true);
        
        int price = 10000;
        if (Player.Ins.playerData.nMoney < price) 
        {
            StartCoroutine(Popup(popup));
            Debug.LogError("소환불가");
            return;
        }        

        if(count >= seeCount)
        {
            StartCoroutine(Popup(popupSlotError));
            Debug.LogError("소환불가");
            return;
        }
   
        bool flag = BuyHero();

        if (flag)
        {
            bool check = LobbyUIMng.Ins.inven.Check();
            if(check)
            {
                LobbyUIMng.Ins.inven.SetHeroImg(oneHeroBGColor, oneHero);
                Player.Ins.MinusMoney(price);
                HeroCountText();
                ++idx;
            }
        }
        else
        {
            Init();
            Debug.LogError("소환에러");
            return;
        }
    }

    public void TenClick()
    {
        int count    = LobbyUIMng.Ins.inven.heroCount + tenHeroSlot.Length;
        int seeCount = LobbyUIMng.Ins.inven.seeSlotSize;

        SetVisible(tenHeroBG.gameObject, true);
        SetVisible(oneHeroBGColor.gameObject, false);

        int price = 100000;
        if (Player.Ins.playerData.nMoney < price) 
        {
            StartCoroutine(Popup(popup));
            Debug.LogError("소환불가");
            return;
        }

        if (count > seeCount)
        {
            StartCoroutine(Popup(popupSlotError));
            Debug.LogError("소환불가");
            return;
        }

        bool flag = BuyHero(tenHeroSlot);

        if (flag)
        {
            bool check = LobbyUIMng.Ins.inven.Check(tenHeroSlot);

            if(check)
            {
                LobbyUIMng.Ins.inven.SetHeroImg(tenHeroSlot, tenHero);
                Player.Ins.MinusMoney(price);
                HeroCountText();
            }
        }
        else
        {
            Debug.LogError("소환에러");
            return;
        }
    }


    //1회 소환
    bool BuyHero()
    {
        ColorType();
        oneHeroBGColor.color = color;
    
        eUnit    = ExtensionMethod.GetSummonsUnit(eUnit);
        var data = ExtensionMethod.GetSummonsUnitTb((int)eUnit);
    
        if (data != null)
        {
            string changeImg = string.Format(Path.IMG, data.strName);
            oneHero.sprite = Resources.Load<Sprite>(changeImg) as Sprite;

            return true;
        }
        else
        {
            Debug.LogError("소환 에러");
            return false;
        }
    }

    //10회 소환
    bool BuyHero(Image[] slot)
    {
        for(int i=0; i<slot.Length; ++i)
        {
            ColorType();
            tenHeroSlot[i].color = color;

            eUnit    = ExtensionMethod.GetSummonsUnit(eUnit);
            var data = ExtensionMethod.GetSummonsUnitTb((int)eUnit);

            if (data != null)
            {
                string changeImg  = string.Format(Path.IMG, data.strName);
                tenHero[i].sprite = Resources.Load<Sprite>(changeImg) as Sprite;
            }
            else
            {
                Debug.LogError("소환 에러");
                return false;
            }
            ++idx;
        }

        return true;
    }
    void ColorType()
    {
        eHeroPropertyColor eColorVal = (eHeroPropertyColor)Random.Range((int)eHeroPropertyColor.Blue, (int)eHeroPropertyColor.Max);

        switch (eColorVal)
        {
            case eHeroPropertyColor.Blue:
                color = Color.blue;
                //eHeroType = eHeroProperty.Ice;
                break;
            case eHeroPropertyColor.Red:
                color = Color.red;
                //eHeroType = eHeroProperty.Fire;
                break;
            case eHeroPropertyColor.Green:
                color = Color.green;
                //eHeroType = eHeroProperty.Forest;
                break;          
            default:
                Debug.LogError("None 컬러");
                break;
        }
    }

    void HeroCountText()
    {
        LobbyUIMng.Ins.inven.HeroCountText(heroCountText);
    }

    IEnumerator Popup(GameObject obj)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        obj.SetActive(false);
    }
}
