using System;
using System.Collections.Generic;
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

        private List<Transform> _hairs;

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
        }

        private void CreateHair()
        {
        }

        private void Update()
        {
            return;
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (_hairs != null)
                {
                    foreach (var hair in _hairs)
                    {
                        Destroy(hair.gameObject);
                    }
                }

                _hairs = new List<Transform>();
                Init();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                foreach (var hair in _hairs)
                {
                    hair.GetComponent<SpriteRenderer>().sprite = cutHair;
                }
            }
        }
    }
}