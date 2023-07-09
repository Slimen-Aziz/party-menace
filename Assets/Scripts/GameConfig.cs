using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable] public struct GameData
{
    public GameBase prefab;
    public string hint;
}

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameData/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Audio")]
    public AudioClip success;
    public AudioClip fail;
    
    [Header("Shared Configuration")]
    public float baseDuration;
    [Header("MiniGames")]
    public List<GameData> availableGames;

    public (string, GameBase) RandomGame(GameBase exclusion)
    {
        if (availableGames == null || availableGames.Count == 0) return ("No Game", null);
        var miniGames = availableGames.ToList();
        if (exclusion)
        {
            var target = miniGames.FirstOrDefault(item => item.prefab.GetType() == exclusion.GetType() );
            // var message = target ? target.GetType().ToString() : "Null";
            // Debug.Log($"{message} == {exclusion.GetType()}");
            miniGames.Remove(target);
        }

        var miniGame = miniGames[Random.Range(0, miniGames.Count - 1)];
        var result = Instantiate(miniGame.prefab);
        
        return (miniGame.hint, result);
    }
}
