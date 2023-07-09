using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class NiceMohawk : GameBase
{
    private Canvas canvas => GetComponent<Canvas>();
    [SerializeField] private RectTransform leftHair, midHair, rightHair;

    [SerializeField] private AudioSource buzzing, win;
    [SerializeField] private Transform razor, bro;
    [SerializeField] private GameObject tearsOfTheKingdom;
    private bool shavingLeft, shavingRight, shavingMiddle;
    [SerializeField] private Image timer;

    void Start()
    {
        float broY = bro.transform.position.y;
        bro.transform.position += Vector3.down * 64;
        bro.DOMoveY(broY, 1);
        StartCoroutine(IEOnTick());
    }

    private bool ended = false;
    private bool started = false;

    public void ShavingMiddle(bool state)
    {
        shavingMiddle = state;
    }

    public void ShavingLeft(bool state)
    {
        shavingLeft = state;
    }

    public void ShavingRight(bool state)
    {
        shavingRight = state;
    }


    protected override void Update()
    {
        if (ended)
        {
            if (buzzing.isPlaying)
                buzzing.Stop();
            return;
        }

        if (started)
        {
            timer.fillAmount = 1 - Time.time / (startTime + gameDuration);
        }

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        razor.transform.position = canvas.transform.TransformPoint(movePos);


        if (Input.GetMouseButtonDown(0))
        {
            if (!buzzing.isPlaying)
            {
                buzzing.time = 2;
                buzzing.Play();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (buzzing.isPlaying) buzzing.Stop();
        }

        if (shavingLeft)
        {
            leftHair.transform.localScale = Vector3.MoveTowards(leftHair
                .transform.localScale, new Vector3(1, 0, 1), Time.deltaTime);
        }
        else if (shavingMiddle)
        {
            midHair.transform.localScale = Vector3.MoveTowards(midHair
                .transform.localScale, new Vector3(1, 0, 1), Time.deltaTime);
        }
        else if (shavingRight)
        {
            rightHair.transform.localScale = Vector3.MoveTowards(rightHair
                .transform.localScale, new Vector3(1, 0, 1), Time.deltaTime);
        }


        if (leftHair.localScale.y <= 0 && rightHair.localScale.y <= 0)
        {
            ended = true;
            OnFail();
            StopAllCoroutines();
        }

        if (midHair.localScale.y <= 0)
        {
            ended = true;
            OnWin();
            StopAllCoroutines();
        }
    }

    public override void OnFail()
    {
        print("Game Failed");
    }

    public override void OnWin()
    {
        win?.Play();
        tearsOfTheKingdom?.SetActive(true);
        print("Game Won");
    }

    float startTime;

    public override IEnumerator IEOnTick()
    {
        startTime = Time.time;
        started = true;
        while (Time.time < startTime + gameDuration)
        {
            yield return new WaitForSeconds(1);
        }

        yield return null;
        ended = true;
        OnFail();
    }
}