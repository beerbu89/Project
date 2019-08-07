using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;
using System.IO;
namespace G_Define
{
    //const
    public static class Define
    {
        public const int nRANDOM_POOL        = 1024;
        //10회소환
        public const int nSUMMONSSLOT_SIZE   = 10;
        //인벤Hero슬롯개수
        public const int nINVENTORYSLOT_SIZE = 40;
        public const int nBATTLESLOT_SIZE    = 4;

        public const string sQUESTPOPUP_TEXT1  = "이미 완료한 퀘스트입니다";
        public const string sQUESTPOPUP_TEXT2  = "퀘스트를 완료하세요";
        public const string sQUESTPOPUP_TEXT3  = "진행중인 퀘스트가 아닙니다";
        public const string sINVENITEMSLOT_TAG = "InvenItemSlot";
        public const string sITEM_TAG1         = "Weapon";
        public const string sITEM_TAG2         = "Top";
        public const string sITEM_TAG3         = "Bottom";
        public const string sITEM_TAG4         = "Arm";
        public const string sITEM_TAG5         = "Helmet";
        public const string sITEM_TAG6         = "Leg";



        public static void Attack_ToMonster(Monster refMonsterStat, Data heroData)
        {
            refMonsterStat.currentHp -= heroData.nAtk;
        }

        public static void Attack_ToMonster(Monster refMonsterStat, HeroSkillDamageData skillDamage)
        {
            if(BattleMng.Ins.monster == null) { return; }
            refMonsterStat.currentHp -= skillDamage.nDamage;
        }

        public static void Attack_ToHero(Hero refHeroStat, Monster monsterData)
        {
            refHeroStat.currentHp -= monsterData.nAtk;
        }

    }

    public static class Path
    {
        public const string HEROPREFAB        = "Prefab\\Hero\\{0}";
        public const string IMG               = "Prefab\\Img\\{0}";
        public const string TILE              = "Prefab\\Tile\\{0}";
        public const string MONSTERPREFAB     = "Prefab\\Monster\\{0}";
        public const string HEROCOLOR         = "Prefab\\UI\\Image";
        public const string HEROIMAGE         = "Prefab\\UI\\InvenHeroImage";
        public const string PLAYERDATA        = "/Resources/Data/PlayerData.Json";
        public const string HERODATA          = "/Resources/Data/HeroData.Json";
        public const string QUESTFLAGEDATA    = "/Resources/Data/QuestFlagData.Json";
        public const string QUESTOPENDATA     = "/Resources/Data/QuestOpenData.Json";
        public const string TILEDATA          = "/Resources/Data/TileData.Json";
        public const string STAGEDATA         = "/Resources/Data/StageData.Json";
        public const string TILEIDX           = "/Resources/Data/TileIdxData.Json";
        public const string SKILL             = "Prefab\\Skill\\Skill{0}";
        public const string TEXT              = "Prefab\\UI\\HitText";
        public const string EQUIPDATAKEY      = "/Resources/Data/EquipDataKey.Json";
        public const string EQUIPDATAVALUE    = "/Resources/Data/EquipDataValue.Json";
        public const string INVENITEM         = "/Resources/Data/InvenItemData.Json";
    }

    public static class Ext
    {
        public static LoginData LoginData(this int a_nID)
        {
            LoginData data = null;

            var map = TableMng.Ins.loginTb.GetTable();
            map.TryGetValue(a_nID, out data);

            return data;
        }

        public static List<string> GetFieldDesc(this System.Type t)
        {
            List<string> li = new List<string>();
            var arr = t.GetProperties();

            foreach (var val in arr)
            {
                var ar = (DescriptionAttribute[])val.GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (ar != null && ar.Length > 0)
                {
                    li.Add(ar[0].Description);
                }
            }
            return li;
        }
    }

    //public static class Rand
    //{
    //    static int   idx   = 0;
    //    static int[] array = new int[Define.nRANDOM_POOL];
    //
    //    static Rand()
    //    {
    //        for(int i=0; i<Define.nRANDOM_POOL; ++i)
    //        {
    //            array[i] = UnityEngine.Random.Range(0, 10000);
    //        }
    //    }
    //
    //    static int nIndex
    //    {
    //        get
    //        {
    //            int temp = idx++;
    //
    //            if(idx > Define.nRANDOM_POOL)
    //            {
    //                idx = 0;
    //            }
    //
    //            return temp;
    //        }
    //    }
    //
    //    public static int Random()
    //    {
    //        return array[nIndex];
    //    }
    //}

    //enum
    public enum eTableType
    {
        //회원가입&&LoginDB
        [Description("1FAIpQLSfxypaNAv-q27MsLklvmtHItUucOZwcuFUWna69UqZRrFuX6w")]        
        GoogleForm,        
        [Description("1pzPdKKx-gDpLb6GwavAEJejnyNLmWm8PrAtMICeSaT4")]
        GoogleFormDB,

