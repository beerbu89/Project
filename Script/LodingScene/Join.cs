using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;

//회원가입 
public class Join : MonoBehaviour
{
    #region INSPECTOR

    public Login      login;
    public InputField idText;
    public InputField pwText;
    public UI_Popup   popup;
    #endregion

    public void SetVisible(bool active)
    {
        if (gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }
    }

    //취소
    public void Cancel()
    {
        login.SetVisible(true);

        login.idText.text = "";
        login.pwText.text = "";

        this.SetVisible(false);
    }

    //회원가입
    public void ContinueClick()
    {
        var tb = ExtensionMethod.GetLoginTb(idText.text);

        //ID가 있다
        if(tb != null)
        {
            popup.SetVisible(true);
            Debug.LogError("아이디 중복");
            return;
        }

        LoginData  data       = new LoginData();
        PlayerData playerData = new PlayerData();

        //공백
        if(idText.text == "" || pwText.text =="")
        {
            popup.SetVisible(true);
            Debug.LogError("ID와PW를 입력");
            return;
        }
        else
        {
            int nID = TableMng.Ins.LoginIdx;

            data.nID     = nID;
            data.strName = idText.text;
            data.strPw   = pwText.text;

            TableDownloader1.Ins.m_TbWWW.PostDB(eTableType.GoogleForm.ToDesc(), typeof(LoginData), data.PostData());

            Debug.LogError("회원가입 성공");

            SetVisible(false);

            login.idText.text = "";
            login.pwText.text = "";

            login.SetVisible(true);
            
        }
    }
}
