using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMng : MonoBehaviour
{
    #region SINGLETON
    static SceneMng _instance = null;

    public static SceneMng Ins
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(SceneMng)) as SceneMng;
                if (_instance == null)
                {
                    _instance = new GameObject("SceneMng", typeof(SceneMng)).GetComponent<SceneMng>();
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
    #endregion

    public string sMonsterName = "";
    public int    nVigor       = 0;

    public GameObject prevObj = null;
    public GameObject nowObj  = null;

    public void Init()
    {
        prevObj = null;
        nowObj = null;
    }

    public bool PopupSetActive(GameObject obj)
    {
        if (prevObj == null)
        {
            prevObj = obj;
            nowObj = obj;
        }
        else
        {
            nowObj = obj;
        }

        if (prevObj != null && prevObj == nowObj)
        {
            return true;
        }

        if (prevObj != null && prevObj != nowObj)
        {
            if (prevObj.activeSelf == true)
            {
                return false;
            }
            else
            {
                prevObj = nowObj;
                return true;
            }
        }
        return false;
    }
}
