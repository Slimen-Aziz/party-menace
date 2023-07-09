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

    
    void Start(){
        StartCoroutine(IEOnTick());

        theButton.onClick.AddListener(delegate{
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
        print("Game Failed");
    }

    public override void OnWin()
    {
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
