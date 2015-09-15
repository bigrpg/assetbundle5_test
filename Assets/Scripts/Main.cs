 using System.Collections.Generic;
 using UnityEngine;
 using UnityEngine.Events;
 using UnityEngine.EventSystems;
using System.Collections;
using System;
using UnityEngine.UI;

public class Main : MonoBehaviour {

    AssetBundle ab;
    string resname;
	// Use this for initialization
	void Start () {
        resname = Application.streamingAssetsPath + "/AssetBundles/";
        if (resname.Contains("://") == false)
            resname = "file://" + resname;
        Debug.Log(Application.streamingAssetsPath);

        StartCoroutine(DownloadAndCache());
	}

    void setTrigers()
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = transform.gameObject.AddComponent<EventTrigger>();

        // 实例化delegates
        trigger.triggers = new List<EventTrigger.Entry>();
        // 定义需要绑定的事件类型。并设置回调函数
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // 设置 事件类型
        entry.eventID = EventTriggerType.PointerClick;
        // 设置回调函数
        entry.callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> callback = new UnityAction<BaseEventData>(OnScriptControll);
        entry.callback.AddListener(callback);
        // 添加事件触发记录到GameObject的事件触发组件
        trigger.triggers.Add(entry);


        var button = transform.gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(TestClick);
        }


    }

    public void TestClick()
    {
        Debug.Log("Test Click. This is Type 4");
    }

    public void OnScriptControll(BaseEventData arg0)
    {
        Debug.Log("Test Click");
    }
	
	// Update is called once per frame
	void Update () {

	
	}

    IEnumerator DownloadAndCache()
    {
        // Wait for the Caching system to be ready
        while (!Caching.ready)
            yield return null;

        // Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
        using (WWW www = new WWW(resname+"AssetBundles"))
        {
            yield return www;
            if (www.error != null)
                throw new Exception("WWW download had an error:" + www.error);
            AssetBundle bundle = www.assetBundle;
            AssetBundleManifest mainfest = bundle.LoadAsset("AssetBundleManifest") as AssetBundleManifest;
            if (mainfest != null)
            {
                string[] dps = mainfest.GetAllDependencies("prefab");
                AssetBundle[] abs = new AssetBundle[dps.Length];
                for (int i = 0; i < dps.Length; i++)
                {
                    string dUrl = resname + dps[i];
                    WWW dwww = WWW.LoadFromCacheOrDownload(dUrl, mainfest.GetAssetBundleHash(dps[i]));
                    yield return dwww;
                    abs[i] = dwww.assetBundle;
                }
                {
                    WWW awww = WWW.LoadFromCacheOrDownload(resname + "prefab", mainfest.GetAssetBundleHash("prefab"));
                    yield return awww;
                    UnityEngine.Object assetObj = awww.assetBundle.LoadAsset("gui1.prefab");
                   awww.assetBundle.Unload(false);
                   UnityEngine.GameObject obj2 = Instantiate(assetObj) as GameObject;

                   obj2.transform.SetParent(GameObject.Find("Canvas").transform, false);
                   //obj2.transform.localPosition = pos;
                   //obj2.transform.localRotation = quart;
                   //obj2.transform.localScale = scale;

                }
                for (int i = 0; i < abs.Length; ++i)
                {
                    abs[i].Unload(false);
                }
            }

        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }
}
