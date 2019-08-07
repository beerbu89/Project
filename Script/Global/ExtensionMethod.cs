using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using G_Define;

public static class ExtensionMethod
{
    // TableEnum
    public static string ToDesc(this System.Enum eEnumVal)
    {
        var da = (DescriptionAttribute[])(eEnumVal.GetType().GetField(eEnumVal.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
        return da.Length > 0 ? da[0].Description : eEnumVal.ToString();
    }

    public static eSummonsUnit GetSummonsUnit(this eSummonsUnit eSummonsUnit)
    {
        eSummonsUnit = (eSummonsUnit)Random.Range((int)eSummonsUnit.box_man, (int)eSummonsUnit.surgeon);

        return eSummonsUnit;
    }

    public static void HeroSetData(this Hero _hero, Data _data)
    {
        _hero.nAtk       = _data.nAtk;
        _hero.nHp        = _data.nHp;
        _hero.nDef       = _data.nDef;
        _hero.nAtkRange  = _data.nAtkRange;
        _hero.nLevel     = _data.nLevel;
        _hero.nExp       = _data.nExp;
        _hero.fSpeed     = _data.fSpeed;
    }

    public static void MonsterSetData(this Monster _monster, MonsterData _data)
    {
        _monster.nAtk      = _data.nAtk;
        _monster.nHp       = _data.nHp;
        _monster.nDef      = _data.nDef;
        _monster.nAtkRange = _data.nAtkRange;        
    }

    public static bool Distance(Vector3 pos, Vector3 targetPos, int nAtkRange)
    {
        Vector3 offset = pos - targetPos;
        float distance = offset.sqrMagnitude;
     
        return distance <= nAtkRange * nAtkRange; 
    }

    //Table 값
    public static LoginData GetLoginTb(this string id)
    {
        return TableMng.Ins.GetValue(id);
    }

    public static PlayerData GetPlayerTb(this string id)
    {
        return TableMng.Ins.GetPlayrtValue(id);
    }

    public static LevelData GetLevelTb(this int level)
    {
        return TableMng.Ins.GetLevelValue(level);
    }

    public static SummonsUnitData GetSummonsUnitTb(this int eID)
    {
        return TableMng.Ins.GetSummonsUnitValue(eID);
    }

    public static void GetQuestTb(this int nQuestType)
    {
        TableMng.Ins.GetQuestValue(nQuestType);
    }

    public static StageData GetStageTb(this eTile tile)
    {
        return TableMng.Ins.GetStageValue(tile);
    }

    public static MonsterData GetMonsterData(this eMonster eMonster)
    {
        return TableMng.Ins.GetMonsterValue(eMonster);
    }

    public static StageRewardData GetStageRewardData(int stageNum)
    {
        return TableMng.Ins.GetStageRewardValue(stageNum);
    }

    public static int GetHeroLevelData(int level)
    {
        return TableMng.Ins.GetHeroLevelValue(level);
    }

    public static QuestData GetQuestData(int eID)
    {
        return TableMng.Ins.GetQuestData(eID);
    }

    public static QuestRewardData  GetQuestRewardData(int stage)
    {
        return TableMng.Ins.GetQuestRewardData(stage);
    }

    public static HeroSkillData GetHeroSkillData(this eHero eHero)
    {
        return TableMng.Ins.GetValue<int, HeroSkillData>(eTable.HeroSkill, (int)eHero);
    }

    public static HeroSkillDamageData GetHeroSkillDamageData(this eHeroSkill eSkill)
    {
        return TableMng.Ins.GetValue<int, HeroSkillDamageData>(eTable.SkillDamage, (int)eSkill);
    }

    public static ItemData GetItemType(this eItem eItem)
    {
        return TableMng.Ins.GetValue<int, ItemData>(eTable.Item, (int)eItem);
    }
}
