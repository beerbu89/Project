using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;

public class UI_HPBar : MonoBehaviour
{
    public Image         mask;
    public RectTransform maskRect;

    float maxHp         = 0;
    float currentHp     = 0;
    float maxHpBarWidth = 0;
    //초기값
    float hpBarWidth    = 0;

    private void Awake()
    {
        maxHpBarWidth = maskRect.sizeDelta.x;
        hpBarWidth    = maxHpBarWidth;
    }

    public void SetHpBar(MonsterData monsterData)
    {        
        maxHp      = monsterData.nHp;
        currentHp  = maxHp;

        Vector2 xSize      = maskRect.sizeDelta;
        xSize.x            = hpBarWidth;
        maskRect.sizeDelta = xSize;
        maxHpBarWidth      = maskRect.sizeDelta.x;
    }

    public void SetHpBar(Data heroData)
    {
        maxHp      = heroData.nHp;
        currentHp  = maxHp;

        Vector2 xSize      = maskRect.sizeDelta;
        xSize.x            = hpBarWidth;
        maskRect.sizeDelta = xSize;
        maxHpBarWidth      = maskRect.sizeDelta.x;
    }
    
    public void HpDown(int atk = 100)
    {
        currentHp -= atk;

        if(currentHp <= 0) { currentHp = 0; }

        float deltaSize = currentHp / maxHp;

        maskRect.sizeDelta = new Vector2(maxHpBarWidth * deltaSize, maskRect.sizeDelta.y);
    }
}
