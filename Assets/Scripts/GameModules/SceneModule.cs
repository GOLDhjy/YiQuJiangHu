using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Modules
{
    /// <summary>
    /// 加载场景模块，包括在AB包加载和build-in加载
    /// </summary>
    public class SceneModule : Singleton<SceneModule>
    {
        /// <summary>
        /// 从AssetBundle包里面加载
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        public void LoadSceneAtAB(string name , LoadSceneMode mode = LoadSceneMode.Single)
        {
            AssetBundleModule.Instance.UnloadAllWithFalse();

            AssetBundle bundle = AssetBundleModule.Instance.LoadFormFile("scenes.unity3d");
            if (bundle == null)
            {
                Debug.LogError("加载AB包失败，不能加载场景");
            }
            SceneManager.LoadScene(name, mode);
            //bundle.Unload(false);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            //bundle.Unload(false);
            AssetBundleModule.Instance.UnloadAllWithFalse();
        }
        /// <summary>
        /// 此函数返回异步加载场景的异步操作
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public AsyncOperation LoadSceneAtABAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            AssetBundleModule.Instance.UnloadAllWithFalse();
            AssetBundle bundle = AssetBundleModule.Instance.LoadFormFile("scenes.unity3d");
            AsyncOperation AO = SceneManager.LoadSceneAsync(name, mode);
            //bundle.Unload(false);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            //AssetBundleModule.Instance.UnloadAllWithFalse();
            return AO;
        }

        public void LoadScene(int index, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(index,mode);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            //AssetBundleModule.Instance.UnloadAllWithFalse();
        }
        public void LoadScene(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            SceneManager.LoadScene(name, mode);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            AssetBundleModule.Instance.UnloadAllWithFalse();
        }

        public AsyncOperation LoadSceneAsync(int index, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Resources.UnloadUnusedAssets();
            AsyncOperation AO = SceneManager.LoadSceneAsync(index, mode);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            return AO;
        }
        public AsyncOperation LoadSceneAsync(string name, LoadSceneMode mode = LoadSceneMode.Single)
        {
            Resources.UnloadUnusedAssets();
            AsyncOperation AO = SceneManager.LoadSceneAsync(name, mode);
            if (mode == LoadSceneMode.Additive)
            {
                Resources.UnloadUnusedAssets();
            }
            return AO;

        }
        public void UnloadScene(int index, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(index, options);
        }
        public void UnloadScene(Scene s, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(s, options);
        }
        public void UnloadScene(string name, UnloadSceneOptions options = UnloadSceneOptions.None)
        {
            SceneManager.UnloadSceneAsync(name, options);
        }
    }
}
