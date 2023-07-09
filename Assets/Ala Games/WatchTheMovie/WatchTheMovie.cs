using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class WatchTheMovie : GameBase
{
   
    [SerializeField]
    Button eject;

    [SerializeField] private Transform cassette;

    [SerializeField] private RawImage movie;

    [SerializeField] private AudioSource switchOff;
    [SerializeField] private Image timer;
    private bool ended;
    private bool started = false;

   public override void OnStart(){
        StartCoroutine(IEOnTick());

        eject.onClick.AddListener(delegate{
            if(ended) return;
           OnWin();
           StopAllCoroutines();
        });
    }
    
    protected override void Update()
    {
        if (ended)
        {
            return;
        }
        if (started)
        {
            timer.fillAmount = 1 - Time.time / (startTime + gameDuration);
        }
    }


    public override void OnFail()
    {
        ended = true;
        base.OnFail();
        print("Game Failed");
    }

    public override void OnWin()
    {
        ended = true;
        movie.enabled = false;
        cassette.transform.DOMoveY(cassette.transform.position.y - 20, 1);
        print("Game Won");
        switchOff?.Play();


        StartCoroutine(DelayedCall());

        IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(1);
            base.OnWin();
        }
    }

    float startTime;

    public override IEnumerator IEOnTick()
    {
        startTime = Time.time;
        started = true;

        while (Time.time < startTime + gameDuration )
        {
            yield return new WaitForSeconds(1);
        }
        yield return null;

        OnFail();
    }
}
