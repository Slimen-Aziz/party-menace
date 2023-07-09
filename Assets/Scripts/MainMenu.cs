using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeDuration;
    [SerializeField] private Ease tweenType;
    [SerializeField] private RectTransform healthHolder;
    [SerializeField] private RectTransform[] healthIcons;

    [SerializeField] private Vector2 offScreenPosition;
    [SerializeField] private Vector2 onScreenPosition;

    [SerializeField] private float movementDuration;
    [SerializeField] private float healthBlinkingInterval;
    [SerializeField] private float healthBlinkingAmount;

    [SerializeField] private TextMeshProUGUI hint;
    
    private int _index;

    public void Init()
    {
        startButton.onClick.AddListener(StartGame);
        TransitionOut(null);
        hint.text = "";
    }

    private void StartGame()
    {
        GameController.LoadNextGame();
    }

    public void OpenMenu()
    {
        startButton.gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        startButton.gameObject.SetActive(false);
    }

    public void TransitionOut(Action inCallback = null)
    {
        canvasGroup.DOFade(0, fadeDuration).OnComplete(() =>
        {
            canvasGroup.interactable = true;
            hint.text = "";
            inCallback?.Invoke();
        });
    }

    public void TransitionIn(Action inCallback)
    {
        canvasGroup.interactable = false;
        canvasGroup.DOFade(1, fadeDuration).OnComplete(() =>
        {
            inCallback?.Invoke();
        });
    }

    public void ShowHealth(Action inCallback)
    {
        healthHolder.DOAnchorPos(onScreenPosition, movementDuration).SetEase(tweenType).OnComplete(() =>
        {
            inCallback?.Invoke();
        });
    }

    public void LoseHealth(Action inCallback)
    {
        ShowHealth(() =>
        {
            LoseHealthAnimated(inCallback);
        });
    }

    private void LoseHealthAnimated(Action inCallback)
    {
        StartCoroutine(_LoseHealth());
        
        IEnumerator _LoseHealth()
        {
            var waitForSeconds = new WaitForSeconds(healthBlinkingInterval);
            var counter = 0;
            var status = false;
            
            inCallback?.Invoke();
            while (counter < healthBlinkingAmount)
            {
                healthIcons[_index].gameObject.SetActive(status);
                status = !status;
                counter++;
                yield return waitForSeconds;
            }
            healthIcons[_index].gameObject.SetActive(false);
            
            yield return waitForSeconds;
            _index++;
            if (_index >= healthIcons.Length)
                LoseGame();
            else
                HideHealth(null);
        }
    }

    private void LoseGame()
    {
    }

    public void HideHealth(Action inCallback)
    {
        healthHolder
            .DOAnchorPos(offScreenPosition, movementDuration)
            .SetEase(tweenType)
            .OnComplete(GameController.StartGame);
    }

    public void ShowHint(string inText)
    {
        hint.text = inText;
    }
}
