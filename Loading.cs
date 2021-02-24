using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーン偏移時のローディングを実装するクラス
/// </summary>
public class Loading : MonoBehaviour
{
    private AsyncOperation async;

    [SerializeField] GameObject loadUI;

    [SerializeField] Slider slider;

    public void LoadGameScene()
    {
        LoadScene("GameScene");
    }

    /// <summary>
    /// 引数に与えられたシーン名のシーンを読み込む
    /// </summary>
    /// <param name="sceneName">シーン名</param>
    private void LoadScene(string sceneName)
    {
        loadUI.SetActive(true);

        StartCoroutine("LoadData", sceneName);
    }

    IEnumerator LoadData(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);

        while (!async.isDone)
        {
            var progress = Mathf.Clamp01(async.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
    }
}
