using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] private MainMenu mainUI;
    [SerializeField] private GameConfig gameConfig;

    [Header("Dev")]
    [SerializeField] private GameBase testMiniGame;
    private GameBase _currentGame;
    private AudioSource _audioSource;

    public GameBase CurrentGame => _currentGame;
    
    private void Awake()
    {
        if(Instance != null) return;
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        mainUI.Init();
        _audioSource = GetComponent<AudioSource>();
        if (!testMiniGame) return;
        _currentGame = testMiniGame;
        StartGame();
        mainUI.CloseMenu();
    }
    
    private static void StartGame()
    {
        if (!Instance._currentGame)
        {
            Debug.LogError("Current Game Can't Be Null");
            return;
        }
        Instance._currentGame.OnStart();
    }

    public static void LoadNextGame()
    {
        if(Instance._currentGame) Destroy(Instance._currentGame.gameObject);
        Instance._currentGame = Instance.gameConfig.RandomGame(Instance._currentGame);
        Instance._currentGame.transform.localPosition = Vector3.zero;
        Instance._currentGame.transform.localScale = Vector3.one;
        Instance.mainUI.CloseMenu();
        //TODO: Show Transition
        StartGame();
    }

    public static void WinGame()
    {
        //TODO: Show Health
        Instance._audioSource.clip = Instance.gameConfig.success;
        Instance._audioSource.Play();
        LoadNextGame();
    }

    public static void FailGame()
    {
        //TODO: Show and Lose Health Anim
        Instance._audioSource.clip = Instance.gameConfig.fail;
        Instance._audioSource.Play();
        LoadNextGame();
    }
}
