using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    public void SetVisible(bool active)
    {
        if(gameObject.activeSelf != active)
        {
            gameObject.SetActive(active);
        }

        if(active)
        {
            StartCoroutine(Popup());
        }
    }

    IEnumerator Popup()
    {
        yield return new WaitForSeconds(0.4f);

        SetVisible(false);
    }
}
