using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_TouchText : MonoBehaviour
{
    #region INSPECTOR

    public Animator touchText;
    public Login    login;

    bool touchFlag = true;

    #endregion

    IEnumerator Start()
    {
        if (touchFlag)
        {
            touchText.SetBool("touchFlag", touchFlag);
        }

        yield return null;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touchFlag = false;
            touchText.SetBool("touchFlag", touchFlag);

            login.SetVisible(true);
        }
    }

}
