using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using G_Define;

public class TableWWW : MonoBehaviour
{
    //const string TableKey = @"12T0GmockDzib4xcEB0EzO-tjzCfDLlwMbfaOe3aXcJQ";
    const string sGET_URL = @"http://spreadsheets.google.com/a/google.com/tq?key={0}&gid={1}";
    const string sPOST_URL = @"https://docs.google.com/forms/d/e/{0}/formResponse";

    // 메뉴에서 사용
    public static void LocalReq<T>(string a_sTableKey,int a_nTableID, List<object> a_refContainer, System.Action<bool> a_fpCallback)
    {
        ServicePointManager.ServerCertificateValidationCallback = Validator;

        string url = string.Format(sGET_URL, a_sTableKey, a_nTableID);

        byte[] data = new WebClient().DownloadData(url);
        MemoryStream ms = new MemoryStream(data);
        StreamReader reader = new StreamReader(ms);

        // 		WebRequest req = WebRequest.Create(url);
        // 		req.Credentials = CredentialCache.DefaultCredentials;
        // 		HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
        // 		StreamReader reader = new StreamReader(resp.GetResponseStream());

        string d = reader.ReadToEnd();

        Debug.LogError(d);

        bool bResult = SetData<T>(d, a_refContainer);

        if (a_fpCallback != null)
        {
            a_fpCallback(bResult);
        }

        reader.Close();
        Console.ReadKey();
    }

    public static bool Validator(object sender, X509Certificate certificate, X509Chain chain,
                                  SslPolicyErrors sslPolicyErrors)
    {
        return true;
    }

    public void Req<T>(string a_sTableKey, int a_nTableID, List<object> a_refContainer, System.Action<bool> a_fpCallback)
    {
        StartCoroutine(Request<T>(a_sTableKey, a_nTableID, a_refContainer, a_fpCallback));
    }

    IEnumerator Request<T>(string a_sTableKey, int a_nTableID, List<object> a_refContainer, System.Action<bool> a_fpOnResponse)
    {
        bool bResult = true;
        string url = string.Format(sGET_URL, a_sTableKey, a_nTableID);

        WWW www = new WWW(url);

        while (www.isDone == false)
        {
            yield return null;
        }

        if (string.IsNullOrEmpty(www.error) == false)
        {
            if (a_fpOnResponse != null)
            {
                Debug.LogError(string.Format("network error - {0}", www.error));
                a_fpOnResponse(false);
                yield break;
            }
        }

        string d = www.text;

        bResult = SetData<T>(d, a_refContainer);

        if (bResult == false)
        {
            Debug.LogError("key : " + a_sTableKey + "ID : " + a_nTableID);
        }

        if (a_fpOnResponse != null)
        {
            a_fpOnResponse(bResult);
        }
    }

    static bool SetData<T>(string s, List<object> a_refContainer)
    {
        bool bResult = true;

        try
        {
            // 필요없는 문자열 제거
            int nStart = s.IndexOf("(");
            int nEnd = s.IndexOf(");");
            ++nStart;

            string data = s.Substring(nStart, nEnd - nStart);

            // 실제 값 파싱
            List<string> liValueName = new List<string>(); // 변수명
            List<List<string>> liValues = new List<List<string>>(); // 변수값

            LitJson.JsonReader reader = new LitJson.JsonReader(data);
            var mapParsed = LitJson.JsonMapper.ToObject(reader);
            var map = mapParsed["table"];

            var liName = map["cols"];
            var liVal = map["rows"];

            // 임시 캐싱 : 테이블명
            for (int i = 0; i < liName.Count; ++i)
            {
                var m1 = liName[i];
                liValueName.Add((string)m1["label"]);
            }

            // 임시 캐싱 : 각 로우의 값들
            for (int i = 0; i < liVal.Count; ++i)
            {
                var m2 = liVal[i];
                var li = m2["c"];

                liValues.Add(new List<string>());

                for (int j = 0; j < li.Count; ++j)
                {
                    var v = li[j];

                    string value = string.Empty;

                    if (v.ContainsKey("f") == true)
                    {
                        value = (string)v["f"];
                    }
                    else
                    {
                        value = (string)v["v"];
                    }

                    if (value == string.Empty)
                    {
                        Debug.LogError("table error");
                    }

                    liValues[i].Add(value);
                }
            }

            // 캐싱한 값으로부터 클래스 생성
            int nValCount = liValues.Count;

            for (int i = 0; i < nValCount; ++i)
            {
                T val = (T)GetInstance(typeof(T).FullName, liValues[i].ToArray());
                a_refContainer.Add(val);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            Debug.LogError("Table Value error");
            bResult = false;
        }

        return bResult;
    }

    // 이름으로부터 클래스 생성 : 이 클래스에는 필요없어서 주석
    // 	public static object GetInstance(string strFullyQualifiedName)
    // 	{
    // 		Type type = Type.GetType(strFullyQualifiedName);
    // 		if (type != null)
    // 			return Activator.CreateInstance(type);
    // 
    // 		foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
    // 		{
    // 			type = asm.GetType(strFullyQualifiedName);
    // 			if (type != null)
    // 				return Activator.CreateInstance(type);
    // 		}
    // 
    // 		return null;
    // 	}

    // 이름, 인자의 string배열로 클래스 생성
    public static object GetInstance(string strFullyQualifiedName, string[] arrArg)
    {
        Type type = Type.GetType(strFullyQualifiedName);
        if (type != null)
            return Activator.CreateInstance(type, arrArg);

        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            type = asm.GetType(strFullyQualifiedName);
            if (type != null)
                return Activator.CreateInstance(type, arrArg);
        }

        return null;
    }

    public void PostDB(string a_sFormKey, Type a_Type, string[] a_arrSave)
    {
        StartCoroutine(Post(a_sFormKey, a_Type, a_arrSave));
    }
    public void PostPlayerDB(string a_sFormKey, Type a_Type, string[] a_arrSave)
    {
        StartCoroutine(PostPlayer(a_sFormKey, a_Type, a_arrSave));
    }

    IEnumerator PostPlayer(string a_sFormKey, Type a_Type, string[] a_arrSave)
    {
        var li = a_Type.GetFieldDesc();

        WWWForm form = new WWWForm();
        for (int i = 0; i < li.Count; ++i)
        {
            form.AddField(li[i], a_arrSave[i]);
        }

        byte[] rawData = form.data;
        string sURL = string.Format(sPOST_URL, a_sFormKey);

        yield return new WWW(sURL, rawData);
    }

    IEnumerator Post(string a_sFormKey, Type a_Type, string[] a_arrSave)
    {
        var li = a_Type.GetFieldDesc();

        WWWForm form = new WWWForm();
        for(int i=0; i<li.Count; ++i)
        {
            form.AddField(li[i], a_arrSave[i]);
        }

        byte[] rawData = form.data;
        string sURL = string.Format(sPOST_URL, a_sFormKey);

        yield return new WWW(sURL, rawData);
    }
}
