using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;


public class Monster : Characters
{
    public eMonster   eMonster;
    public List<Hero> currentTarget = new List<Hero>();
    public Hero       target        = null;
    public int        currentHp     = 0;

    bool bIdleState;
    bool bAtkState;
    bool bDamageState;
    bool bRunState;
    bool bDieState { get { return currentHp <= 0; } }

    float time        = 1.0f;
    float currentTime = 0.0f;
    Animator anim;

    private void Start()
    {
        Init();
        SetData();
        anim = GetComponent<Animator>();        
    }

    private void Update()
    {   
        if (!bDieState && BattleMng.Ins.battleStart)
        {
            if (bRunState)
            {
                anim.SetBool("RnuState", bRunState);

                GetTarget();
              
                if (target != null)
                {
                    this.transform.LookAt(target.transform.position);

                    float step = fSpeed * Time.deltaTime;

                    bAtkState = ExtensionMethod.Distance(this.transform.position, target.transform.position, nAtkRange);
                }

                if (bAtkState == false)
                {
                    this.transform.position -= Vector3.forward * Time.deltaTime;
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
            anim.SetBool("RnuState", bRunState);
            anim.SetInteger("DieState", -1);

            StartCoroutine(Die());
            return;
        }

        if (bAtkState)
        {
            bRunState = false;
            anim.SetBool("RnuState", bRunState);
            anim.SetBool("AtkState", bAtkState);

            if (currentTime > 0.0f)
            {
                currentTime -= Time.deltaTime;

                if (currentTime < 0.0f)
                {
                    Define.Attack_ToHero(target, BattleMng.Ins.monster);

                    var temp = BattleUIMng.Ins.hitText.GetHitText();

                    BattleUIMng.Ins.hitTextUI.ActionText(target, temp, nAtk);

                    target.hpBar.HpDown(BattleMng.Ins.monster.nAtk);
                    bDamageState = true;

                    currentTime = time;                    
                }
            }
        }

        if (target != null && target.currentHp <= 0)
        {
            currentTarget.Remove(target);
            target    = null;            
            bAtkState = false;
            bRunState = true;
        }
    }

    public void SetData()
    {
        Monster monster = this;

        //table
        var data = ExtensionMethod.GetMonsterData(eMonster);

        if(data == null) { Debug.LogError("data Error"); return; }

        ExtensionMethod.MonsterSetData(monster, data);
        BattleUIMng.Ins.monsterHPBar.SetHpBar(data);

        currentHp = nHp;

        //Debug.Log("monster : " + nHp);
    }

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
        bRunState    = true;
    }

    void GetTarget()
    {
        if(target != null) { return; }

        var a = currentTarget.GetEnumerator();

        while(a.MoveNext())
        {
            var b = a.Current;

            bool flag = ExtensionMethod.Distance(this.transform.position,b.transform.position,nAtkRange);

            if(flag == true)
            {
                target = b;
                return;
            }
        }
    }
    
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.transform.CompareTag("Hero"))
    //    {
    //        for (int i=0; i<BattleMng.Ins.heroList.Count; ++i)
    //        {
    //            if(BattleMng.Ins.heroList[i].bAtkState == true)
    //            {
    //                Define.Attack_ToMonster(this, BattleMng.Ins.heroList[i].data);                    
    //                BattleUIMng.Ins.monsterHPBar.HpDown(BattleMng.Ins.heroList[i].data.nAtk);
    //            }
    //        }
    //    }        
    //}

    IEnumerator Die()
    {
        yield return new WaitForSeconds(2.0f);
        BattleMng.Ins.MonsterDie();
    }
}
