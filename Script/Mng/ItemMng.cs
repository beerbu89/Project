using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using System;

public class ItemMng : MonoBehaviour
{
    #region SINGLETON
    static ItemMng _instance = null;

    public static ItemMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(ItemMng)) as ItemMng;
                if (_instance == null)
                {
                    _instance = new GameObject("ItemMng", typeof(ItemMng)).GetComponent<ItemMng>();
                }
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    #endregion
    
    public List<string> li_Item     = new List<string>();
    public List<int>    li_ItemStat = new List<int>();

    const int nLIST_SIZE = 12;
    
    public void AddItem(Image[] img)
    {
        if(li_Item.Count >= nLIST_SIZE) { return; }

        for(int i=0; i<img.Length; ++i)
        {
            li_Item.Add(img[i].sprite.name);
        }
    }

    public void RemoveItem(int itemIdx)
    {
        li_Item.RemoveAt(itemIdx);        
    }

    public List<int> GetItemStat(List<string> li_Temp)
    {
        li_ItemStat.Clear();

        int nAtk = 0;
        int nDef = 0;
        int nHP  = 0;

        for(int i=0; i<li_Temp.Count; ++i)
        {
            eItem eItem = eItem.None;
            eItem = (eItem)Enum.Parse(typeof(eItem), li_Temp[i]);

            switch (eItem)
            {   
                case eItem.sword1:                    
                case eItem.axe1:                    
                case eItem.top1:                    
                case eItem.top2:                    
                case eItem.bottom1:                    
                case eItem.bottom2:                    
                case eItem.Arm1:                    
                case eItem.Arm2:                    
                case eItem.helmet1:                    
                case eItem.helmet2:                    
                case eItem.shoes1:                    
                case eItem.shoes2:
                    var data = eItem.GetItemType();
                    nAtk += data.nAtk;
                    nDef += data.nDef;
                    nHP  += data.nHP;
                    break;
                default:
                    Debug.Log("착용하지 않은 아이템");
                    break;
            }
        }

        //Debug.Log(nAtk);
        //Debug.Log(nDef);
        //Debug.Log(nHP);

        li_ItemStat.Add(nAtk);
        li_ItemStat.Add(nDef);
        li_ItemStat.Add(nHP);

        return li_ItemStat;
    }
}
