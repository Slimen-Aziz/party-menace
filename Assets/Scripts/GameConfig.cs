using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameData/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    [Header("Audio")]
    public AudioClip success;
    public AudioClip fail;
    
    [Header("Shared Configuration")]
    public float baseDuration;
    [Header("MiniGames")]
    public List<GameBase> availableGames;

    public GameBase RandomGame(GameBase exclusion)
    {
        if (availableGames == null || availableGames.Count == 0) return null;
        var miniGames = availableGames.ToList();
        if (exclusion)
        {
            var target = miniGames.FirstOrDefault(item => item.GetType() == exclusion.GetType() );
            // var message = target ? target.GetType().ToString() : "Null";
            // Debug.Log($"{message} == {exclusion.GetType()}");
            miniGames.Remove(target);
        }
        var result = Instantiate(miniGames[Random.Range(0, miniGames.Count - 1)]);
        return result;
    }
}
