using System;
using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Analytics;

public class NoDateForYou : GameBase
{
    [SerializeField] Button sendButton, cancelButton;

    [SerializeField] AudioSource yay, wonJingle, click, powerOff;

    [SerializeField] RectTransform overlayTop, overlayBottom;

    [SerializeField] GameObject oldMessages, delayMessages, failMessages, messageField;

    [SerializeField] private Image timer;

    bool ended,started;
    [SerializeField] bool won;


    public override void OnStart()
    {
        var cor = StartCoroutine(IEOnTick());
        sendButton.onClick.AddListener(delegate
        {
            if (ended) return;
            click?.Play();
            delayMessages?.SetActive(false);
            oldMessages?.SetActive(false);
            failMessages?.SetActive(true);
            messageField?.SetActive(false);


            StopCoroutine(cor);
            OnFail();
        });

        cancelButton.onClick.AddListener(delegate
        {
            if (ended) return;
            click?.Play();
            powerOff?.Play();

            overlayTop.DOScaleY(1, 0.1f).SetEase(Ease.Linear);
            overlayBottom.DOScaleY(1, 0.1f).SetEase(Ease.Linear);

            StopCoroutine(cor);
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

    public override void OnWin()
    {
        ended = true;
        print("Game Won");
        StartCoroutine(PlaySounds());

        IEnumerator PlaySounds()
        {
            yay?.Play();

            yield return new WaitForSeconds(2);

            wonJingle?.Play();
            yield return new WaitForSeconds(1);

            base.OnWin();
        }
    }

    public override void OnFail()
    {
        ended = true;
        base.OnFail();
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