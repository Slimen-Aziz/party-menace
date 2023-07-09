using System.Collections;
using UnityEngine;

namespace MiniGames.HairPick
{
    public class EyebrowShaver : GameBase
    {
        [SerializeField] private Eyebrow leftEyeBrow;
        [SerializeField] private Eyebrow rightEyeBrow;
        [SerializeField] private Eyebrow middleEyeBrow;
        [SerializeField] private Shaver shaver;
        [SerializeField] private float delay;
        private float _lastOnTime;
        [SerializeField] private GameObject tear;
        private float _elapsedTime;

        protected override void Start()
        {
            tear.SetActive(false);
        }

        public override void OnStart()
        {
            Debug.Log("Start");
            var transform1 = middleEyeBrow.transform;
            var yPoint =  transform1.localPosition.y - transform1.localScale.y * .5f;
            shaver.Init(yPoint);
            leftEyeBrow.Init();
            rightEyeBrow.Init();
            middleEyeBrow.Init();
            StartCoroutine(IEOnTick());
        }

        public void EndGame()
        {
            _elapsedTime = gameDuration + 1;
        }

        public override IEnumerator IEOnTick()
        {
            while (_elapsedTime < gameDuration)
            {
                _elapsedTime += Time.deltaTime;
                var turnChance = Random.value > .25f;
                if (turnChance && Time.time >= _lastOnTime + delay)
                {
                    _lastOnTime = Time.time;
                    if (!shaver.IsOn)
                    {
                        shaver.TurnOn();
                    }
                }
                yield return null;
            }

            Debug.Log("Stop Game");
            shaver.TurnOff();
            var isLeftOrRightGone = leftEyeBrow.IsShaved() || rightEyeBrow.IsShaved();
            var isMiddleGone = middleEyeBrow.IsShaved();
            if (isMiddleGone)
            {
                if(isLeftOrRightGone) OnWin();
                else OnFail();
                yield break;
            }

            OnFail();
        }

        public override void OnFail()
        {
            tear.SetActive(true);
            base.OnFail();
        }
    }
}