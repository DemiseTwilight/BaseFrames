using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTools : SingletonBase<SceneTools>, IInitialize {

    public void Initialize() {
        Debug.Log("SceneTools is Initialized");
    }
    private const LoadSceneMode LOADSCENE_MODE = LoadSceneMode.Single;
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="mode">模式</param>
    /// <param name="async">异步加载</param>
    public void LoadScene(string sceneName,
        LoadSceneMode mode = LOADSCENE_MODE, bool async = false) {
        if (async) {
            StartCoroutine(LoadSceneAsync(sceneName, mode));
        } else
            SceneManager.LoadScene(sceneName, mode);
    }
    /// <summary>
    /// 加载场景
    /// </summary>
    /// <param name="sceneBuildIndex">场景名</param>
    /// <param name="mode">模式</param>
    /// <param name="async">异步加载</param>
    public void LoadScene(int sceneBuildIndex,
        LoadSceneMode mode = LOADSCENE_MODE, bool async = false) {
        if (async) {
            StartCoroutine(LoadSceneAsync(sceneBuildIndex, mode));
        } else
            SceneManager.LoadScene(sceneBuildIndex, mode);
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(string sceneName,
        LoadSceneMode mode) {
        AsyncOperation asy = SceneManager.LoadSceneAsync(
            sceneName, mode);
        //防止加载完跳出：
        asy.allowSceneActivation = false;
        AsyncOperationBuffer.Add(sceneName, asy);
        while (!asy.isDone) {
            if (asy.progress >= 0.9f) {
                asy.allowSceneActivation = true;
            }
            yield return null;
        }

        //UISystem.Instance.fade.StartFadeInAndOut();

    }
    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadSceneAsync(int sceneBuildIndex,
        LoadSceneMode mode) {
        AsyncOperation asy = SceneManager.LoadSceneAsync(
            sceneBuildIndex, mode);
        //防止加载完跳出：
        asy.allowSceneActivation = false;

        while (asy.progress < 0.9f) {
            //Debug.Log("fadeIsDone:" + fadeIsDone+ "isDone:"+ asy.isDone);
            //Debug.Log("progress:"+asy.progress);
            yield return new WaitForEndOfFrame();
        }
        ///加载完成且淡出特效消失后跳出：
        //Debug.Log("fadeIsDone:" + fadeIsDone + "isDone:" + asy.isDone);
        asy.allowSceneActivation = true;
    }

    /////////////////////////////////////
    ///预加载模式
    //////////////////////////////////////
    /// <summary>
    /// 预加载缓存
    /// </summary>
    public Dictionary<string, AsyncOperation> AsyncOperationBuffer =
        new Dictionary<string, AsyncOperation>();
    /// <summary>
    /// 预加载场景
    /// </summary>
    /// <param name="sceneName">场景名</param>
    /// <param name="mode">模式</param>
    public void PreloadingScene(string sceneName,
        LoadSceneMode mode = LOADSCENE_MODE) {
        StartCoroutine(PreloadingAsync(sceneName, mode));
        //PreloadingAsync(sceneName, mode);
    }

    /// <summary>
    /// 异步预加载场景
    /// </summary>
    /// <returns></returns>
    IEnumerator PreloadingAsync(string sceneName,
        LoadSceneMode mode) {
        AsyncOperation asy = SceneManager.LoadSceneAsync(
            sceneName, mode);

        this.AsyncOperationBuffer.Add(sceneName, asy);
        //防止加载完跳出：
        asy.allowSceneActivation = false;

        while (asy.progress < 0.9f) {
            yield return null;
        }
    }
    /// <summary>
    /// 激活场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void ActivationScene(string sceneName) {
        this.AsyncOperationBuffer[sceneName].allowSceneActivation = true;
        this.AsyncOperationBuffer.Remove(sceneName);
    }
    /// <summary>
    /// 卸载并清理预加载缓存
    /// </summary>
    public void ClearBuffer() {
        foreach (var buffer in this.AsyncOperationBuffer) {
            SceneManager.UnloadSceneAsync(buffer.Key);
        }
        this.AsyncOperationBuffer.Clear();
    }
    /// <summary>
    /// 获得活动场景
    /// </summary>
    /// <returns></returns>
    public Scene GetActiveScene() {
        return SceneManager.GetActiveScene();
    }
}
