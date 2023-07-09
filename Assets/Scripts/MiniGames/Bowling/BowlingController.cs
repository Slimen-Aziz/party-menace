using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MiniGames.Bowling
{
    public enum BowlingState
    {
        Prepare,
        Throw,
        Success,
        Fail,
    }
    
    public class BowlingController : GameBase
    {
        [SerializeField] private List<Transform> pins;
        [SerializeField] private BowlingBall ball;
        [SerializeField] private float throwBallTime;
        [SerializeField] private float _elapsedTime = 0;
        private WaitForEndOfFrame _frame = new WaitForEndOfFrame();
        [SerializeField] private AudioSource strikeSFX;

        public BowlingState state;
        
        [SerializeField] private float rotationOffset;

        [SerializeField] private float xPositionOffset;
        [SerializeField] private float yPositionOffset;
        private float RandomRotation => Random.Range(-rotationOffset, rotationOffset);
        private float RandomXOffset => Random.Range(-xPositionOffset, xPositionOffset);
        private float RandomYOffset => Random.Range(-yPositionOffset, yPositionOffset);

        public override void OnStart()
        {
            ball.Init(this);
            StartCoroutine(IEOnTick());
        }

        public override IEnumerator IEOnTick()
        {
            while (_elapsedTime < gameDuration)
            {
                _elapsedTime += Time.deltaTime;
                MoveAround();
                yield return _frame;
            }

            if (state == BowlingState.Success) OnWin();
            else if (state == BowlingState.Fail) OnFail();
        }
        
        public void EndGame()
        {
            _elapsedTime = gameDuration;
        }

        private void MoveAround()
        {
            if (state != BowlingState.Prepare) return;
            ball.MoveAround();

            if (_elapsedTime >= throwBallTime)
            {
                Debug.Log("Throw Ball!");
                ball.ThrowBall();
                state = BowlingState.Throw;
            }
        }

        public void KnockPins()
        {
            strikeSFX.time = 0.3f;
            strikeSFX.Play();
            for (int index = 0; index < pins.Count; index++)
            {
                var pin = pins[index];
                var newPosition = pin.localPosition;
                newPosition.x += RandomXOffset;
                newPosition.y += RandomYOffset;
                pin.localPosition = newPosition;
                pin.rotation = Quaternion.Euler(0, 0, RandomRotation);
            }
        }
    }
}