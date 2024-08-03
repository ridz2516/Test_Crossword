using System.Collections;
using UnityEngine;

public class ScreenManager : Singleton<ScreenManager>
{
    [Header("Private References")]
    [SerializeField]
    private ScreenDelay m_ScreenDelays;
    [SerializeField]
    private ScreenBase[] m_Screens;
    private GameStates GameState
    {
        get
        {
            return GameManager.Instance.GameState;
        }
    }
    private void OnEnable()
    {
        GameManager.OnGameStateChange += UpdateScreenStates;
        UpdateScreenStates();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnGameStateChange -= UpdateScreenStates;

    }

    public void UpdateScreenStates()
    {
        for (int i = 0; i < m_Screens.Length; i++)
        {
            if (GameManager.Instance.GameState == m_Screens[i].ScreenState)
            {
                OpenScreen(m_Screens[i]);
            }
            else
            {
                CloseScreen(m_Screens[i]);
            }

        }
    }

    public void OpenScreen(ScreenBase i_Screen)
    {
        float delay = m_ScreenDelays.ContainsKey(GameState) ? m_ScreenDelays[GameState] : 0f;
        m_Couroutine = StartCoroutine(OpenScreenCoroutine(delay, i_Screen));
    }

    private Coroutine m_Couroutine;
    public IEnumerator OpenScreenCoroutine(float i_Delay, ScreenBase i_Screen)
    {
        yield return new WaitForSecondsRealtime(i_Delay);
        i_Screen.OpenScreen();
        i_Screen.gameObject.SetActive(true);
        m_Couroutine = null;

    }

    public void CloseScreen(ScreenBase i_Screen)
    {
        i_Screen.CloseScreen();
        //i_Screen.gameObject.SetActive(false);
    }


#if UNITY_EDITOR
    [Sirenix.OdinInspector.Button]
    public void SetRefs()
    {
        m_Screens = GetComponentsInChildren<ScreenBase>(true);
    }
#endif
}
[System.Serializable]
public class ScreenDelay : UnitySerializedDictionary<GameStates, float>
{
}