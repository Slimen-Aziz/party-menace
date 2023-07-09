using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Unscrew : GameBase
{
    [SerializeField] private Sprite screwedSprite, unscrewedSprite;
    private Canvas canvas => GetComponent<Canvas>();
    [SerializeField] private RectTransform screwBottom, screwTop;
    [SerializeField] private Transform screwdriver;
    [SerializeField] private AudioSource win;
    [SerializeField] private Rigidbody2D frame;
    private bool screwing, unscrewing;
    
    [SerializeField] private Image timer;

    private Image bottomImage, topImage;


    private bool ended = false;
    private bool started = false;

    protected override void Start()
    {
        bottomImage = screwBottom.GetComponent<Image>();
        topImage = screwTop.GetComponent<Image>();
        StartCoroutine(IEOnTick());

    }

    public void Unscrewing(bool state)
    {
        unscrewing = state;
    }

    public void Screwing(bool state)
    {
        screwing = state;
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

        Vector2 movePos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition, canvas.worldCamera,
            out movePos);

        screwdriver.position = canvas.transform.TransformPoint(movePos);

        if (unscrewing)
        {
            screwTop.transform.Rotate(Vector3.forward * (180 * Time.deltaTime));
            screwTop.localScale += Vector3.one * (Time.deltaTime);
        }


        if (screwing)
        {
            screwBottom.transform.Rotate(Vector3.forward * (-180 * Time.deltaTime));
            screwBottom.localScale -= Vector3.one * (Time.deltaTime);
        }

        bottomImage.sprite = screwBottom.localScale.y >= 2 ? screwedSprite : unscrewedSprite;
        topImage.sprite = screwTop.localScale.y >= 2 ? screwedSprite : unscrewedSprite;

        if (screwBottom.localScale.y <= 1)
        {
            screwBottom.localScale = Vector3.one * 1;
            ended = true;
            OnFail();
            StopAllCoroutines();

            return;
        }

        if (screwTop.localScale.y >= 2)
        {
            ended = true;
            Rigidbody2D topRb = screwTop.AddComponent<Rigidbody2D>();
            Rigidbody2D bottomRb = screwBottom.AddComponent<Rigidbody2D>();
            topRb.gravityScale = 100;
            bottomRb.gravityScale = 100;
            frame.gravityScale = 70;
            frame.angularVelocity = 45;
            win?.Play();
            StopAllCoroutines();
            Invoke("DelayedWin",1);
        }
    }

    void DelayedWin()
    {
        OnWin();
    }

    public override void OnFail()
    {
        print("Game Failed");
    }

    public override void OnWin()
    {
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