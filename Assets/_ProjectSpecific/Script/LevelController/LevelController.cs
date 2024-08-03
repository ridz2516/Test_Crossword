using System.Collections.Generic;
using UnityEngine;

public class LevelController : Singleton<LevelController>
{
    #region Data

    [SerializeField] private LevelData[] m_LevelData;
    [SerializeField] private CrosswordGenerator m_CrosswordGenerator;
    private LevelData m_CurrentLevelData;

    private List<CrossedWord> GameData = new List<CrossedWord>();

    public LevelData CurrentLevelData => m_CurrentLevelData;

    #endregion Data

    public void CreateALevel()
    {
        m_CurrentLevelData = m_LevelData[(StorageManager.Instance.CurrentLevel - 1) % m_LevelData.Length];

        GameData = m_CrosswordGenerator.SpawnGrid(m_CurrentLevelData.TargetWords.ToArray());
    }

    public void TestWord(string i_TargetWord)
    {
        foreach (var data in GameData)
        {
            if(!data.IsCompleted && data.Word == i_TargetWord)
            {
                data.IsCompleted = true;
                activateGrid(new Vector2Int(data.StartingPosition.X, data.StartingPosition.Y),data.WordDirection,data.Size);
                SoundManager.Instance.PlayClip(eSoundEffect.CorrectWordSelected);
            }
        }

        checkLevelCompleted();
    }

    private void activateGrid(Vector2Int i_Id,CrossedWord.Direction i_Direction, int i_MaxSize)
    {
        for (int i = 0; i < m_CrosswordGenerator.AllCell.Count; i++)
        {
            if(m_CrosswordGenerator.AllCell[i].Position == i_Id)
            {
                if(i_Direction == CrossedWord.Direction.Horizontal)
                {
                    for (int x = 0; x < i_MaxSize; x++)
                    {
                        m_CrosswordGenerator.GetSpecificCell(new Vector2(i_Id.x + x, i_Id.y)).ActivateCell();
                    }
                }
                else
                {
                    for (int y = 0; y < i_MaxSize; y++)
                    {
                        m_CrosswordGenerator.GetSpecificCell(new Vector2(i_Id.x, i_Id.y - y)).ActivateCell();
                    }
                }
            }
        }

    }

    private void checkLevelCompleted()
    {
        foreach (var data in GameData)
        {
            if (!data.IsCompleted)
            {
                return;
            }
        }

        m_CrosswordGenerator.ResetGrid();
        GameManager.Instance.LevelCompleted();
    }

}
