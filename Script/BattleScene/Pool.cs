using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G_Define;

//Hero Monster Pool
public class Pool : MonoBehaviour
{
    const int nHERO_SIZE = 5;

    public List<Transform> heroPoolList    = new List<Transform>();
    public List<Transform> monsterPoolList = new List<Transform>();
    public List<eHero>     eHeroList       = new List<eHero>();
    public List<eMonster>  eMonsterList    = new List<eMonster>();

    public Transform heroRoot = null;

    Transform monsterRoot = null;

    GameObject obj = null;

    public void SetPool(Transform hero, Transform monster)
    {
        heroRoot    = hero;
        monsterRoot = monster;

        for (int i = 1; i < (int)eHero.Max; ++i)
        {
            eHero eHero = (eHero)i;
            SetHero(eHero);          
        }

        for (int i = 0; i < (int)eMonster.Max; ++i)
        {
            eMonster eMonster = (eMonster)i;
            string monsterPath = string.Format(Path.MONSTERPREFAB, eMonster.ToString());

            obj = Instantiate(Resources.Load(monsterPath)) as GameObject;

            obj.transform.parent = monsterRoot;
            obj.SetActive(false);

            monsterPoolList.Add(obj.transform);
            eMonsterList.Add(eMonster);
        }
    }

    void SetHero(eHero eHero)
    {
        for(int i=0; i<nHERO_SIZE; ++i)
        {
            string heroPath = string.Format(Path.HEROPREFAB, eHero.ToString());

            obj = Instantiate(Resources.Load(heroPath)) as GameObject;

            obj.transform.parent = heroRoot;
            obj.SetActive(false);

            heroPoolList.Add(obj.transform);
            eHeroList.Add(eHero);
        }
    }
}
