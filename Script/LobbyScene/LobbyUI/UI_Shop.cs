using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using System;

public class UI_Shop : MonoBehaviour
{
    public Image      gemImg;
    public Image      goldImg;
    public GameObject gemItemBtn;
    public GameObject goldItemBtn;
    public List<Text> gemTextList;
    public List<Text> goldTextList;
    public GameObject popupGem;
    public GameObject popupMoney;
    public Button     shopBtn;
    
    eShopCategory eShopCategory = eShopCategory.Gem;

    int minus = 0;
    int plus  = 0;

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
            gemItemBtn.SetActive(true);
            goldItemBtn.SetActive(false);

            gemImg.color  = Color.red;
            goldImg.color = Color.white;

        }
    }

    public void SetVisible(GameObject obj, bool active)
    {
        if(obj.gameObject.activeSelf != active)
        {
            obj.gameObject.SetActive(active);
        }
    }

    public void CategoryClick(string s)
    {
        eShopCategory eShop = (eShopCategory)Enum.Parse(typeof(eShopCategory), s);

        switch (eShop)
        {
            case eShopCategory.Gem:
                SetVisible(gemItemBtn, true);
                SetVisible(goldItemBtn, false);

                gemImg.color  = Color.red;
                goldImg.color = Color.white;

                eShopCategory = eShopCategory.Gem;
                break;
            case eShopCategory.Gold:
                SetVisible(gemItemBtn, false);
                SetVisible(goldItemBtn, true);

                gemImg.color  = Color.white;
                goldImg.color = Color.red;

                eShopCategory = eShopCategory.Gold;
                break;
            default:
                break;
        }
    }

    public void BuyItem(Text t)
    {
         switch (eShopCategory)
         {
             case eShopCategory.Gem:
                 ShopCategoryBuy(gemTextList, t);
                 
                 if (minus > Player.Ins.playerData.nGem)
                 {
                    StartCoroutine(Popup(popupGem));
                    return;
                 }
                 
                 Player.Ins.AddMoney(minus,plus);
    
                 break;
             case eShopCategory.Gold:                
                 ShopCategoryBuy(goldTextList, t);
                 
                 if (minus > Player.Ins.playerData.nMoney)
                 {
                    StartCoroutine(Popup(popupMoney));
                    return;
                 }
                 
                 Player.Ins.AddGem(minus, plus);
                 break;
             default:
                 break;
         }
    }

    void ShopCategoryBuy(List<Text> li,Text t)
    {
        int idx      = 0;
        int listIdx  = 0;
        string s     = null;
        string s1    = null;

        for (int i=0; i<li.Count; ++i)
        {
            if(t == li[i])
            {
                idx = i;
                break;
            }
        }

        //리스트 순서가 Add가 0~3인덱스 소모되는 비용 인덱스 4~7
        listIdx = idx - 4;

        s  = t.text;
        s1 = li[listIdx].text;
        s  = s.Replace(",", "");
        s1 = s1.Replace(",", "");

        minus = int.Parse(s.Trim());
        plus  = int.Parse(s1.Trim());
    }

    IEnumerator Popup(GameObject obj)
    {
        obj.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        obj.SetActive(false);
    }
}
