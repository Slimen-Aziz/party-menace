using System;
using UnityEngine;

namespace MiniGames.HairPick
{
    public class Shaver : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer powerButton;
        [SerializeField] private Sprite onSprite;
        [SerializeField] private Sprite offSprite;

        [SerializeField] private float movementSpeed;

        [SerializeField] private bool _isOn;
        private float _xDirection = 1;
        private Rigidbody2D _rb;

        public bool IsOn => _isOn;

        private Vector3 _mouseOffset;
        [SerializeField] private Vector2 _startPosition;
        [SerializeField] private float _targetYPosition;
        [SerializeField] private float sizeOffset;
        private bool _isDragging;
        private AudioSource _audioSource;

        public void Init(float targetPosition)
        {
            _audioSource = GetComponent<AudioSource>();
            _rb = GetComponent<Rigidbody2D>();
            TurnOff();
            _startPosition = _rb.position;
            _targetYPosition = targetPosition + sizeOffset;
        }

        private void OnMouseDown()
        {
            if(!IsOn) return;
            _isDragging = true;
            _mouseOffset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void OnMouseDrag()
        {
            if(!IsOn) return;
            if(!_isDragging) return;
            _rb.position = _mouseOffset + Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Update()
        {
            if(_rb == null) return;
            if (!_isOn)
            {
                PullBackUp();
                return;
            }

            ShaveDown();
        }

        private void PullBackUp()
        {
            var newPosition = _rb.position;
            if (newPosition.y >= _startPosition.y) return;
            newPosition.y += Time.deltaTime * movementSpeed;
            newPosition = ResetXPosition(newPosition);
            _rb.position = newPosition;
        }

        private void ShaveDown()
        {
            var newPosition = _rb.position;
            if (newPosition.y <= _targetYPosition) return;
            newPosition.y -= movementSpeed * Time.deltaTime;
            newPosition = ResetXPosition(newPosition);
            _rb.position = newPosition;
        }

        private Vector2 ResetXPosition(Vector2 newPosition)
        {
            if (Math.Abs(newPosition.x - _startPosition.x) > 0.2)
            {
                if(newPosition.x > _startPosition.x)
                    newPosition.x -= Time.deltaTime * movementSpeed;
                else 
                    newPosition.x += Time.deltaTime * movementSpeed;
            }

            return newPosition;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out Eyebrow brow))
            {
                brow.ShaveHair();
                if(brow.TurnOff) TurnOff();
            }
        }

        public void TurnOn()
        {
            Debug.Log("Turn On!");
            _isOn = true;
            powerButton.sprite = onSprite;
            _audioSource.time = 0;
            _audioSource.Play();
        }

        public void TurnOff()
        {
            Debug.Log("Turn Off!");
            _audioSource.Stop();
            _isOn = false;
            powerButton.sprite = offSprite;
            _isDragging = false;
        }
    }
}