using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class CreateAssetBundles
{
    [MenuItem("Assets/Build AssetBundles")]
    static void BuildAllAssetBundles()
    {
        BuildPipeline.BuildAssetBundles("AssetBundles", BuildAssetBundleOptions.None, BuildTarget.Android);
    }


    [MenuItem("Assets/Auto Set AssetBundleName")]
    static void AutoSetAssetBundleName()
    {
        Object[] selection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        List<string> alist = new List<string>();
        foreach (Object one in selection)
        {
            string path = AssetDatabase.GetAssetPath(one);
            alist.Add(path);
        }

        string[] abPaths = alist.ToArray() as string[];
        string[] dps = AssetDatabase.GetDependencies(abPaths);
        for (int i = 0; i < dps.Length; ++i)
        {
            if (alist.IndexOf(dps[i]) >= 0)
                continue;

            AssetImporter ai = AssetImporter.GetAtPath(dps[i]);
            if (ai.assetBundleName != null)
            {
                string abName = AssetDatabase.AssetPathToGUID(dps[i]);
                UnityEngine.Debug.Log(string.Format("No.{0} obj's id is:{1}", i + 1, abName));
                ai.assetBundleName = abName;
            }
        }
    }
}