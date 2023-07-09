using System.Collections;
using Elements;
using UnityEngine;

namespace DisconnectGame
{
    public class MoveTheGoal : GameBase
    {
        [SerializeField] private Football ball;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip kickEffect;
        
        private WaitForSeconds _second;
        private Coroutine _routine;
    
        public override void OnStart()
        {
            ball.OnReachGoal += ReachedGoal;
            _second = new WaitForSeconds(1);
            _routine = StartCoroutine(IEOnTick());
            Invoke(nameof(MoveBall), 1f);
        }

        private void MoveBall()
        {
            source.clip = kickEffect;
            source.Play();
            ball.ThrowBall();
        }

        private void ReachedGoal()
        {
            StopCoroutine(_routine);
            OnFail();
        }
        
        public override IEnumerator IEOnTick()
        {
            var startTime = Time.time;
            while (Time.time < startTime + gameDuration)
            {
                yield return _second;
            }

            yield return null;
            
            if (ball.InGoal) OnFail();
            else OnWin();
        }
        
    }
}
