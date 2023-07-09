using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontPressTheButton : GameBase
{
   
    [SerializeField]
    Button theButton;

    [SerializeField]
    AudioSource boom;

    [SerializeField] private GameObject boomEffect;
    [SerializeField] private Image timer;

    private bool started = false;
    private bool stopped = false;


    public override void OnStart()
    { 
        StartCoroutine(IEOnTick());

        theButton.onClick.AddListener(delegate{
            if (stopped) return;
           OnWin();
           StopAllCoroutines();
        });

    }

    protected override void Update()
    {
        if (started)
        {
            timer.fillAmount = 1 - Time.time / (startTime + gameDuration);
        }
    }

    
    public override void OnFail()
    {
        stopped = true;
        print("Game Failed");
    }

    public override void OnWin()
    {
        stopped = true;
        print("Game Won");
        boomEffect.SetActive(true);
        boom?.Play();
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
