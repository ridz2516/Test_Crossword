using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HomeScreen : ScreenBase
{
    #region Data

    [SerializeField] private TextMeshProUGUI m_LevelButtonText;
    [SerializeField] private Button m_LevelButton;

    #endregion Data

    #region Init

    private void OnEnable()
    {
        m_LevelButton.onClick.AddListener(OnLevelClicked);

        initialized();
    }

    private void OnDisable()
    {
        m_LevelButton.onClick.RemoveAllListeners();
    }

    #endregion Init

    private void OnLevelClicked()
    {
        SoundManager.Instance.PlayClip(eSoundEffect.ButtonClick);
        LevelController.Instance.CreateALevel();
        GameManager.Instance.StartLevel();
    }

    private void initialized()
    {
        m_LevelButtonText.text = "Level "+ StorageManager.Instance.CurrentLevel;
    }

}
