using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Message : MonoBehaviour
{
    //퀘스트 완료 후 확인 버튼 눌렀을 때 
    public List<int>       li_Item = new List<int>();
    //미리 만들어둔 슬롯들 퀘스트 완료후 개수에 맞게 켜주기 위해
    public List<Transform> li_Slot = new List<Transform>();
    public List<Transform> li_Button = new List<Transform>();    

    public Transform slotPrefab;
    public Transform slotParent;

    const int nPLUSSIZE = 10;

    int nSize     = 10;
    int nSlotSize = 0;

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
            CreateSlot();
            Init();
        }
    }

    void CreateSlot()
    {
        for (int i = nSlotSize; i < nSize; ++i)
        {
            Transform tr = Instantiate(slotPrefab);
            tr.parent = slotParent;

            li_Slot.Add(tr);
            li_Button.Add(tr.GetComponent<UI_MessageSlot>().btn);

            Button btn = li_Button[i].GetComponent<Button>();
            btn.onClick.AddListener(() => ClickReward(btn.gameObject));

            tr.gameObject.SetActive(false);
        }
    }

    void Init()
    {
        if(li_Item.Count >= li_Slot.Count)  { nSize += nPLUSSIZE; CreateSlot(); }

        for(int i=0; i<li_Item.Count; ++i)
        {
            li_Slot[i].gameObject.SetActive(true);
        }
    }

    public void ClickReward(GameObject obj)
    {
        int idx = 0;
        for(int i=0; i<li_Button.Count; ++i)
        {
            if(obj == li_Button[i].gameObject)
            {
                idx = i;
                break;
            }
        }
        
        Player.Ins.AddGem(li_Item[idx]);
        li_Item.Remove(li_Item[idx]);
        li_Slot[idx].gameObject.SetActive(false);

        li_Item.Sort();
    }
}
