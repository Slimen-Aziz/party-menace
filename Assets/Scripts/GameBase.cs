using System.Collections;
using UnityEngine;

interface IGameBase
{
    void OnStart(); 
    void OnWin(); 
    void OnFail();
    IEnumerator IEOnTick();
}

public class GameBase : MonoBehaviour, IGameBase
{
    [SerializeField]
    protected float gameDuration;


    public virtual IEnumerator IEOnTick()
    {
        
        yield return null;
        throw new System.NotImplementedException();
    }

    public virtual void OnFail()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnStart()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnWin()
    {
        throw new System.NotImplementedException();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }


}
