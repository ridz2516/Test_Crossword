using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellData : MonoBehaviour
{
    [ShowInInspector, ReadOnly] public char Letter { get; set; }
    [ShowInInspector, ReadOnly] public Vector2 Position { get; set; }

    [SerializeField] private TextMeshProUGUI m_Text;

    [SerializeField] private Color m_DefaultColor;
    [SerializeField] private Image m_BgColor;


    public CellData(char letter, Vector2 position)
    {
        Letter = letter;
        Position = position;
    }

    private void OnEnable()
    {
        m_Text.text = "";
        m_BgColor.color = m_DefaultColor;
    }

    public void ActivateCell()
    {
        m_Text.text = ""+ Letter;
        m_BgColor.color = LevelController.Instance.CurrentLevelData.LevelColor;
    }
}
