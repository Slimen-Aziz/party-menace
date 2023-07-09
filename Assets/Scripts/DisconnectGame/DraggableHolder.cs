using System;
using UnityEngine;

namespace DisconnectGame
{
    public class DraggableHolder : MonoBehaviour
    {
        public event Action OnConnect;
        public event Action OnDisconnect;
    
        public bool IsConnected { get; private set; }
    
        private DraggableElement _attachedElement;

        public void AttachElement(DraggableElement inElement)
        {
            if (IsConnected && _attachedElement != null) 
                _attachedElement.ResetElement();
        
            _attachedElement = inElement;
            _attachedElement.transform.localPosition = transform.position;
            IsConnected = true;
        
            OnConnect?.Invoke();
        }

        public void DisconnectElement(DraggableElement inElement)
        {
            if (IsConnected && _attachedElement != inElement) return;
            _attachedElement = null;
            IsConnected = false;
            OnDisconnect?.Invoke();
        }
    }
}
