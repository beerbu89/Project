using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public enum eSetSprite
{
    BattleHeroList,
    BattleSlotList,
}

//전투입장 대기방
public class BattleWait : MonoBehaviour
{
    const string TAG = "BattleHeroList";

    public Transform             heroSlotRoot;

    //===> 
    //하단의 히어로 속성 슬롯
    public Image                 slotPrefab;
    //하단의 히어로 이미지 슬롯 
    public Image                 heroImgPrefab;
    //=====<

    //전투에 참여하는 슬롯
    public Image[]               battleSlot      = new Image[Define.nBATTLESLOT_SIZE];
    public GraphicRaycaster      gr;
    public Image                 monsterImg;
    public Button                battleBtn;
    public Text                  vigorText;
    public GameObject            popup;
                                
    List<Image>                 heroSlotList     = new List<Image>();
    List<Image>                 heroList         = new List<Image>();        
    PointerEventData            ped              = new PointerEventData(null);    
    Dictionary<int, bool>       heroSelectList   = new Dictionary<int, bool>();        

    //전투에 참여하는 슬롯에 대한 hero idx 저장 넣고 뺴주기 위해
    int[] arrayIdx = new int[Define.nBATTLESLOT_SIZE];

    int addIdx          = 0;
    int removeIdx       = 0;
    int battleSlotCount = 0;    

    private void Start()
    {
        int size = Player.Ins.heroDataList.Count;

        int x = 140;
        int y = 140;

        for (int i=0; i<size; ++i)
        {
            Image newSlot = Instantiate(slotPrefab);
            newSlot.transform.parent = heroSlotRoot;

            Image newHero = Instantiate(heroImgPrefab);
            newHero.transform.parent = newSlot.transform;

            newSlot.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);


            heroSlotList.Add(newSlot);
            heroList.Add(newHero);

            heroList[i].sprite    = Player.Ins.heroImgList[i].sprite;
            heroList[i].tag       = TAG;
            heroSlotList[i].color = Player.Ins.invenSlotList[i].color;

            //이미지 크기
            heroSlotList[i].GetComponent<RectTransform>().sizeDelta = new Vector2(x, y);

            heroSelectList.Add(i, false);
        }

        string sMonster   = string.Format(Path.IMG, BattleMng.Ins.sMonsterName);
        monsterImg.sprite = Resources.Load<Sprite>(sMonster) as Sprite;

        vigorText.text = string.Format("X {0}", BattleMng.Ins.nVigor);

        for(int i=0; i< arrayIdx.Length; ++i)
        {
            arrayIdx[i] = -1;
        }
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (BattleUIMng.Ins.message.gameObject.activeSelf == true) { return; }

            ped.position = Input.mousePosition;
            List<RaycastResult> result = new List<RaycastResult>();

            gr.Raycast(ped, result);

            for (int i = 0; i < heroList.Count; ++i)
            {
                //클릭한것과 리스트가 같은지
                if (result[0].gameObject == heroList[i].gameObject)
                {
                    addIdx = i;

                    //같다면 선택한 영웅인지 아닌지
                    if (heroSelectList[i] == false && battleSlotCount < battleSlot.Length)
                    {
                        heroSelectList[i] = true;
                    }
                    else if (battleSlotCount >= battleSlot.Length)
                    {
                        Debug.Log(battleSlotCount);
                        return;
                    }
                    else
                    {
                        Debug.LogError("이미 선택된 영웅입니다");
                        StartCoroutine(PopupText());
                        return;
                    }
                    break;
                }
            }


            //영웅선택
            if (result.Count != 0 && result[0].gameObject.CompareTag("BattleHeroList"))
            {
                SetSlotSprite(result[0].gameObject, result[0].gameObject.tag);
            }

            //전투 가능한 영웅 슬롯에서 삭제
            if (result.Count != 0 && result[0].gameObject.CompareTag("BattleSlotList"))
            {
                SetSlotSprite(result[0].gameObject, result[0].gameObject.tag);
            }
        }
        
    }

    public void SetSlotSprite(GameObject obj, string s)
    {
        eSetSprite eSprite = (eSetSprite)Enum.Parse(typeof(eSetSprite),s);

        var list = Player.Ins.heroDataList.GetEnumerator();

        removeIdx = 0;

        switch (eSprite)
        {
            //Add
            case eSetSprite.BattleHeroList:

                //if (BattleMng.Ins.heroDatas.Count >= battleSlot.Length) { return; }
                //int arrayIdx = 0;
                
                if(battleSlotCount >= battleSlot.Length) { return; }

                //슬롯이미지 추가
                for (int i = 0; i < battleSlot.Length; ++i)
                {
                    if (battleSlot[i].sprite == null)
                    {
                        var image = obj.gameObject.GetComponent<Image>();

                        battleSlot[i].sprite = image.sprite;
                        arrayIdx[i] = addIdx;
                        //image.raycastTarget  = false;
                        break;
                        
                    }
                }

                ++battleSlotCount;  
                break;

            //Remove
            case eSetSprite.BattleSlotList:

                if(obj.GetComponent<Image>().sprite == null) { return; }

                var battleHeroList = BattleMng.Ins.heroDatas.GetEnumerator();

                //이미지삭제
                for (int i = 0; i < battleSlot.Length; ++i)
                {
                    if (battleSlot[i].gameObject == obj.gameObject)
                    {
                        battleSlot[i].sprite = null;
                        heroSelectList[arrayIdx[i]] = false;
                        arrayIdx[i] = -1;

                        break;
                    }
                }

                --battleSlotCount;
                break;
            default:
                break;
        }

        //Debug.Log(battleSlotCount);
    }
    
    public void BattleClick()
    {
        if(BattleUIMng.Ins.message.gameObject.activeSelf == true) { return; }

        if (Player.Ins.playerData.nVigor < BattleMng.Ins.nVigor)
        {
            Debug.Log("행동력이 부족합니다.");
            return;
        }

        BattleMng.Ins.heroIdxList.Clear();

        for (int i = 0; i < arrayIdx.Length; ++i)
        {
            if(arrayIdx[i] != -1)
            {
                BattleMng.Ins.AddHero(arrayIdx[i]);
                BattleMng.Ins.heroIdxList.Add(arrayIdx[i]);
            }
        }        

        if (BattleMng.Ins.heroDatas.Count == 0) { Debug.LogError("Hero가 선택되지 않음"); return; }

        BattleUIMng.Ins.topUI.SetActive(false);
        this.gameObject.SetActive(false);

        BattleUIMng.Ins.battleMap.SetActive(true);

        Player.Ins.MinusVigor(BattleMng.Ins.nVigor);

        //던전 입장
        BattleMng.Ins.BattleEntrance();
    }

    public void Init()
    {
        for(int i=0; i<heroList.Count; ++i)
        {
            heroList[i].raycastTarget = true;
        }
    }

    public void Clear()
    {
        heroList.Clear();
        heroSlotList.Clear();
        battleSlot = null;   
    }


    public void BackScene()
    {
        Clear();
        BattleMng.Ins.Clear();

        SceneManager.LoadScene((int)eScene.WorldMapScene);
    }

    IEnumerator PopupText()
    {
        popup.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        popup.SetActive(false);
    }
}   
