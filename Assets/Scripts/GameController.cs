using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private List<GameBase> miniGames;
    [SerializeField] private GameBase testGame;

    private void Awake()
    {
        Debug.Log("Initialize");
    }

    private void Start()
    {
        testGame.OnStart();
    }
}
