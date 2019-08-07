using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loding : MonoBehaviour
{
    #region INSPECTOR

    public Slider     tableLodingBar;
    public Slider     lodingBar;
    public Text       lodingText;
    public GameObject loginBG;

    bool  lodingFlag = false;
    bool  tableFlag  = false;
    float stopValue  = 1.0f;
    #endregion

    private void Start()
    {
        tableLodingBar.value = 0.0f;
        lodingBar.value      = 0.0f;
        
        TableLoad();
    }

    private void Update()
    {
        tableLodingBar.value = TableDownloader1.Ins.fPercent;

        if (tableLodingBar.value >= (1f - float.Epsilon) && tableFlag == false)
        {   
            tableFlag = true;
        }

        if (lodingFlag == true && tableFlag == true)
        {
            StartCoroutine(LoginBG());
        }
    }

    void TableLoad()
    {   
        TableDownloader1.Ins.Download();        
        StartCoroutine(SceneLoding());
        StartCoroutine(LodingText());
    }

    IEnumerator SceneLoding()
    {
        float startTime   = 0.0f;
        float endTime     = stopValue;
        float currentTime = 0.0f;

        while(startTime < endTime)
        {
            startTime       += Time.deltaTime;
            currentTime     =  startTime / endTime;
            lodingBar.value =  currentTime;

            if(lodingBar.value >= 0.9f)
            {
                lodingFlag = true;
            }

            yield return null;
        }

        lodingBar.value = stopValue;        
    }

    IEnumerator LoginBG()
    {
        yield return new WaitForSeconds(1.0f);
        this.gameObject.SetActive(false);
        loginBG.SetActive(true);

        tableLodingBar.value = 0.0f;
        lodingBar.value      = 0.0f;
    }

    IEnumerator LodingText()
    {
        string[] arrDot = new string[] { "", ".", "..", "..." };
        int nIndex = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.4f);

            lodingText.text = string.Format("loading{0}", arrDot[nIndex]);

            ++nIndex;
            if (nIndex >= arrDot.Length)
            {
                nIndex = 0;
            }

            if(tableFlag == true)
            {
                break;
            }

        }
    }
}
