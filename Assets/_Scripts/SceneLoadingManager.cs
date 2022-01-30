using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoadingManager : MonoBehaviour
{
    [SerializeField] CanvasGroup fader;
    void Awake()
    {
        fader.alpha = 0;
        DontDestroyOnLoad(gameObject);
    }

    public void OnPlayButton()
    {
        OnLoadScene(1);
    }

    public void OnContinueButton()
    {
        OnLoadScene(2);
    }
    public void OnRestartButton()
    {
        OnLoadScene(2);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    public void OnLoadScene(int sceneIndex)
    {
        fader.alpha = 0;
        DOTween
            .To(() => fader.alpha, a => fader.alpha = a, 1, 1.5f)
            .OnComplete(() => OnRevealScene(sceneIndex));
    }

    public void OnRevealScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        DOTween
            .To(() => fader.alpha, a => fader.alpha = a, 0, 1.5f);
    }
}
