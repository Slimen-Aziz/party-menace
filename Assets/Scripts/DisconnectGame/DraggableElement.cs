using UnityEngine;

namespace DisconnectGame
{
    public class DraggableElement : MonoBehaviour
    {
        public bool IsConnected { get; private set; }

        private DraggableHolder _holder;

        private void OnMouseDrag()
        {
            var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = 0;
            transform.position = newPosition;
        }

        private void OnMouseUp()
        {
            if (_holder != null && !IsConnected) 
                _holder.AttachElement(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("DraggableHolder")) return;
            other.TryGetComponent(out _holder);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_holder == null) return;
            _holder.DisconnectElement(this);
            _holder = null;
        }

        public void ResetElement()
        {
            // transform.position = Vector3.zero;
            _holder = null;
            IsConnected = false;
        }
    }
}
