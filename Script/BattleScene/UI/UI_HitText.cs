using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum eType
{
    Monster,
    Hero,
    Skill,
}

public class UI_HitText : MonoBehaviour
{
    Text hitText;

    int monX  = 80;
    int monY  = 180;
    int heroY = 50;

    //Hero 기본공격
    public void ActionText(Monster monster, Text text, int nAtk)
    {
        hitText = text;
        CameraPos(monster.tag, monster.transform, nAtk);
        StartCoroutine(ActionUI(hitText));
    }

    //몬스터 공격
    public void ActionText(Hero hero, Text text, int nAtk)
    {
        hitText = text;
        CameraPos(hero.tag, hero.transform, nAtk);
        StartCoroutine(ActionUI(hitText));
    }

    //스킬 공격
    public void ActionText(Monster monster, Text text, int nAtk, string tag)
    {
        if (monster == null) { return; }

        hitText = text;
        CameraPos(tag, monster.transform, nAtk);
        StartCoroutine(ActionUI(hitText));
    }

    //CameraPos 및 Text
    void CameraPos(string tag,Transform tr, int nAtk)
    {
        eType eType = (eType)Enum.Parse(typeof(eType), tag);

        var uiPos = Camera.main.WorldToScreenPoint(tr.position);
        var pos   = uiPos;

        switch (eType)
        {
            case eType.Monster:
                pos.x = UnityEngine.Random.Range(pos.x - monX, pos.x + monX);
                pos.y += monY;
                hitText.color = Color.red;
                break;
            case eType.Hero:
                pos.y += heroY;
                hitText.color = Color.yellow;
                break;
            case eType.Skill:
                pos.x = UnityEngine.Random.Range(pos.x - monX, pos.x + monX);
                pos.y += monY;
                hitText.color = Color.green;
                break;
            default:
                break;
        }
        
        uiPos = pos;
        hitText.transform.position = uiPos;
        
        hitText.text  = string.Format("{0}", nAtk);
        hitText.gameObject.SetActive(true);
    }

    IEnumerator ActionUI(Text _hitText)
    {
        float y      = 15.0f;
        float fY     = _hitText.transform.position.y + y;
        float fSpeed = 15.0f;

        while (_hitText.transform.position.y <= fY)
        {
            var pos = _hitText.transform.position;
            pos.y += fSpeed * Time.deltaTime;

            _hitText.transform.position = pos;

            yield return null;
        }

        _hitText.gameObject.SetActive(false);
    }
}
