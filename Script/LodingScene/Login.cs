using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using G_Define;

public class Login : MonoBehaviour
{
    #region INSPECTOR

    public InputField     idText;
    public InputField     pwText;
    public Join           join;
    public UI_Popup       popup;

    #endregion

    public void JoinClick()
    {
        this.SetVisible(false);

        join.idText.text = "";
        join.pwText.text = "";

        join.SetVisible(true);

    }

    public void SetVisible(bool active)
    {
        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }

    public void LoginClick()
    {
        //table에 초기데이터로 하나 넣은게 있기 때문에 TableMng.Ins.playerTb.GetTable().Count  == 1이면 회원가입이 안돼있음
        if (TableMng.Ins.playerTb.GetTable().Count > 1)
        {
            JsonMng.Ins.JsonLoad();
        }

        var tb = ExtensionMethod.GetLoginTb(idText.text);        

        if (tb != null)
        {
            //Debug.LogError("아이디 있슴");
            //Debug.LogError(tb.strName);

            if(pwText.text == tb.strPw)
            {   
                //Debug.LogError(pwText.text);
                //Debug.LogError(tb.strPw);

                //Debug.LogError("비번도 같음");
                //Debug.LogError("로그인 성공");

                             
                SetVisible(false);
            }
            else
            {
                popup.SetVisible(true);
                Debug.LogError("비밀번호가 다름");
                return;
            }
        }
        else
        {
            popup.SetVisible(true);
            Debug.LogError("아이디와 패스워드가 존재 하지 않음");
            return;
        }       

        Player.Ins.Init(idText.text, tb.nID);

        SceneManager.LoadScene((int)eScene.LobbyScene);
    }





}
