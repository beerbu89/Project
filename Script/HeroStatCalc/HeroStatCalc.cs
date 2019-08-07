using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;
using System;

public class HeroStatCalc 
{
    //영웅이 착용한 장비 이름
    List<string>    li_HeroEquipString = new List<string>();
    //장비 스텟(각 영웅마다 스텟 적용)      
    List<int>       li_HeroItemStat    = new List<int>();
    //전투에 참여한 모든 영웅의 장비 스텟 저장
    List<List<int>> li_BattleHeroStat  = new List<List<int>>();

    //전투시 영웅 기본 스텟 + 착용아이템 스텟
    public void AddHeroStat(int heroIdx,Hero hero)
    {   
        //해당 영웅이 장비를 착용했는지 확인
        if (!Player.Ins.heroEquipList.ContainsKey(heroIdx)) { Debug.Log("해당 영웅은 장비를 착용하지 않음"); return; }

        li_HeroEquipString.Clear();
        li_HeroItemStat.Clear();

        //착용했다면.        
        var equipString = Player.Ins.heroEquipList[heroIdx];
        var temp        = BattleUIMng.Ins.inven.sBasicEquipImg;

        for(int i=0; i< equipString.Count; ++i)
        {
            var path = string.Format(Path.IMG, equipString[i]);
            var img = Resources.Load<Sprite>(path) as Sprite;

            var path1 = string.Format(Path.IMG, temp[i]);
            var img1 = Resources.Load<Sprite>(path1) as Sprite;

            if (img != img1)
            {
                li_HeroEquipString.Add(equipString[i]);
            }
        }

        li_HeroItemStat = ItemMng.Ins.GetItemStat(li_HeroEquipString);

        hero.PlusStat(li_HeroItemStat);

        li_BattleHeroStat.Add(li_HeroItemStat);
    }

    //전투 종료 후 영웅 스텟 - 착용아이템 스텟
    public void RemoveHeroStat(Hero hero)
    {
        if(hero.statFlag == false) { Debug.Log("해당 영웅은 장비를 착용하지 않음"); return; }

        if(li_BattleHeroStat.Count == 0) { return; }

        var temp = li_BattleHeroStat[0];

        hero.MinusStat(temp);

        li_BattleHeroStat.RemoveAt(0);
    }
}
