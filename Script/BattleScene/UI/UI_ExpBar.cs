using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ExpBar : MonoBehaviour
{    
    public Slider slider;

    public void SetExp( int nMaxExp, int nCurrentExp)
    {
        slider.maxValue = nMaxExp;
        slider.minValue = 0;

        slider.value = (float)nCurrentExp / (float)nMaxExp * 100;     
    }

    
    public void ExpUp(int repeatCount, int nMaxExp, int nCurrentExp, int nPlusExp)
    {     
        if(repeatCount == 0)
        {   
            StartCoroutine(ExpBar(nMaxExp, nCurrentExp, nPlusExp));
        }
        else
        {   
            StartCoroutine(ExpBar(nMaxExp, nCurrentExp));
        }       
    }

    IEnumerator ExpBar(int nMaxExp, int nCurrentExp, int nPlusExp)
    {
        int max     = nMaxExp;
        int currnet = nCurrentExp;
        int value   = currnet + max;

        slider.maxValue = max;
        slider.value    = nCurrentExp;

        while(currnet < value)
        {
            currnet += 10;
            slider.value = (float)currnet / (float)max * 100;

            yield return null;
        }
    }

    IEnumerator ExpBar(int nMaxExp, int nCurrentExp)
    {
        int max     = 100;
        int current = 0;

        slider.maxValue = max;

        while (current < max)
        {
            current += 10;

            slider.value = (float)current / (float)max * 100;

            yield return null;
        }

        max     = nMaxExp;
        current = nCurrentExp;
        //Debug.Log(current);

        int min = 0;
        slider.maxValue = max;

        while(min < current)
        {
            min += 10;
            //Debug.Log(min);
            slider.value = (float)min / (float)max * 100;

            yield return null;
        }
    }
}
