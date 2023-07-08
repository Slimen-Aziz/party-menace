using System.Collections;
using UnityEngine;

namespace DeleteLater
{
    public class TestGame : GameBase
    {
        [SerializeField] private float _elapsedTime;

        public override void OnStart()
        {
            Debug.Log(gameObject.name);
            StartCoroutine(IEOnTick());
            var item = new GameObject();
            item.transform.parent = transform;
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one * .5f;
            item.AddComponent<TextMesh>().text = gameObject.name;
        }

        public override IEnumerator IEOnTick()
        {
            while (_elapsedTime < gameDuration)
            {
                _elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            if(Random.value > .5) OnWin();
            else OnFail();
        }
    }
}
