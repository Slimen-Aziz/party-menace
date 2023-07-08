using System.Collections;
using Elements;
using UnityEngine;

namespace DisconnectGame
{
    public class DisconnectGame : GameBase
    {
        [SerializeField] private DraggableElement draggable;
        [SerializeField] private DraggableHolder draggableHolder;
        [SerializeField] private Cable cable;

        private WaitForSeconds _second;

        public override void OnStart()
        {
            draggableHolder.OnConnect += cable.Connect;
            draggableHolder.OnDisconnect += cable.Disconnect;
            draggableHolder.AttachElement(draggable);
            _second = new WaitForSeconds(1);
            StartCoroutine(IEOnTick());
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
