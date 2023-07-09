using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RhythmicMenace : GameBase
{

    [SerializeField]
    List<Sprite> arrows;

    [SerializeField]
    List<RectTransform> spawners;

    [SerializeField]
    List<Image> ticks;

    [SerializeField]
    GameObject screenOverlay;

    [SerializeField]
    float delay = 2;

    bool ended;
    bool won;


    protected override void Start()
    {
        StartCoroutine(IEOnTick());
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void OnStart()
    {
        base.OnStart();
    }

    public override void OnFail()
    {
        print("Game Failed");
    }

    public override void OnWin()
    {
        print("Game Won");

    }

    public void DisableVolume()
    {
        ended = true;
        won = true;
    }

    IEnumerator SpawnNotes()
    {

        float startTime = Time.time;
        while (!ended && Time.time < startTime + gameDuration * 0.25f + delay)
        {

            yield return new WaitForSeconds(0.1f);

            int spawner = Random.Range(0, 3);
            int sprite = Random.Range(0, 4);

            GameObject arrow = new GameObject();
            arrow.transform.parent = spawners[spawner].transform;
            arrow.name = "Arrow - " + Time.time;

            arrow.AddComponent<CanvasRenderer>();
            var img = arrow.AddComponent<Image>();
            arrow.transform.position = spawners[spawner].transform.position;
            img.sprite = arrows[sprite];

            var rt = arrow.GetComponent<RectTransform>();
            rt.sizeDelta = Vector2.one * 32;

            var rb2d = arrow.AddComponent<Rigidbody2D>();
            rb2d.gravityScale = 0;
            rb2d.velocity = Vector2.down * 300;

            Destroy(arrow, 2);
        }

    }

    public override IEnumerator IEOnTick()
    {

        float startTime = Time.time;
        StartCoroutine(SpawnNotes());
        yield return new WaitForSeconds(delay);


        while (!ended && Time.time < startTime + gameDuration * 0.5f + 3)
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

            print("Second");
        }
        yield return null;
        ended = true;

        if (won)
            OnFail();
        else
            OnWin();


    }

}
