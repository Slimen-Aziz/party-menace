using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MiniGames.Bowling
{
    public class BowlingBall : MonoBehaviour
    {
        [SerializeField] private float xRange;
        [SerializeField] private float ballSpeed;
        [SerializeField] private float forceMagnitude;
        [SerializeField] private ForceMode2D forceMode;
        [SerializeField] private bool isTricky;
        [SerializeField] private GameObject trickText;
        [SerializeField] private GameObject strikeText;
        private float _direction = 1;

        private BowlingController parent;

        private Rigidbody2D _rb;
        private Transform _ballTransform;

        private float ForceMagnitude => forceMagnitude * Random.value + 2;
        private float _throwMagnitude = 0;

        public void Init(BowlingController mainGame)
        {
            parent = mainGame;
            trickText.SetActive(false);
            strikeText.SetActive(false);
            _rb = GetComponent<Rigidbody2D>();
            _ballTransform = transform;
        }

        public void MoveAround()
        {
            var newPosition = _ballTransform.localPosition + Time.deltaTime * _direction * ballSpeed * Vector3.right;
            _ballTransform.localPosition = newPosition;
            if (newPosition.x > xRange)
                _direction = -1;
            else if (newPosition.x < -xRange) _direction = 1;
        }

        public void ThrowBall()
        {
            if (isTricky)
            {
                var position = _ballTransform.localPosition;
                var newX = Random.Range(-xRange, xRange);
                Debug.Log($"Trick: Move X From:{position.x}, To:{newX}");
                position.x = newX;
                _ballTransform.localPosition = position;
                trickText.transform.position = Camera.main.WorldToScreenPoint(_ballTransform.position);
                trickText.SetActive(true);
            }

            StartCoroutine(_ThrowBall());

            IEnumerator _ThrowBall()
            {
                yield return null;
                _ballTransform.GetComponent<Animator>().SetTrigger("Throw");
                _throwMagnitude = ForceMagnitude;
                _rb.AddForce(Vector2.up * _throwMagnitude, forceMode);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.name != "PinColliders") return;
            Debug.Log("Ball Hit Pin");
            parent.KnockPins();
            // _ballTransform.GetComponent<Animator>().SetTrigger("Strike");
            // _rb.velocity = Vector2.zero;
            // _rb.angularVelocity = 0;
            strikeText.SetActive(true);
            parent.state = BowlingState.Fail;
        }

        private void OnMouseOver()
        {
            if(parent.state != BowlingState.Throw) return;
            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("Ball Is Clicked");
                var xDirection = Random.Range(-1f, 1f);
                var direction = new Vector2(xDirection, -1);
                _ballTransform.up = direction;
                _rb.velocity = Vector2.zero;
                _rb.angularVelocity = 0;
                _rb.AddForce(direction * _throwMagnitude, forceMode);
                parent.state = BowlingState.Success;
            }
        }
    }
}
