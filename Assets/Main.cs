using UnityEngine;
using System.Collections;
using System;

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
                Debug.Log("aaaaaa");

                string[] dps = mainfest.GetAllDependencies("arts");
                AssetBundle[] abs = new AssetBundle[dps.Length];
                for (int i = 0; i < dps.Length; i++)
                {
                    string dUrl = resname + dps[i];
                    WWW dwww = WWW.LoadFromCacheOrDownload(dUrl, mainfest.GetAssetBundleHash(dps[i]));
                    yield return dwww;
                    abs[i] = dwww.assetBundle;
                }
                {
                    WWW awww = WWW.LoadFromCacheOrDownload(resname + "arts", mainfest.GetAssetBundleHash("arts"));
                    yield return awww;
                   UnityEngine.Object assetObj = awww.assetBundle.LoadAsset("New Prefab");
                   awww.assetBundle.Unload(false);
                   UnityEngine.Object obj2 = Instantiate(assetObj);

                }
                for (int i = 0; i < abs.Length; ++i)
                {
                    abs[i].Unload(false);
                }
            }

        } // memory is freed from the web stream (www.Dispose() gets called implicitly)
    }
}
