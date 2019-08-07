using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using System;

public class SkillMng : MonoBehaviour
{
    #region SINGLETON
    static SkillMng _instance = null;

    public static SkillMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SkillMng)) as SkillMng;
                if (_instance == null)
                {
                    _instance = new GameObject("SkillMng", typeof(SkillMng)).GetComponent<SkillMng>();
                }
            }
            return _instance;
        }
    }


    #endregion

    //스킬을 쓸 수 있을 때 쿨타임 돌리는 이미지
    public List<Image>     li_ActiveSkillCoolTime   = new List<Image>();      
    //스킬을 쏜 후 쿨타임 돌리는 이미지
    public List<Image>     li_DeActiveSkillCoolTime = new List<Image>();    
    public List<Transform> li_Pool                  = new List<Transform>();
    //스킬등록    
    public List<Image>     li_ActiveSkill           = new List<Image>();

    Dictionary<int, eHeroSkill> dli_Pool = new Dictionary<int, eHeroSkill>();

    //List<bool> li_ActiveFlag = new List<bool>();

    public bool bActiveAction = true;

    const int nSKILL_COUNT     = 13;
    const int nSKILLPOOL_COUNT = 5;

    float activeCoolTime   = 4.0f;
    float deActiveCoolTime = 8.0f;

    string sTag = "Skill";

    public void Init()
    {
        if(li_ActiveSkillCoolTime.Count == 0)
        {
            li_ActiveSkillCoolTime = BattleUIMng.Ins.heroinfo.li_ActiveSkillCool;
        }

        int size = BattleUIMng.Ins.heroinfo.currentIdx;

        for (int i = size; i < li_ActiveSkillCoolTime.Count; ++i)
        {
            li_ActiveSkillCoolTime[i].gameObject.SetActive(false);
        }
        
        for (int i = 0; i < size; ++i)
        {
            li_ActiveSkillCoolTime[i].fillAmount = 0.0f;
            li_ActiveSkillCoolTime[i].gameObject.SetActive(true);
            li_ActiveSkillCoolTime[i].transform.position = BattleUIMng.Ins.heroinfo.li_Skill[i].transform.position;
            li_DeActiveSkillCoolTime[i].fillAmount = 1.0f;
            //li_ActiveFlag.Add(true);
        }

        if(li_Pool.Count == 0)
        {
            SkillPool();
        }

        bActiveAction = true;
    }
    
    public void Clear()
    {   
        li_DeActiveSkillCoolTime.Clear();
        li_ActiveSkill.Clear();
    }

    private void Update()
    {
        if(bActiveAction && BattleMng.Ins.monster != null)
        {
            bActiveAction = !bActiveAction;
            StartCoroutine(ActiveSkillCoolTime());
        }
    }

    void SkillPool()
    {
        string s = "";
        var parent = BattleMng.Ins.skillRoot;

        int key = 0;

        for (int i = 1; i < nSKILL_COUNT; ++i)
        {
            s = string.Format(Path.SKILL, i);

            eHeroSkill eSkill = (eHeroSkill) (i-1);

            for (int j = 0; j < nSKILLPOOL_COUNT; ++j)
            {
                Transform tr = Instantiate(Resources.Load<Transform>(s)) as Transform;
                tr.parent = parent;
                tr.position = Vector3.zero;
                tr.gameObject.SetActive(false);

                li_Pool.Add(tr);
                dli_Pool.Add(key, eSkill);
                ++key;
            }
            ++key;
        }
    }
    
    public void SkillCall()
    {
        if(BattleMng.Ins.monster == null) { return; }

        while (true)
        {   
            //쓸 스킬 슬롯 
            int nRanVal = UnityEngine.Random.Range(0, li_ActiveSkill.Count);

            //활성화된 스킬 중 전체 스킬 쿨타임
            //li_ActiveSkillCoolTime[nRanVal].gameObject.activeSelf == false는 이미 쓴 스킬.
            //li_ActiveSkillCoolTime[nRanVal].fillAmount <= 0.95f는 현재 스킬은 사용할 수 있으나 아직 쿨타임이 돌고있다.
            if (li_ActiveSkillCoolTime[nRanVal].gameObject.activeSelf == false || li_ActiveSkillCoolTime[nRanVal].fillAmount <= 0.95f)
            {
                continue;
            }

            //스킬을 쓸 수 없다.
            //li_DeActiveSkillCoolTime[nRanVal].fillAmount <=0.95f는 
            if (li_DeActiveSkillCoolTime[nRanVal].fillAmount <= 0.95f)
            {
                continue;
            }

            //li_ActiveSkillCoolTime[nRanVal]이 게임오브젝트는 꺼서 비활성화 시켜주고 = 스킬을 썻다.
            li_ActiveSkillCoolTime[nRanVal].gameObject.SetActive(false);

            string     strSkill = li_DeActiveSkillCoolTime[nRanVal].sprite.name;
            eHeroSkill eSkill   = (eHeroSkill)System.Enum.Parse(typeof(eHeroSkill), strSkill);

            int key = 0;

            //스킬 발동
            foreach(var val in dli_Pool)
            {
                if(val.Value == eSkill)
                {   
                    if (li_Pool[key].gameObject.activeSelf == false)
                    {
                        Debug.Log(eSkill);
                        Debug.Log(val.Value);
                        Debug.Log(li_Pool[key].gameObject.name);
                        Debug.Log(key);
                        li_Pool[key].gameObject.SetActive(true);
                       
                        Vector3 skillPos = BattleMng.Ins.monster.transform.position;
                        float y = 0.5f;
                        skillPos.y += y;
                        li_Pool[key].position = skillPos;
                        break;
                    }
                }
                ++key;
            }

            var data = eSkill.GetHeroSkillDamageData();

            if(data != null)
            {
                Define.Attack_ToMonster(BattleMng.Ins.monster, data);
            }

            var skillData = eSkill.GetHeroSkillDamageData();

            int damage = skillData.nDamage;

            var temp = BattleUIMng.Ins.hitText.GetHitText();

            BattleUIMng.Ins.hitTextUI.ActionText(BattleMng.Ins.monster, temp, damage, sTag);

            BattleUIMng.Ins.monsterHPBar.HpDown(damage);

            //스킬 obj끄기
            StartCoroutine(SkillSetActive(key));

            //쓴 스킬 다시 쿨타임 돌리기
            StartCoroutine(DeActiveSkillCoolTime(nRanVal));

            break;
        }
    }


    //쓸 수 있는 모든 스킬의 쿨타임은 전체가 같이돔
    IEnumerator ActiveSkillCoolTime()
    {
        if(BattleMng.Ins.monster == null) { yield break; }

        float time = 0.0f;

        while(time <= activeCoolTime)
        {
            time += Time.deltaTime;

            for(int i=0; i<li_ActiveSkill.Count; ++i)
            {
                li_ActiveSkillCoolTime[i].fillAmount = time / activeCoolTime;
            }

            yield return new WaitForFixedUpdate();
        }

        SkillCall();
    }  

    //쓴 스킬 쿨타임 돌리기
    IEnumerator DeActiveSkillCoolTime(int idx)
    {
        if (BattleMng.Ins.monster == null) { yield break; }

        float time = 0.0f;

        bActiveAction = !bActiveAction;

        li_DeActiveSkillCoolTime[idx].fillAmount = 0.0f;

        while (time <= deActiveCoolTime)
        {
            time += Time.deltaTime;

            li_DeActiveSkillCoolTime[idx].fillAmount = time / deActiveCoolTime;

            if (li_DeActiveSkillCoolTime[idx].fillAmount >= 1.0f)
            {
                li_ActiveSkillCoolTime[idx].fillAmount = 0.0f;
                li_ActiveSkillCoolTime[idx].gameObject.SetActive(true);
            }

            yield return new WaitForFixedUpdate();
        }
    }

    //스킬 사용 obj끄기
    IEnumerator SkillSetActive(int key)
    {
        float waitTime = 2.0f;
        yield return new WaitForSeconds(waitTime);

        li_Pool[key].gameObject.SetActive(false);
    }

}
