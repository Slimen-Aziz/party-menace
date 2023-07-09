using UnityEngine;

namespace Elements
{
    public class Goal : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Football")) return;
            if (!other.TryGetComponent<Football>(out var ball)) return;
            ball.StopBall(true);
        }
    }
}
