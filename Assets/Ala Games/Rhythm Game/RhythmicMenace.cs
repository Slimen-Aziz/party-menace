using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RhythmicMenace : GameBase
{
    [SerializeField] List<Sprite> arrows;
    [SerializeField] List<KeyCode> arrowsKeys;

    [SerializeField] private Image note;

    [SerializeField] List<Image> ticks;

    [SerializeField] float delay = 2;
    [SerializeField] private Image timer;

    bool ended;
    [SerializeField]bool won;

    private int arrow;
    private bool started = false;
    private bool stopped = false;

    public override void OnStart()
    {
        StartCoroutine(IEOnTick());

        arrow = Random.Range(0, 4);
        note.sprite = arrows[arrow];
    }

    void Update()
    {
        if (stopped) return;
        if (started)
        {
            timer.fillAmount = 1 - Time.time / (startTime + gameDuration);
        }
        for (KeyCode key = KeyCode.None; key < KeyCode.Joystick8Button19; key++)
        {
            if (Input.GetKeyDown(key) && key == arrowsKeys[arrow])
            {
                ended = true;
                won = false;
                OnFail();
                StopAllCoroutines();
                break;
            }
            else  if (Input.GetKeyDown(key) && key != arrowsKeys[arrow])
            {
                ended = true;
                won = true;
                OnWin();
                StopAllCoroutines();
                break;
            }
        }
        
    }

    public override void OnFail()
    {
        stopped = true;
        print("Game Failed");
        base.OnFail();
    }

    public override void OnWin()
    {
        stopped = true;
        var rb = note.AddComponent<Rigidbody2D>();
        note.color = Color.red;
        if (rb == null) return;
        rb.gravityScale = 70;
        rb.angularVelocity = 45;
        print("Game Won");


        StartCoroutine(DelayedCall());

        IEnumerator DelayedCall()
        {
            yield return new WaitForSeconds(1);
            base.OnWin();
        }
    }

    public void DisableVolume()
    {
        ended = true;
        won = true;
    }


    float startTime;

    public override IEnumerator IEOnTick()
    {
        startTime = Time.time;
        started = true;
        yield return new WaitForSeconds(delay);


        while (!ended && Time.time < startTime + gameDuration)
        {
            yield return new WaitForSeconds(0.5f);

            try
            {
                ticks[ticks.Count - 1].enabled = false;
                ticks.RemoveAt(ticks.Count - 1);
            }
            catch
            {
            }

          
            
           

            yield return null;
        }

        yield return null;
        ended = true;

        if (won)
            OnFail();
        else
            OnWin();
    }
}