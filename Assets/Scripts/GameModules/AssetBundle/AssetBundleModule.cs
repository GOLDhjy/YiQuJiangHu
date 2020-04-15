using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;

namespace Modules
{
    public class AssetBundleModule : Singleton<AssetBundleModule>
    {
        public AssetBundle LoadFormFile(string AssetBundleName)
        {
            AssetBundle AB;
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "StreamingAssets"));
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies(AssetBundleName);
            foreach (string dependency in dependencies)
            {
                AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, dependency));
            }

            AB = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath , AssetBundleName));

#elif UNITY_ANDROID
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets", "StreamingAssets"));
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies(AssetBundleName);
            foreach (string dependency in dependencies)
            {
                AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "!assets", dependency));
            }

            AB = AssetBundle.LoadFromFile(Application.dataPath + "!assets/" + AssetBundleName);
#elif UNITY_IOS
            AssetBundle assetBundle = AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/Raw", "StreamingAssets"));
            AssetBundleManifest manifest = assetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            string[] dependencies = manifest.GetAllDependencies(AssetBundleName);
            foreach (string dependency in dependencies)
            {
                AssetBundle.LoadFromFile(Path.Combine(Application.dataPath + "/Raw", dependency));
            }

            AB = AssetBundle.LoadFromFile(Application.dataPath + "/Raw/"+AssetBundleName);
#endif

            return AB;
        }

        public void UnloadAllWithFalse()
        {
            Debug.Log("Unload AssetBundle");
            AssetBundle.UnloadAllAssetBundles(false);
        }
        public void UnloadAllWithTrue()
        {
            AssetBundle.UnloadAllAssetBundles(true);
        }
    }
}
