using System.Collections;
using DG.Tweening;
using Elements;
using UnityEngine;

namespace DisconnectGame
{
    public class DisconnectGame : GameBase
    {
        [SerializeField] private DraggableElement draggable;
        [SerializeField] private DraggableHolder draggableHolder;
        [SerializeField] private Cable cable;
        [SerializeField] private Canvas gameCanvas;
        [SerializeField] private CanvasGroup gameView;
        [SerializeField] private CanvasGroup staticView;
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip staticSound;
        [SerializeField] private AudioClip gameSound;

        private WaitForSeconds _second;
        private Coroutine _routine;

        public override void OnStart()
        {
            source.clip = gameSound;
            source.Play();
            gameView.alpha = 1f;
            staticView.alpha = 0f;
            gameCanvas.worldCamera = Camera.main;
            draggableHolder.OnConnect += cable.Connect;
            draggableHolder.OnDisconnect += DisconnectCable;
            draggableHolder.AttachElement(draggable);
            _second = new WaitForSeconds(1);
            _routine = StartCoroutine(IEOnTick());
        }

        private void DisconnectCable()
        {
            source.clip = staticSound;
            source.Play();
            StopCoroutine(_routine);
            cable.Disconnect();
            gameView.DOFade(0f, 1f);
            staticView.DOFade(1f, 1f)
                .OnComplete(() =>
                {
                    Invoke(nameof(OnWin), 1f);
                });
        }
        
        public override IEnumerator IEOnTick()
        {
            var startTime = Time.time;
            while (Time.time < startTime + gameDuration)
            {
                yield return _second;
            }

            yield return null;
            
            if (draggable.IsConnected) OnFail();
            else OnWin();
        }
    }
}
