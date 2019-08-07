using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using System;

//전투시 영웅UI(hpbar skill)
public class UI_Info : MonoBehaviour
{
    public List<Image>       li_HeroImg;
    public List<UI_HPBar>    li_HeroHpBar; 
    public List<Image>       li_Skill;
    public List<Image>       li_ActiveSkillCool;

    public int currentIdx = 0;
 
    const int nSKILL_COUNT = 3;    

    private void Start()
    {
        SetInfo();
    }   

    public void SetInfo()
    {
        var list = BattleMng.Ins.heroDatas.GetEnumerator();        

        int idx = 0;
        int size = BattleMng.Ins.heroDatas.Count;
     
        int currentSkillIdx = 0;

        while (list.MoveNext())
        {   
            var data = list.Current;

            string imgPath = string.Format(Path.IMG, data.strName);

            li_HeroImg[idx].sprite = Resources.Load<Sprite>(imgPath) as Sprite;

            li_HeroHpBar[idx].SetHpBar(data);

            BattleMng.Ins.heroList[idx].hpBar = li_HeroHpBar[idx];

            eHero eHero = (eHero)data.nID;
            var temp = eHero.GetHeroSkillData();

            var s = temp.strSkill.Split(',');

            int sIdx = 0;

            if(currentSkillIdx >= li_Skill.Count) { return; }
         
            for(int i=0; i <  nSKILL_COUNT; ++i)
            {
                string path = string.Format(Path.IMG, s[sIdx]);

                li_Skill[currentSkillIdx].sprite = Resources.Load<Sprite>(path) as Sprite;

                SkillMng.Ins.li_DeActiveSkillCoolTime.Add(li_Skill[currentSkillIdx]);

                SkillMng.Ins.li_ActiveSkill.Add(li_Skill[currentSkillIdx]);

                ++sIdx;
                ++currentSkillIdx;
            }

            currentIdx = currentSkillIdx;

            ++idx;
        }

        SkillMng.Ins.Init();

        for (int i= currentSkillIdx; i<li_Skill.Count; ++i)
        {
            li_Skill[i].gameObject.SetActive(false);
        }

        for(int i=size; i< li_HeroImg.Count; ++i)
        {
            li_HeroImg[i].gameObject.SetActive(false);
            li_HeroHpBar[i].gameObject.SetActive(false);
        }
    }

}
