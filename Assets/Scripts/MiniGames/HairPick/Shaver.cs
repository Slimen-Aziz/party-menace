using System;
using UnityEngine;

namespace MiniGames.HairPick
{
    public class ElectricRazor : MonoBehaviour
    {
        [SerializeField] private float coverSpeed;
        [SerializeField] private float xRange;
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        [SerializeField] private Transform bladeCover;
        private Transform _transform;
        [SerializeField] private bool _isOn;
        private float _direction = 1;

        private Vector3 _mouseOffset;

        public void Init()
        {
            _transform = transform;
            _isOn = true;
        }

        private void OnMouseDown()
        {
            _mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            transform.localPosition = _mouseOffset + Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            if(!_isOn) return;
            var newPosition = bladeCover.localPosition;
            newPosition.x += _direction * coverSpeed * Time.deltaTime;
            bladeCover.localPosition = newPosition;
            if (newPosition.x >= xRange) _direction = -1;
            else if (newPosition.x < -xRange) _direction = 1;
        }
    }
}
