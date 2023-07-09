using UnityEngine;

public class GameController : MonoBehaviour
{
    private enum GameResult
    {
        Win,
        Lose,
        None
    }

    public static GameController Instance;

    [SerializeField] private MainMenu mainUI;
    [SerializeField] private GameConfig gameConfig;

    [Header("Dev")] [SerializeField] private GameBase testMiniGame;
    private GameBase _currentGame;
    private AudioSource _audioSource;
    private GameResult _gameResult = GameResult.None;

    public GameBase CurrentGame => _currentGame;

    private void Awake()
    {
        if (Instance != null) return;
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

    public static void StartGame()
    {
        if (!Instance._currentGame)
        {
            Debug.LogError("Current Game Can't Be Null");
            return;
        }

        Instance.mainUI.TransitionOut(() => { Instance._currentGame.OnStart(); });
    }

    public static void LoadNextGame()
    {
        Instance.mainUI.CloseMenu();
        Instance.mainUI.TransitionIn(() =>
        {
            if (Instance._currentGame) Destroy(Instance._currentGame.gameObject);
            var randomGame = Instance.gameConfig.RandomGame(Instance._currentGame);
            Instance._currentGame = randomGame.Item2;
            Instance.mainUI.ShowHint(randomGame.Item1);
            Instance._currentGame.transform.localPosition = Vector3.zero;
            Instance._currentGame.transform.localScale = Vector3.one;

            if (Instance._gameResult == GameResult.Win)
                Instance.ShowHealth();
            else if(Instance._gameResult == GameResult.Lose)
                Instance.LoseHealth();
            else
                Instance.ShowHealth();
        });
    }

    public static void WinGame()
    {
        Instance._gameResult = GameResult.Win;
        LoadNextGame();
    }

    public static void FailGame()
    {
        Instance._gameResult = GameResult.Lose;
        LoadNextGame();
    }

    public void ShowHealth()
    {
        Instance.mainUI.ShowHealth(() =>
        {
            PlayAudio();
            mainUI.HideHealth(null);
        });
    }

    public void LoseHealth()
    {
        Instance.mainUI.LoseHealth(PlayAudio);
    }

    private void PlayAudio()
    {
        if (Instance._gameResult == GameResult.None) return;
        var resultAudio = Instance._gameResult == GameResult.Win
            ? Instance.gameConfig.success
            : Instance.gameConfig.fail;
        Instance._audioSource.clip = resultAudio;
        Instance._audioSource.Play();
    }
}