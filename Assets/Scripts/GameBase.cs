using System.Collections;
using UnityEngine;

interface IGameBase
{
    void OnStart(); 
    void OnWin(); 
    void OnFail();
    IEnumerator IEOnTick();
}

public abstract class GameBase : MonoBehaviour, IGameBase
{
    [SerializeField]
    protected float gameDuration;

    public virtual IEnumerator IEOnTick()
    {
        yield return null;
        throw new System.NotImplementedException();
    }

    public virtual void OnStart()
    {

    }

    public virtual void OnWin()
    {
        GameController.WinGame();
    }
    public virtual void OnFail()
    {
        GameController.FailGame();
    }

    protected virtual void Start() { }
    protected virtual void Update() { }
}
