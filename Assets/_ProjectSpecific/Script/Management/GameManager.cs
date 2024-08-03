using UnityEngine;


[DefaultExecutionOrder(-50)]
public class GameManager : Singleton<GameManager> {
    #region Game Events
    public delegate void GameEvent();
    public static event GameEvent OnGameStateChange = delegate { };
    public static event GameEvent OnLevelLoaded = delegate { };
    public static event GameEvent OnLevelComplete = delegate { };
    public static event GameEvent OnLevelFailed = delegate { };
    public static event GameEvent OnLevelStarted = delegate { };
    public static event GameEvent OnGameRestarted = delegate { };
    #endregion

    [SerializeField]
    private GameStates initGameState = GameStates.Idle;
    public GameStates GameState {
        get {
            return m_GameState;
        }
        set {
            if (value != m_GameState) {
                m_GameState = value;
                OnGameStateChange?.Invoke();
            }

        }
    }
    private GameStates m_GameState;

    public override void Start() {

        Application.targetFrameRate = Mathf.Clamp(Screen.currentResolution.refreshRate,60,240);
        GameState = initGameState;
        OnGameStateChange?.Invoke();
    }


    public void StartLevel() {
        GameState = GameStates.Playing;
        OnLevelStarted?.Invoke();
    }

    public void LevelCompleted() {
        GameState = GameStates.GameComplete;
        OnLevelComplete?.Invoke();
        StorageManager.Instance.CurrentLevel++;
    }
    public void LevelFailed() {
        GameState = GameStates.GameOver;
        OnLevelFailed?.Invoke();
    }

    public void LoadLevel() {


    }

    public void GameReset()
    {
        GameState = GameStates.Idle;
        OnGameRestarted?.Invoke();
    }



}