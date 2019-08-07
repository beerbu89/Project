using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;
using System;
    

public class Hero : Characters
{
    public eHero eHero;
    public int   currentHp;    
    public bool  bAtkState;

    bool bIdleState;    
    bool bDamageState;
    bool bRunState;
    bool bDieState { get { return currentHp <= 0; } }   

    public UI_HPBar hpBar = null;

    public Data data;

    Animator anim;
    float    time        = 1.0f;
    float    currentTime = 0.0f;

    public void Init()
    {
        nAtk         = 0;
        nHp          = 0;
        nDef         = 0;
        nAtkRange    = 0;
        nLevel       = 0;
        nExp         = 0;
        currentTime  = time;
        bIdleState   = false;
        bAtkState    = false;
        bDamageState = false;
        bRunState    = false;        
        data         = null;
    }

    private void Start()
    {
        Init();        
        SetData();
        anim = GetComponent<Animator>();        
    }

    private void Update()
    {
        if(!bDieState && BattleMng.Ins.battleStart)
        {
            bRunState = true;

            if(bRunState)
            {
                anim.SetBool("RnuState", bRunState);

                if(BattleMng.Ins.monster != null)
                {
                    this.transform.LookAt(BattleMng.Ins.monster.transform);

                    float step = fSpeed * Time.deltaTime;

                    //transform.position = Vector3.MoveTowards(transform.position, BattleMng.Ins.monster.transform.position, step);

                    bAtkState = ExtensionMethod.Distance(this.transform.position, BattleMng.Ins.monster.transform.position, nAtkRange);

                    if(!bAtkState)
                    {
                        transform.Translate(Vector3.forward * step);
                    }
                }
            }
        }
        else if(bDieState && BattleMng.Ins.battleStart)
        {
            bAtkState  = false;
            bIdleState = true;
            bRunState  = false;

            anim.SetBool("IdleState", bIdleState);
            anim.SetBool("AtkState", bAtkState);
            anim.SetBool("RunState", bRunState);
            anim.SetFloat("DieState", -0.1f);

            StartCoroutine(Die());
            return;
        }

        if(bAtkState)
        {
            bRunState  = false;
            bIdleState = false;
            anim.SetBool("RnuState", bRunState);
            anim.SetBool("AtkState", bAtkState);
            anim.SetBool("bIdleState", bIdleState);

            if(currentTime > 0.0f)
            {
                currentTime -= Time.deltaTime;

                if (currentTime < 0.0f)
                {
                    Define.Attack_ToMonster(BattleMng.Ins.monster, data);

                    var temp = BattleUIMng.Ins.hitText.GetHitText();

                    BattleUIMng.Ins.hitTextUI.ActionText(BattleMng.Ins.monster, temp,nAtk);                   

                    BattleUIMng.Ins.monsterHPBar.HpDown(nAtk);

                    currentTime = time;
                }
            }
        }

        if(bDamageState)
        {           
            StartCoroutine(Damage());          
        }
    }

    public void SetData()
    {
        Hero currentHero = this;

        var list = BattleMng.Ins.heroDatas.GetEnumerator();

        int idx = 0;

        while(list.MoveNext())
        {
            var temp = list.Current;

            eHero eHeroTemp = (eHero)Enum.Parse(typeof(eHero), temp.strName);

            if(eHero == eHeroTemp)
            {
                data = temp;
                break;
            }
            ++idx;
        }

        ExtensionMethod.HeroSetData(currentHero, data);

        currentHp = nHp;
    }

    void Clear()
    {
        nAtk      = 0;
        nHp       = 0;
        nDef      = 0;
        nAtkRange = 0;
        nLevel    = 0;
        nExp      = 0;
    }

    //해당 영웅이 전투에 참여하고, 아이템도 착용하고 있을 시 스텟 변경
    public bool statFlag = false;
    public void PlusStat(List<int> li_Temp)
    {
        nAtk += li_Temp[0];
        nDef += li_Temp[1];
        nHp  += li_Temp[2];

        currentHp = nHp;
        statFlag  = !statFlag;
    }

    public void MinusStat(List<int> li_Temp)
    {
        nAtk -= li_Temp[0];
        nDef -= li_Temp[1];
        nHp  -= li_Temp[2];

        currentHp = nHp;
        statFlag = !statFlag;
    }

    IEnumerator Damage()
    {
        bAtkState = false;
        anim.SetBool("AtkState", bAtkState);

        anim.SetBool("DamageState", bDamageState);   

        yield return new WaitForSeconds(0.3f);
        bDamageState = false;
        anim.SetBool("DamageState", bDamageState);
        bAtkState = true;
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2.0f);
        BattleMng.Ins.HeroDie(this);
    }

}
