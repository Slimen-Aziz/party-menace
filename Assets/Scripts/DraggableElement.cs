using UnityEngine;

public class DraggableElement : MonoBehaviour
{
    public bool IsConnected { get; private set; }
    
    [SerializeField] private Camera mainCamera;

    private DraggableHolder _holder;
    
    private void OnMouseDrag()
    {
        var newPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPosition.z = 0;
        transform.position = newPosition;
    }

    private void OnMouseUp()
    {
        if (_holder != null && !IsConnected) _holder.AttachElement(this);
        // otherwise we can reset object to original position.
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
        // reset position as well
        transform.position = Vector3.one * 5;
        // transform.parent = null;
        _holder = null;
        IsConnected = false;
    }
}
