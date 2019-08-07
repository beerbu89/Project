using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;
using System;

public  class Characters : MonoBehaviour
{
    //public HeroData heroData = null;

    public int   nAtk;
    public int   nHp;
    public int   nDef;
    public int   nAtkRange;
    public int   nLevel;
    public int   nExp;
    public float fSpeed;

    int idx = 0;

    public void SetData(eHeroProperty _eHeroProperty, int _nID)
    {
        //HeroData heroData = null;      
   
        var tb    = TableMng.Ins.heroTb.GetTable();
        Data data = new Data();

        if(tb != null)
        {
            foreach (var val in tb)
            {
                if (val.Value.nID == _nID)
                {
                    data.SetData(val.Value.nID, val.Value.eHeroProperty, val.Value.strName, val.Value.strPrefab, val.Value.nAtk, val.Value.nHp, val.Value.nDef,
                                val.Value.nAtkRange, val.Value.nLevel, val.Value.nExp, val.Value.fSpeed);
                    break;
                }
            }
        }
 
        if (data != null)
        {            
            data.eHeroProperty = (int)_eHeroProperty;
            //Debug.LogError(heroData.eHeroProperty);

            Player.Ins.heroDataList.AddLast(data);

        }
        else
        {
            Debug.LogError("heroDataTb null");
        }

    }
}
