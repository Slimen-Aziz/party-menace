using UnityEngine;

public class DisconnectGame : GameBase
{
    [SerializeField] private DraggableElement element;
    [SerializeField] private DraggableHolder elementHolder;
    [SerializeField] private Cable cable;

    public override void OnStart()
    {
        elementHolder.OnConnect += cable.DisableJoints;
        elementHolder.OnDisconnect += cable.EnableJoints;
        if (element == null) return;
        elementHolder.AttachElement(element);
    }
}
