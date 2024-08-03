using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Alphabet : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private TextMeshProUGUI m_AlphabetText;
    [SerializeField] private Image  m_AlhabetIcon;
    [SerializeField] private RectTransform m_MainTransform;
    private char m_Alphabet;

    public char AlphabetChar => m_Alphabet;
    [SerializeField] public RectTransform MainTransform => m_MainTransform;
    private InteractableAlphabet m_Parent;

    #region Init

    private void OnEnable()
    {
        GameManager.OnGameRestarted += OnGameReset;
    }

    private void OnDisable()
    {
        GameManager.OnGameRestarted -= OnGameReset;
    }

    #endregion Init

    #region Event

    [Button]
    private void OnGameReset()
    {
        PoolBack();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!InputManager.Instance.IsInputDown) return;

        if(!m_AlhabetIcon.gameObject.activeSelf)
            activate();
        else
        {
            if(m_Parent.AllAlphabet.Count >= 2 && m_Parent.AllAlphabet[m_Parent.AllAlphabet.Count -2] == this)
            {
                m_Parent.RemoveLastAlphabet();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        activate();
    }

    #endregion Event

    public void Initialize(char i_TargetAlhabet,InteractableAlphabet i_TargetParent)
    {
        m_Alphabet = i_TargetAlhabet;
        m_AlphabetText.text = i_TargetAlhabet+"";
        m_Parent = i_TargetParent;
        m_AlphabetText.color = Color.white;
    }

    public void ResetAlphabet()
    {
        m_AlhabetIcon.gameObject.SetActive(false);
        m_AlhabetIcon.color = Color.red;
        m_AlphabetText.color = Color.white;
    }

    public void PoolBack()
    {
        Destroy(this.gameObject);
    }

    private void activate()
    {
        m_AlhabetIcon.gameObject.SetActive(true);
        m_AlhabetIcon.color = LevelController.Instance.CurrentLevelData.LevelColor;
        m_AlphabetText.color = Color.white;
        m_Parent.AddAlphabet(this);
        SoundManager.Instance.PlayClip(eSoundEffect.AlphabetSelected);
    }

}
