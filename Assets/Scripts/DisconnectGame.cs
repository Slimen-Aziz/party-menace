using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisconnectGame : GameBase
{
    [SerializeField] private DraggableElement element;
    [SerializeField] private DraggableHolder elementHolder;
    [SerializeField] private Cable cable;
    
    protected override void Start()
    {
        elementHolder.OnConnect += cable.DisableJoints;
        elementHolder.OnDisconnect += cable.EnableJoints;
        if (element == null) return;
        elementHolder.AttachElement(element);
    }

    protected override void Update()
    {
        
    }
}
