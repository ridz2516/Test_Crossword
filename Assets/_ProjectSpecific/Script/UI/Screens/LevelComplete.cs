using UnityEngine;
using UnityEngine.UI;

public class LevelComplete : ScreenBase
{
    [SerializeField] private Button m_Next;


    private void OnEnable()
    {
        m_Next.onClick.AddListener(OnNextClicked);

        SoundManager.Instance.PlayClip(eSoundEffect.LevelWin);
        EnvironmentController.Instance.PlayConfetti();
    }

    private void OnDisable()
    {
        m_Next.onClick.RemoveAllListeners();
    }

    private void OnNextClicked()
    {
        SoundManager.Instance.PlayClip(eSoundEffect.ButtonClick);
        GameManager.Instance.GameReset();
    }
}
