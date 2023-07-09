using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Elements
{
    public class Football : MonoBehaviour
    {
        public event Action OnReachGoal;
            
        [SerializeField] private Rigidbody2D ballRb;
        [SerializeField] private float forceFactor = 100;
        [SerializeField] private float forceMagnitude = 9f;
        
        public bool InGoal { get; private set; }

        public void ThrowBall()
        {
            var throwMagnitude = forceFactor * forceMagnitude;
            ballRb.AddForce(new Vector2(Random.Range(-.25f, .25f), 1) * throwMagnitude, ForceMode2D.Force);
        }

        public void StopBall(bool isInGoal = false)
        {
            ballRb.velocity = Vector2.zero;
            ballRb.angularVelocity = 0f;
            InGoal = isInGoal;
            if (isInGoal) OnReachGoal?.Invoke();
        }
    }
}
