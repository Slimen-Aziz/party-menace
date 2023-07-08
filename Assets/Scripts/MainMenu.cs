using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button startButton;
    private Canvas _canvas;
    
    public void Init()
    {
        _canvas = GetComponent<Canvas>();
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        GameController.LoadNextGame();
    }
    
    public void OpenMenu()
    {
        _canvas.enabled = true;
    }

    public void CloseMenu()
    {
        _canvas.enabled = false;
    }
}
