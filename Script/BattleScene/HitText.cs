using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using G_Define;

public class HitText : MonoBehaviour
{
    public List<Text> li_HitText = new List<Text>();    

    const int nPOOL_SIZE = 30;

    private void Start()
    {
        Pool();        
    }

    public void Init()
    {
        for (int i = 0; i < li_HitText.Count; ++i)
        {
            li_HitText[i].gameObject.SetActive(false);
        }
    }

    public void Pool()
    {
        for(int i=0; i<nPOOL_SIZE; ++i)
        {
            var temp = Instantiate(Resources.Load<Text>(Path.TEXT)) as Text;

            temp.transform.parent = this.transform;

            li_HitText.Add(temp);
            temp.gameObject.SetActive(false);
        }
    }

    public Text GetHitText()
    {
        for (int i=0; i<nPOOL_SIZE; ++i)
        {
            if(li_HitText[i].gameObject.activeSelf == false)
            {
                return li_HitText[i];                
            }
        }

        return null;
    }       
}
