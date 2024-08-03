using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class InteractableAlphabet : MonoBehaviour
{
    [SerializeField] private GameObject     m_Alphabet;
    [SerializeField] private RectTransform  m_CenterRect;
    [SerializeField] private UILineRenderer m_UILineRenderer;
    [SerializeField] private Transform  m_MousePos;
    [SerializeField] private TextMeshProUGUI m_WordText;

    private string m_Word;
    [SerializeField, ReadOnly] private List<Alphabet> m_AllSelectedAlphabet = new List<Alphabet>();
    [SerializeField, ReadOnly] private List<Alphabet> m_AllAlphabet = new List<Alphabet>();

    public float radius = 100f;

    #region Init


    private void OnEnable()
    {
        GameManager.OnGameRestarted += OnGameReset;
        InputManager.OnInputUp += OnInputUp;
        OnLevelStarted();
    }

    private void OnDisable()
    {
        GameManager.OnGameRestarted -= OnGameReset;
        InputManager.OnInputUp -= OnInputUp;
        OnGameReset();
    }


    #endregion Init

    #region Event

    private void OnGameReset()
    {
        m_Word = "";
        resetAllAlphabets();
        m_AllSelectedAlphabet.Clear();
        updateLineRenderer();
    }

    private void OnLevelStarted()
    {
        m_UILineRenderer.color = LevelController.Instance.CurrentLevelData.LevelColor;
        SpawnUIElements(LevelController.Instance.CurrentLevelData.Alphabets);
    }
    private void OnInputUp()
    {
        LevelController.Instance.TestWord(m_Word);

        for (int i = 0; i < m_AllSelectedAlphabet.Count; i++)
        {
            m_AllSelectedAlphabet[i].ResetAlphabet();
        }

        m_AllSelectedAlphabet.Clear();
        updateLineRenderer();
    }

    #endregion Event

    private void Update()
    {
        if (InputManager.Instance.IsInputDown && m_AllSelectedAlphabet.Count > 0)
        {
            m_MousePos.position = Input.mousePosition;

            m_UILineRenderer.Points[m_UILineRenderer.Points.Length - 1] = new Vector2(m_MousePos.localPosition.x, m_MousePos.localPosition.y-50);
            m_UILineRenderer.SetAllDirty();
        }
    }

    private void resetAllAlphabets()
    {
        for (int i = 0; i < m_AllAlphabet.Count; i++)
        {
            m_AllAlphabet[i].PoolBack();
        }
        m_AllAlphabet.Clear();
    }

    void SpawnUIElements( char[] i_Alphabets)
    {
        float angleStep = 360f / i_Alphabets.Length;
        Vector2 centerPosition = m_CenterRect.rect.center;

        for (int i = 0; i < i_Alphabets.Length; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector2 spawnPosition = centerPosition + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;

            GameObject uiElement = Instantiate(m_Alphabet, transform);
            var alphabet = uiElement.GetComponent<Alphabet>();
            alphabet.Initialize(i_Alphabets[i], this);
            m_AllAlphabet.Add(alphabet);

            RectTransform rectTransform = uiElement.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = spawnPosition;
        }
    }


    public void AddAlphabet(Alphabet i_Alphabet)
    {
        m_AllSelectedAlphabet.Add(i_Alphabet);

        updateLineRenderer();
    }

    public void RemoveAlphabet(Alphabet i_Alphabet)
    {
        m_AllSelectedAlphabet.Remove(i_Alphabet);
        updateLineRenderer();
    }

    public void RemoveLastAlphabet()
    {
        m_AllSelectedAlphabet[m_AllSelectedAlphabet.Count - 1].ResetAlphabet();
        m_AllSelectedAlphabet.RemoveAt(m_AllSelectedAlphabet.Count - 1);
        updateLineRenderer();
    }

    public List<Alphabet> AllAlphabet => m_AllSelectedAlphabet;

    private void updateLineRenderer()
    {
        if (m_AllSelectedAlphabet.Count == 0)
        {
            m_UILineRenderer.Points = new Vector2[0];


            m_Word = "";
        }
        else
        {
            m_UILineRenderer.Points = new Vector2[m_AllSelectedAlphabet.Count + 1];
            m_Word = "";
            for (int i = 0; i < m_AllSelectedAlphabet.Count; i++)
            {
                m_UILineRenderer.Points[i] = new Vector2(m_AllSelectedAlphabet[i].MainTransform.localPosition.x, m_AllSelectedAlphabet[i].MainTransform.localPosition.y - 50);
                m_Word += m_AllSelectedAlphabet[i].AlphabetChar;
            }
        }

        m_WordText.text = m_Word;
    }
}
