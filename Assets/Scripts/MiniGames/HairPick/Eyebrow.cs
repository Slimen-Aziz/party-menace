using System;
using System.Collections.Generic;
using MiniGames.Bowling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames.HairPick
{
    public class Eyebrow:MonoBehaviour
    {
        [SerializeField] private Sprite longHair;
        [SerializeField] private Sprite cutHair;
        [SerializeField] private int hairAmount;
        [SerializeField] private Vector3 size;
        [SerializeField] private float xSpread;
        [SerializeField] private float ySpread;
        [SerializeField] private float rotation;
        [SerializeField] private bool turnOff;
        [SerializeField] private int shaveAmount;
        
        public bool TurnOff => turnOff;
        private List<Transform> _hairs;
        private int _index;

        public void Init()
        {
            _hairs = new List<Transform>();
            for (var i = 0; i < hairAmount; i++)
            {
                var item = new GameObject().GetComponent<Transform>();
                item.gameObject.AddComponent<SpriteRenderer>().sprite = longHair;
                _hairs.Add(item);
                item.parent = transform;
                item.localScale = size;
                item.localPosition = new Vector3(Random.Range(-xSpread, xSpread), Random.Range(-ySpread, ySpread), 0);
                item.rotation = Quaternion.Euler(0, 0, Random.Range(-1f, 1f) * rotation);
            }

            _index = 0;
        }

        public void ShaveHair()
        {
            for (int i = 0; i < shaveAmount; i++)
            {
                _index++;
                if(_index >= hairAmount) break;
                Debug.Log("Shave");
                _hairs[_index].GetComponent<SpriteRenderer>().sprite = cutHair;
            }
            
            if(turnOff) return;
            if (_index >= hairAmount) (GameController.Instance.CurrentGame as EyebrowShaver)?.EndGame();
        }


        public bool IsShaved()
        {
            return _index >= hairAmount / 2;
        }
    }
}