        //PlayerDB        
        [Description("1FAIpQLSdc96avFv0TxMLNONsduC8W9MBDC0JCV-BTJVfbQa4XtwpKVA")]
        GooglePlayerForm,        
        [Description("1UvVNWAxH9ezi_CnA1zeb5329UQQ_WvMbl_zDBQkIqek")]
        GooglePlayerDB,

        //Table
        [Description("1WJoue3Y-oKEByy9saq3Y_juh8jomNf1gZBZihJIHvCI")]
        Table,
    }

    public enum eTable
    {
        //디비
        [Description("1722797036")]
        Login,            
        [Description("103305190")]
        Player,

        //Table
        //플레이어 초기값 테이블
        [Description("0")]
        PlayerFirstInit,
        [Description("1529986754")]
        Level,
        [Description("1143781854")]
        Shop,
        //소환확률
        [Description("2053570072")]
        Summons,
        //소환유닛
        [Description("400175052")]
        SummonsUnit,
        [Description("1523192222")]
        Hero,
        [Description("1235317899")]
        Quest,
        [Description("178002054")]
        Map,
        [Description("818239045")]
        Stage,
        [Description("201755030")]
        Monster,
        [Description("1410687451")]
        StageReward,
        [Description("67983173")]
        HeroLevel,
        [Description("1774275838")]
        QuestReward,
        [Description("774796405")]
        HeroSkill,
        //HeroSkillDamage
        [Description("1192645593")]
        SkillDamage,
        [Description("125310116")]
        Item,
        Max,
    }
    
    public enum eScene
    {
        LodingScene,
        LobbyScene,
        WorldMapScene,
        BattleScene,
    }

    public enum eShopCategory
    {
        Gem,
        Gold,
    }

    //영웅 속성
    public enum eHeroProperty
    {
        None = 0,
        Ice,
        Fire,
        Forest,

        Max,
    }

    //영웅 속성에 따른 이미지 컬러
    public enum eHeroPropertyColor
    {       
        Blue,
        Red,
        Green,

        Max,
    }

    //Resource이름과 동일하게 하기위해
    public enum eSummonsUnit
    {
        None    = 0,
        box_man = 101,
        fogaman,
        greenwar,
        king,
        knight,
        magic_plant,
        nurse,
        pirate,
        sparcher,
        surgeon,
    }

    public enum eHero
    {
        None    =0,
        box_man,
        fogaman,
        greenwar,
        king,
        knight,
        magic_plant,
        nurse,
        pirate,
        sparcher,
        surgeon,

        Max,
    }

    public enum eMonster
    {
        mon01 = 0,
        mon02,
        mon03,
        mon04,

        Max,
    }

    public enum eQuestType
    {       
        None = 0,
        Forest,
        Fire,
        Ice,

        Max,
    }

    public enum eTile
    {
        None = 0,
        Forest,
        Fire,
        Ice,

        Max,
    }

    public enum eStage
    {
        None = 0,
        Forest1,
        Forest2,
        Forest3,
        Forest4,
        Fire1,
        Fire2,
        Fire3,
        Fire4,
        Ice1,
        Ice2,
        Ice3,
        Ice4,

        Max,
    }

    public enum eHeroSkill
    {   
        Skill1,
        Skill2,
        Skill3,
        Skill4,
        Skill5,
        Skill6,
        Skill7,
        Skill8,
        Skill9,
        Skill10,
        Skill11,
        Skill12,
    }

    public enum eInvenBtnType
    {
        Info,
        Equip,
    }

    public enum eItemType
    {
        Weapon,
        Top,
        Bottom,
        Arm,
        Helmet,
        Leg,
    }

    public enum eItem
    {
        None = 0,
        sword1,
        axe1,
        top1,
        top2,
        bottom1,
        bottom2,
        Arm1,
        Arm2,
        helmet1,
        helmet2,
        shoes1,
        shoes2,
        
        Max,
    }

    //public interface IState
    //{
    //    void Attack (float time);
    //    void Die    (float time);
    //    void Damage (float time);
    //    void Run    (float time);
    //    void Idle   (float time);
    //}

    public class Data
    {
        public int    nID;
        public int    eHeroProperty;
        public string strName;
        public string strPrefab;
        public int    nAtk;
        public int    nHp;
        public int    nDef;
        public int    nAtkRange;
        public int    nLevel;
        public int    nExp;
        public int    fSpeed;

        public void SetData(int a, int b, string c, string d, int e, int f, int g, int h, int i, int j,int k)
        {
            nID           = a;
            eHeroProperty = b;
            strName       = c;
            strPrefab     = d;
            nAtk          = e;
            nHp           = f;
            nDef          = g;
            nAtkRange     = h;
            nLevel        = i;
            nExp          = j;
            fSpeed        = k;
        }
    }

    //public class ItemStat
    //{
    //    public int nAtk;
    //    public int nDef;
    //    public int nHp;
    //
    //    public ItemStat(int _nAtk, int _nDef, int _nHp)
    //    {
    //        nAtk = _nAtk;
    //        nDef = _nDef;
    //        nHp  = _nHp;
    //    }
    //}

}

