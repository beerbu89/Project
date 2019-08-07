using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class UI_Inventory : MonoBehaviour
{
    //슬롯
    public Image                   prefab;
    //HeroImage                    
    public Image                   imgPrefab;                    
    public Transform               heroSlotRoot;
    public int                     heroCount       = 0;
    public int                     seeSlotSize     = 20;
    public Text                    heroCountText;  
    //public List<HeroData>          heroDataList     = new List<HeroData>(); 
    //이미지 레이캐스트
    public GraphicRaycaster        gr;
    #region BASICDATA
    public Text                    nameText;       
    public Text                    levelText;      
    public Text                    expText;        
    public Text                    atkText;        
    public Text                    hpText;         
    public Text                    defText;        
    public Text                    propertyText;
    #endregion
    //인벤토리 확장을 위한 관리 리스트                
    public Dictionary<Image, bool> flagList        = new Dictionary<Image, bool>();
    //속성마다 색 바꾸는거(빨,녹,파)
    public List<Image>             heroSlotList    = new List<Image>();
    public Transform               popup;

    //정보나 장비 클릭시 육안으로 확인하기 위해
    public Image  info;    
    public Image  equip;
    public Image  infoImg;
    public Image  equipImg;
    public Sprite basicInvenSlotImg;
    //아이템 슬롯에서 클릭이 발생할경우, 장비를 장착할꺼냐 아니냐에 대한 팝업
    public GameObject equipMounting;
    //장비 슬롯에서 클릭이 발생할경우 = 해제
    public GameObject equipClear;

    public List<Image> invenItemSlotList;
    public List<Image> equipList;

    public Dictionary<eItemType, Image> equipSlotTypeList = new Dictionary<eItemType, Image>();

    public Text itemAtkText;
    public Text itemDefText;
    public Text itemHpText;

    public int idx = 0;

    eHero                   eHeroName     = eHero.None;
    eHeroProperty           eHeroProperty = eHeroProperty.None; 
    PointerEventData        ped           = new PointerEventData(null);    
    //Hero이미지
    List<Image>             heroList      = new List<Image>();    
    Characters              characters    = new Characters();    
    
    int                     heroIdx       = 0;

    const string CLOSETAG      = "Close";
    const string OPENTAG       = "InvenHero";
    const int    OPENSLOTSIZE  = 10;
    const int    EQUIPSLOTSIZE = 6;

    public string[] sBasicEquipImg = new string[EQUIPSLOTSIZE];

    private void Update()
    {       
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject() == true)
            {
                ped.position = Input.mousePosition;
                List<RaycastResult> result = new List<RaycastResult>();

                gr.Raycast(ped, result);

                if (result.Count != 0 && result[0].gameObject.CompareTag("InvenHero") && gameObject.activeSelf == true)
                {
                    var list = Player.Ins.heroDataList.GetEnumerator();

                    heroIdx = 0;

                    //클릭한 hero 정보나타내기
                    while (list.MoveNext())
                    {
                        var data = list.Current;

                        if (result[0].gameObject == heroList[heroIdx].gameObject)
                        {
                            HeroInfo(data);                            
                            HeroEquipImg();

                            ItemText();
                            break;
                        }
                        ++heroIdx;
                    }
                }

                //슬롯 추가
                if (result.Count != 0 && result[0].gameObject.CompareTag(CLOSETAG))
                {
                    PlusSlot();
                }

                //장비장착
                if(result.Count != 0 && result[0].gameObject.CompareTag(Define.sINVENITEMSLOT_TAG))
                {
                    SetEquipImg(result[0].gameObject);
                }

                //장비해제
                if(result.Count != 0 && result[0].gameObject.CompareTag(Define.sITEM_TAG1) || result[0].gameObject.CompareTag(Define.sITEM_TAG2) ||
                    result[0].gameObject.CompareTag(Define.sITEM_TAG3) || result[0].gameObject.CompareTag(Define.sITEM_TAG4) || result[0].gameObject.CompareTag(Define.sITEM_TAG5) ||
                    result[0].gameObject.CompareTag(Define.sITEM_TAG6))
                {
                    ClearEquipItem(result[0].gameObject);
                }
            }
           
        }
    }
 
    public void Init()
    {   
        for (int i = 0; i < Define.nINVENTORYSLOT_SIZE; ++i)
        {
            //속성 슬롯 
            Image newSlot = Instantiate(prefab);
            newSlot.transform.parent = heroSlotRoot;
            //hero 이미지 
            Image newHero = Instantiate(imgPrefab);
            newHero.transform.parent = newSlot.transform;

            if (i >= seeSlotSize)
            {   
                string changeImg = string.Format(Path.IMG, "Close");
                newHero.sprite   = Resources.Load<Sprite>(changeImg) as Sprite;
                newHero.color    = Color.black;
                newHero.tag      = CLOSETAG;
            }

            //속성 슬롯 pool
            heroSlotList.Add(newSlot);
            //hero 이미지 pool
            heroList.Add(newHero);
            //인벤 슬롯 pool
            flagList.Add(heroSlotList[i], false);
        }
        
        for (int i = 0; i < seeSlotSize; ++i)
        {
            flagList[heroSlotList[i]] = true;
        }

        //플레이어가 보유한 히어로 수
        var size = Player.Ins.heroDataList.Count;

        //히어로 데이터가 0이 아닐시 각 슬롯마다 영웅에따라 이미지랑 속성 표시
        if(size > 0)
        {
            for (int i = 0; i < size; ++i)
            {
                heroSlotList[i].color     = Player.Ins.invenSlotList[i].color;
                heroList[i].sprite        = Player.Ins.heroImgList[i].sprite;
                flagList[heroSlotList[i]] = true;
                heroList[i].tag           = OPENTAG;
                heroList[i].color         = Color.white;
        
                Player.Ins.invenSlotList[i] = heroSlotList[i];
                Player.Ins.heroImgList[i]   = heroList[i];
            }
        }   

        //장비슬롯 타입 세팅
        for(int i=0; i<equipList.Count; ++i)
        {
            eItemType eType = (eItemType)i;

            equipSlotTypeList.Add(eType, equipList[i]);
        }

        for(int i=0; i< sBasicEquipImg.Length; ++i)
        {
            sBasicEquipImg[i] = equipList[i].sprite.name;
        }

        if(Player.Ins.heroEquipList.Count != 0)
        {
            ItemText();
        }
        else
        {
            itemAtkText.text = string.Format("+ 0");
            itemDefText.text = string.Format("+ 0");
            itemHpText.text  = string.Format("+ 0");
        }
    }

    public void SetVisible(bool active)
    {      
        if (active == true)
        {
            bool flag = SceneMng.Ins.PopupSetActive(this.gameObject);

            if (!flag)
            {
                return;
            }
        }

        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }

        if(gameObject.activeSelf == true)
        {
            HeroCountText(heroCountText);

            var list = Player.Ins.heroDataList.GetEnumerator();

            //인벤토리 창 활성시 첫 영웅에 대한 정보 나타내기
            while(list.MoveNext())
            {
                var data = list.Current;

                if(data.nID == 0 )
                {
                    return;
                }
                else
                {
                    HeroInfo(data);
                    break;
                }
            }
        }
    }

    void SetVisible(GameObject obj, bool active)
    {
        if(obj.activeSelf != active)
        {
            obj.SetActive(active);
        }
    }

    //영웅 정보 장비 탭
    public void ButtonArea(int num)
    {
        eInvenBtnType eType = (eInvenBtnType)num;

        switch (eType)
        {
            case eInvenBtnType.Info:
                info.color  = Color.red;
                equip.color = Color.white;
                SetVisible(infoImg.gameObject, true);
                SetVisible(equipImg.gameObject, false);
                ItemText();
                break;
            case eInvenBtnType.Equip:
                info.color  = Color.white;
                equip.color = Color.red;
                SetVisible(infoImg.gameObject, false);
                SetVisible(equipImg.gameObject, true);

                if(Player.Ins.heroEquipList.ContainsKey(heroIdx))
                {
                    HeroEquipImg();
                }
                
                ItemSlotSort();
                break;
        }
    }

    public bool Check()
    {
        if (flagList[heroSlotList[idx]] == false)
        {
            Debug.LogError("슬롯이 닫혀있음");
            return false;
        }

        return true;
    }

    public bool Check(Image[] Bg)
    {
        if (flagList[heroSlotList[idx + Bg.Length - 1]] == false)
        {
            Debug.LogError("슬롯이 닫혀있음");
            return false;
        }
        return true;
    }

    //1회 소환시 인벤추가
    public void SetHeroImg(Image Bg, Image _hero)
    {
        //Debug.Log(idx); 
        //if (flagList[heroSlotList[idx]] == false)
        //{
        //    Debug.LogError("슬롯이 닫혀있음");
        //    return;
        //}

        heroSlotList[idx].color  = Bg.color;
        heroList[idx].sprite     = _hero.sprite;
        
        HeroProperty(heroSlotList[idx].color);
      
        //Debug.LogError(eHeroProperty.ToString());

        eHeroName = (eHero)Enum.Parse(typeof(eHero), heroList[idx].sprite.name);

        if(eHeroName != eHero.None)
        {
            characters.SetData(eHeroProperty, (int)eHeroName);
        }

        Player.Ins.invenSlotList.Add(heroSlotList[idx]);
        Player.Ins.heroImgList.Add(heroList[idx]);

        ++idx;
        ++heroCount;
    }

    //10회 소환
    public void SetHeroImg(Image[] Bg, Image[] _hero)
    {
        //if (flagList[heroSlotList[idx+Bg.Length-1]] == false)
        //{
        //    Debug.LogError("슬롯이 닫혀있음");
        //    return;
        //}

        for (int i=0; i<Bg.Length; ++i)
        {
            heroSlotList[idx].color = Bg[i].color;
            heroList[idx].sprite    = _hero[i].sprite;

            HeroProperty(heroSlotList[idx].color);

            eHeroName = (eHero)Enum.Parse(typeof(eHero), heroList[idx].sprite.name);

            if (eHeroName != eHero.None)
            {
                characters.SetData(eHeroProperty, (int)eHeroName);
            }

            Player.Ins.invenSlotList.Add(heroSlotList[idx]);
            Player.Ins.heroImgList.Add(heroList[idx]);

            ++idx;
            ++heroCount;
        }
        
    }

    public void HeroCountText(Text t)
    {
        heroCount = Player.Ins.heroDataList.Count;
        t.text             = string.Format("{0}/{1} 영웅보유", heroCount, seeSlotSize);
        heroCountText.text = string.Format("{0}/{1}", heroCount, seeSlotSize);
    }
    
    public void ItemText()
    {
        Item(heroIdx);

        if (heroItemStat.Count == 0)
        {
            itemAtkText.text = string.Format("+ 0");
            itemDefText.text = string.Format("+ 0");
            itemHpText.text  = string.Format("+ 0");
            return;
        }

        int nAtk = heroItemStat[0];
        int nDef = heroItemStat[1];
        int nHP  = heroItemStat[2];

        itemAtkText.text = string.Format("+ {0}",nAtk);
        itemDefText.text = string.Format("+ {0}",nDef);
        itemHpText.text  = string.Format("+ {0}",nHP);
    }

    List<string> heroEquipStat = new List<string>();
    public List<int> heroItemStat;
    public void Item(int heroIdx)
    {
        heroEquipStat.Clear();
        heroItemStat.Clear();

        if (!Player.Ins.heroEquipList.ContainsKey(heroIdx)) { return; }

        var temp = Player.Ins.heroEquipList[heroIdx];

        for (int i = 0; i < temp.Count; ++i)
        {
            var path = string.Format(Path.IMG, temp[i]);
            var img  = Resources.Load<Sprite>(path) as Sprite;

            var path1 = string.Format(Path.IMG, sBasicEquipImg[i]);
            var img1  = Resources.Load<Sprite>(path1) as Sprite;

            if(img != img1)
            {
                heroEquipStat.Add(temp[i]);
            }
        }

        heroItemStat = ItemMng.Ins.GetItemStat(heroEquipStat);
    }
    
    void HeroProperty(Color eColor)
    {   
        switch (eColor)
        {
            case Color color when eColor ==  Color.blue:
                eHeroProperty = eHeroProperty.Ice;
                break;
            case Color color when eColor == Color.red:
                eHeroProperty = eHeroProperty.Fire;
                break;
            case Color color when eColor == Color.green:
                eHeroProperty = eHeroProperty.Forest;
                break;
            default:
                break;
        }    
    }

    void HeroInfo(Data info)
    {        
        nameText.text     = string.Format("{0}",info.strName); 
        levelText.text    = string.Format("{0}",info.nLevel);
        expText.text      = string.Format("{0}",info.nExp);
        atkText.text      = string.Format("{0}",info.nAtk);
        hpText.text       = string.Format("{0}",info.nHp);;
        defText.text      = string.Format("{0}",info.nDef);

        eHeroProperty eProperty = (eHeroProperty)info.eHeroProperty;

        propertyText.text = string.Format(eProperty.ToString());
    }

    
    void PlusSlot()
    {
        int price = 100000;

        if(price > Player.Ins.playerData.nMoney)
        {
            StartCoroutine(Popup());
            return;
        }
        

        for(int i= seeSlotSize; i<seeSlotSize + OPENSLOTSIZE; ++i)
        {
            flagList[heroSlotList[i]] = true;
            heroList[i].sprite        = null;
            heroList[i].tag           = OPENTAG;
            heroList[i].color         = Color.white;
        }

        seeSlotSize += OPENSLOTSIZE;

        HeroCountText(heroCountText);
    }

    IEnumerator Popup()
    {
        popup.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.0f);

        popup.gameObject.SetActive(false);
    }
    
    //인벤 아이템 슬롯 리스트
    void ItemSlotSort()
    {
        int size = ItemMng.Ins.li_Item.Count;

        //인벤슬롯 관련
        for (int i = 0; i < size; ++i)
        {
            var path = string.Format(Path.IMG, ItemMng.Ins.li_Item[i]);
            var img = Resources.Load<Sprite>(path) as Sprite;
            invenItemSlotList[i].sprite = img;
        }

        if (size != invenItemSlotList.Count)
        {
            for (int i = size; i < invenItemSlotList.Count; ++i)
            {
                invenItemSlotList[i].sprite = basicInvenSlotImg;
            }
        }
    }

    public List<string> equipStringList;

    int itemIdx    = 0;
    int equipIdx   = 0;

    //체인지할 이미지
    Sprite sprItem     = null;
    Sprite currentItem = null;

    void SetEquipImg(GameObject obj)
    {
        if (ItemMng.Ins.li_Item.Count == 0) { return; }
        equipStringList = new List<string>();

        //인벤 슬롯을 클릭했을 경우
        for (int i=0; i<invenItemSlotList.Count; ++i)
        {
            if(obj.gameObject == invenItemSlotList[i].gameObject)
            {   
                itemIdx = i;
                break;
            }
        }

        //==>클릭한 이미지의 장비아이템, 장비아이템 타입, 장비아이템 이미지
        eItem eItem = (eItem)Enum.Parse(typeof(eItem), ItemMng.Ins.li_Item[itemIdx]);
        var   type  = eItem.GetItemType();
        var   path  = string.Format(Path.IMG, ItemMng.Ins.li_Item[itemIdx]);
        var   img   = Resources.Load<Sprite>(path) as Sprite;
        //==<

        //체인지할 클릭한 이미지
        sprItem = img;

        //클릭한 아이템과 장비 슬롯에 타입이 같은지
        for (int i=0; i< equipList.Count; ++i)
        {
            //==>아이템 슬롯에서 클릭한 아이템 타입 찾기
            //var path = string.Format(Path.IMG, ItemMng.Ins.li_Item[itemIdx]);
            //var img  = Resources.Load<Sprite>(path) as Sprite;            
            eItemType eType = (eItemType)Enum.Parse(typeof(eItemType), type.eType);
            //==<

            //==>euqip 슬롯에서 같은 타입 찾기
            eItemType eEquipType = (eItemType)Enum.Parse(typeof(eItemType), equipList[i].tag);

            if(eType == eEquipType)
            {
                equipIdx = i;
                break;
            }
        }

        Debug.Log(itemIdx);
        Debug.Log(equipIdx);

        //아이템 착용시 해당 타입의 장비를 착용했는지 체크
        bool flag = false;

        //해당 영웅이 장비를 한번이라도 착용한적있나?
        if(Player.Ins.heroEquipList.ContainsKey(heroIdx))
        {
            //있다면 클릭한 장비의 타입을 착용했나?            
            //equip슬롯이 기본이미지라면 해당 타입의 장비는 착용x 아니면 착용
            var sPath = string.Format(Path.IMG, sBasicEquipImg[equipIdx]);
            var temp  = Resources.Load<Sprite>(sPath) as Sprite;
            
            if(equipList[equipIdx].sprite != temp) { flag = true; }            
        }   
        else
        {
            //해당 영웅이 장비를 착용한 적이 없음          
            EquipImg();

            for (int i=0; i<equipList.Count; ++i)
            {
                equipStringList.Add(equipList[i].sprite.name);
            }            

            Player.Ins.heroEquipList.Add(heroIdx, equipStringList);
            //GameMng.Ins.heroStat.PlusStat(heroIdx, sprItem);

            ItemMng.Ins.RemoveItem(itemIdx);

            ItemSlotSort();
            return;
        }

        if (flag)
        {
            //장비는 한개라도 착용했고 해당 타입의 장비도 착용했다           
            //장비스왑
            equipMounting.gameObject.SetActive(true);
            return;
        }
        else
        {
            //장비는 한개라도 착용했고 해당 타입의 장비는 착용하지 않았다.
            EquipImg();

            for (int i = 0; i < equipList.Count; ++i)
            {
                equipStringList.Add(equipList[i].sprite.name);
            }

            Player.Ins.heroEquipList[heroIdx] = equipStringList;
            //GameMng.Ins.heroStat.PlusStat(heroIdx, sprItem);

            ItemMng.Ins.RemoveItem(itemIdx);            
        }

        ItemSlotSort();
    }

    void EquipImg()
    {
        equipList[equipIdx].sprite = sprItem;
        invenItemSlotList[itemIdx].sprite = basicInvenSlotImg;
    }

    public void EquipMounting(bool active)
    {
        if(active)
        {
            //아이템 스왑시 착용하고있던 장비는 없어짐

            equipMounting.gameObject.SetActive(false);

            currentItem = equipList[equipIdx].sprite;

            EquipImg();

            for (int i = 0; i < equipList.Count; ++i)
            {
                equipStringList.Add(equipList[i].sprite.name);
            }

            Player.Ins.heroEquipList[heroIdx] = equipStringList;
            //GameMng.Ins.heroStat.PlusStat(heroIdx, currentItem.name,sprItem.name);

            ItemMng.Ins.RemoveItem(itemIdx);
            ItemSlotSort();
            
        }

        equipMounting.gameObject.SetActive(false);
        return;
    }

    void HeroEquipImg()
    {
        Debug.Log(heroIdx);
        if (Player.Ins.heroEquipList.ContainsKey(heroIdx))
        {
            var list = Player.Ins.heroEquipList[heroIdx];

            EquipView(list);      
        }
        else
        {
            EquipView(sBasicEquipImg);     
        }
    }

    public List<string> li_CurrentItem = new List<string>();

    void EquipView(List<string> list)
    {
        li_CurrentItem.Clear();
        for (int i = 0; i < equipList.Count; ++i)
        {
            var path = string.Format(Path.IMG, list[i]);
            var img = Resources.Load<Sprite>(path) as Sprite;

            li_CurrentItem.Add(list[i]);

            equipList[i].sprite = img;
        }
    }

    void EquipView(string[] str)
    {
        for (int i = 0; i < equipList.Count; ++i)
        {
            var path = string.Format(Path.IMG, sBasicEquipImg[i]);
            var img = Resources.Load<Sprite>(path) as Sprite;

            equipList[i].sprite = img;
        }
    }

    //장비해제
    int equipClearIdx = 0;

    void ClearEquipItem(GameObject obj)
    {
        //장비 슬롯 클릭한 슬롯 찾기
        for(int i=0; i<equipList.Count; ++i)
        {
            if(obj == equipList[i].gameObject)
            {
                equipClearIdx = i;
                break;
            }
        }

        //클릭한 슬롯이 비어있는지 확인        
        for(int i=0; i<sBasicEquipImg.Length; ++i)
        {
            var sPath = string.Format(Path.IMG, sBasicEquipImg[i]);
            var temp  = Resources.Load<Sprite>(sPath) as Sprite;

            if(equipList[equipClearIdx].sprite == temp)
            {
                Debug.Log("착용한 장비가 없다");
                return;
            }
        }

        //해제 클릭시
        equipClear.SetActive(true);
        return;
       
    }

    public void ClickEquipClear(bool active)
    {
        equipStringList = new List<string>();

        if (active)
        {
            //착용한 장비가 있다면.
            var path = string.Format(Path.IMG, sBasicEquipImg[equipClearIdx]);
            var img = Resources.Load<Sprite>(path) as Sprite;
            var currentImg = equipList[equipClearIdx].sprite;
            //장비 슬롯 이미지를 기본 슬롯 이미지로
            equipList[equipClearIdx].sprite = img;

            //아이템슬롯 추가
            for (int i = 0; i < invenItemSlotList.Count; ++i)
            {
                //비어있는 슬롯 찾기
                if (invenItemSlotList[i].sprite == basicInvenSlotImg)
                {
                    //아이템 슬롯 이미지 변경
                    invenItemSlotList[i].sprite = currentImg;
                    ItemMng.Ins.li_Item.Add(invenItemSlotList[i].sprite.name);

                    for (int j = 0; j < equipList.Count; ++j)
                    {
                        equipStringList.Add(equipList[j].sprite.name);
                    }

                    Player.Ins.heroEquipList[heroIdx] = equipStringList;

                    //GameMng.Ins.heroStat.MinsuStat(heroIdx, currentImg);

                    equipClear.SetActive(false);
                    ItemText();
                    break;
                }
            }
        }

        equipClear.SetActive(false);
    }
}
