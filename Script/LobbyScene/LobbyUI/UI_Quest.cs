using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;

public class UI_Quest : MonoBehaviour
{
    public List<Button>            questTypeBtn;
    public List<Text>              questText;
    public Dictionary<int, string> d_QuestList          = new Dictionary<int, string>();
    public List<Button>            successBtn;
    public Text                    popupText;
    public GameObject              popup;
    public Image                   questRewardBG;
    public Text                    questRewardGemText;
    public QuestReward             questReward          = new QuestReward();    

    Color questRewardBGColor = new Color(0.5f, 0.5f, 0.5f);
    int gem = 0;
    //내가 퀘스트 창에서 클릭한 속성타입
    int nType = 1;

    public void SetVisible( bool active)
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
            Init();            
        }
    }    

    //퀘스트창이 활성화되고 난후에 타입 클릭시
    public void TypeClick(int type)
    {
        nType = type;

        if(nType >= (int)eQuestType.Max)
        {
            Debug.LogError("TypeError");
            return;
        }

        QuestText(type);
    }

    //각 타입에 따른 텍스트와 퀘스트 완료여부 (완료시 색 변경)
    public void QuestText(int questnID)
    {
        d_QuestList.Clear();

        ExtensionMethod.GetQuestTb(questnID);

        int idx = 0;

        if (d_QuestList.Count > questText.Count) { Debug.LogError("logic error"); return; }

        foreach (var val in d_QuestList)
        {
            questText[idx].text = string.Format(val.Value);

            if (Player.Ins.questFlagList[val.Key - 1] == true)
            {
                successBtn[idx].GetComponent<Image>().color = Color.green;
            }
            else
            {
                successBtn[idx].GetComponent<Image>().color = Color.white;
            }

            ++idx;
        }
    }

    void Init()
    {
        nType = (int)eStage.Forest1;

        d_QuestList.Clear();

        ExtensionMethod.GetQuestTb(nType);
 
        int idx = 0;

        if(d_QuestList.Count > questText.Count) { Debug.LogError("logic error"); return; }

        foreach(var val in d_QuestList)
        {
            questText[idx].text = string.Format(val.Value);

            if (Player.Ins.questFlagList[val.Key - 1] == true)
            {
                successBtn[idx].GetComponent<Image>().color = Color.green;
            }
            else
            {
                successBtn[idx].GetComponent<Image>().color = Color.white;
            }

            ++idx;
        }
    }

    //퀘스트창에서 완료버튼 누를시
    public void ClickSuccess(int btnIdx)
    {        
        //btnIdx = 퀘스트 정보마다 한개씩 버튼을 가지고 있고 그 버튼에 대한 인덱스        

        if (successBtn[btnIdx].GetComponent<Image>().color == Color.green)
        {
            //Debug.LogError("이미 완료한 퀘스트 입니다");
            StartCoroutine(PopupText(Define.sQUESTPOPUP_TEXT1));
            return;
        }
        
        eQuestType eType = (eQuestType)nType;

        //완료 버튼 누른게 어느 스테인지 확인
        int idx = 0;        
        eStage eStage = eStage.None;
        //d_QuestList에는 내가 현재 클릭한 속성에 대한 퀘스트정보를 가지고있음
        foreach (var val in d_QuestList)
        {
            if(idx > btnIdx) { break; }

            eStage = (eStage)val.Key;            
            ++idx;
        }
        //현재 퀘스트가 진행중인건지 확인, Player.Ins.questOpenList 인덱스 0부터 시작 eStage는 1부터 시작
        bool bOpen = Player.Ins.questOpenList[(int)eStage - 1];
        if (!bOpen)
        {
            //Debug.LogError("현재 진행중인 퀘스트가 아닙니다");

            StartCoroutine(PopupText(Define.sQUESTPOPUP_TEXT3));
            return;
        }

        //현재 퀘스트 완료중에 해당 속성이 있는지 확인
        bool bType = GameMng.Ins.dli_CurrenQuest.ContainsKey(eType);
        if (!bType)
        {
            //Debug.LogError("해당 속성 퀘스트를 완료하세요");

            StartCoroutine(PopupText(Define.sQUESTPOPUP_TEXT2));
            return;
        }

        //Debug.LogError("퀘스트 완료");

        //questFlagList 인덱스 0부터 시작 eStage는 1부터 시작 = 현재 진행중인 스테이지
        Player.Ins.questFlagList[(int)eStage-1] = true;
        //다음 퀘스트 오픈
        Player.Ins.questOpenList[(int)eStage]   = true;

        successBtn[btnIdx].GetComponent<Image>().color = Color.green;

        //퀘스트 완료후니까 현재 담고있는 퀘스트 삭제
        GameMng.Ins.dli_CurrenQuest.Remove(eType);
        
        //퀘스트 보상창
        questRewardBG.color = questRewardBGColor;
        questRewardBG.gameObject.SetActive(true);

        int gemNum = questReward.ReturnGem((int)eStage);
        
        gem = gemNum;
        
        if(gemNum == -1) { Debug.LogError((int)eStage); }
        
        questRewardGemText.text = string.Format("{0}",gemNum);
        questRewardBG.gameObject.SetActive(true);
    }

    public void QuestReward()
    {
        LobbyUIMng.Ins.message.li_Item.Add(gem);
        questRewardBG.gameObject.SetActive(false);
    }

    IEnumerator PopupText(string sText)
    {
        popupText.text = string.Format(sText);
        popup.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        popup.SetActive(false);
    }
}
