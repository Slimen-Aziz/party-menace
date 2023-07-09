using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NoDateForYou : GameBase
{
    [SerializeField] Button sendButton, cancelButton;

    [SerializeField] AudioSource yay, wonJingle, click, powerOff;

    [SerializeField] RectTransform overlayTop, overlayBottom;

    [SerializeField] GameObject oldMessages, delayMessages, failMessages, messageField;

    [SerializeField] private Image timer;

    bool ended,started;
    [SerializeField] bool won;


    private void Start()
    {
        StartCoroutine(IEOnTick());
        sendButton.onClick.AddListener(delegate
        {
            click?.Play();
            delayMessages?.SetActive(false);
            oldMessages?.SetActive(false);
            failMessages?.SetActive(true);
            messageField?.SetActive(false);


            StopAllCoroutines();
            OnFail();
        });

        cancelButton.onClick.AddListener(delegate
        {
            click?.Play();
            powerOff?.Play();

            overlayTop.DOScaleY(1, 0.1f).SetEase(Ease.Linear);
            overlayBottom.DOScaleY(1, 0.1f).SetEase(Ease.Linear);

            StopAllCoroutines();
            OnWin();
        });
    }

    void Update()
    {
        if (started)
        {
            timer.fillAmount = 1 - Time.time/(startTime + gameDuration) ;
        }
    }

    public override void OnFail()
    {
        print("Game Failed");
    }

    public override void OnWin()
    {
        print("Game Won");
        StartCoroutine(PlaySounds());

        IEnumerator PlaySounds()
        {
            yay?.Play();

            yield return new WaitForSeconds(2);

            wonJingle?.Play();
        }
    }

    float startTime;

    public override IEnumerator IEOnTick()
    {
        startTime = Time.time;
        started = true;
        while (Time.time < startTime + gameDuration)
        {
            yield return new WaitForSeconds(0.5f);

            //try
            //{
            //    ticks[ticks.Count - 1].enabled = false;
            //    ticks.RemoveAt(ticks.Count - 1);
            //}
            //catch
            //{
            //}

            print("Second");
        }

        yield return null;

        oldMessages?.SetActive(false);
        delayMessages?.SetActive(true);

        OnWin();
    }
}