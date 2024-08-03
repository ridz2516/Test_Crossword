using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrosswordGenerator : MonoBehaviour
{
    #region Data

    public GameObject tilePrefab; 
    public RectTransform tileArea; 

    [SerializeField]private List<CrossedWord> m_CrossWordsToShow = new List<CrossedWord>();
    [SerializeField] private List<CellData> m_AllCell = new List<CellData>();

    #endregion Data

    public List<CellData> AllCell => m_AllCell;

    public CellData GetSpecificCell(Vector2 i_Id)
    {
        for (int i = 0; i < AllCell.Count; i++)
        {
            if (AllCell[i].Position == i_Id)
            {
                return AllCell[i];
            }
        }

        return null;
    }

    public void ResetGrid()
    {
        while (m_AllCell.Count > 0)
        {
            Destroy(m_AllCell[0].gameObject);
            m_AllCell.RemoveAt(0);
        }
        m_AllCell.Clear();
        m_CrossWordsToShow.Clear();

    }

    public List<CrossedWord> SpawnGrid(string[] i_WordsArray)
    {
        GenerateCrossWords(i_WordsArray);
        SpawnTiles();

        return m_CrossWordsToShow;
    }

    private void GenerateCrossWords(string[] i_WordsArray)
    {
        List<CrossedWord> fixedWordsList = new List<CrossedWord>();

        foreach (string word in i_WordsArray)
        {
            fixedWordsList.Add(new CrossedWord(word.Trim(), "Placeholder clue"));
        }

        List<CrossedWord> bestCrossWords = new List<CrossedWord>();
        float bestGridSize = float.MaxValue;
        int wordsPlaced = 0;
        int minGridX = 0, minGridY = 0;

        for (int gen = 0; gen < fixedWordsList.Count; gen++)
        {
            List<CrossedWord> currentWords = new List<CrossedWord>(fixedWordsList);
            List<CrossedWord> currentPlacement = new List<CrossedWord>();

            currentPlacement.Add(new CrossedWord(currentWords[0]));
            currentWords.RemoveAt(0);

            int minX = 0, maxX = currentPlacement[0].Size - 1, minY = 0, maxY = 0;

            int maxAttempts = Mathf.FloorToInt(currentWords.Count * currentWords.Count);
            int attemptIndex = 0;
            int wordIndex = 0;

            for (; currentWords.Count > 0 && attemptIndex < maxAttempts; attemptIndex++)
            {
                CrossedWord wordToPlace = new CrossedWord(currentWords[wordIndex]);

                bool isPlaced = false;
                Tile bestPosition = new Tile(0, 0);
                CrossedWord.Direction bestDirection = CrossedWord.Direction.Horizontal;
                float bestScore = float.MaxValue;

                foreach (CrossedWord placedWord in currentPlacement)
                {
                    wordToPlace.WordDirection = (placedWord.WordDirection == CrossedWord.Direction.Horizontal)
                        ? CrossedWord.Direction.Vertical
                        : CrossedWord.Direction.Horizontal;

                    List<Tile>[] intersections = placedWord.SimilarLetterTiles(wordToPlace);

                    for (int letterIndex = 0; letterIndex < intersections.Length; letterIndex++)
                    {
                        foreach (Tile tile in intersections[letterIndex])
                        {
                            if (placedWord.WordDirection == CrossedWord.Direction.Horizontal)
                            {
                                wordToPlace.StartingPosition = new Tile(tile.X, tile.Y - letterIndex);
                            }
                            else
                            {
                                wordToPlace.StartingPosition = new Tile(tile.X - letterIndex, tile.Y);
                            }

                            bool canBePlaced = true;
                            int validIntersections = 0;
                            foreach (CrossedWord w in currentPlacement)
                            {
                                int acceptance = w.CanAccept(wordToPlace);
                                if (acceptance > 0) validIntersections += acceptance;
                                if (acceptance == -1) canBePlaced = false;
                            }

                            if (canBePlaced && validIntersections > 0)
                            {
                                int inverseIntersections = -validIntersections;
                                float score = UnityEngine.Random.Range(0, 10) + inverseIntersections * 100;

                                if (score < bestScore)
                                {
                                    isPlaced = true;
                                    bestScore = score;
                                    bestPosition = wordToPlace.StartingPosition;
                                    bestDirection = wordToPlace.WordDirection;
                                }
                            }
                        }
                    }
                }

                if (isPlaced)
                {
                    wordToPlace.StartingPosition = bestPosition;
                    wordToPlace.WordDirection = bestDirection;
                    currentPlacement.Add(wordToPlace);

                    for (int j = currentPlacement.Count - 1; j > 0; j--)
                    {
                        int r = UnityEngine.Random.Range(0, j + 1);
                        CrossedWord tmp = currentPlacement[r];
                        currentPlacement[r] = currentPlacement[j];
                        currentPlacement[j] = tmp;
                    }

                    minX = Mathf.Min(minX, wordToPlace.StartingPosition.X);
                    minY = Mathf.Min(minY, wordToPlace.StartingPosition.Y);
                    maxX = Mathf.Max(maxX, wordToPlace.WordDirection == CrossedWord.Direction.Horizontal
                        ? wordToPlace.StartingPosition.X + wordToPlace.Size - 1
                        : wordToPlace.StartingPosition.X);
                    maxY = Mathf.Max(maxY, wordToPlace.WordDirection == CrossedWord.Direction.Vertical
                        ? wordToPlace.StartingPosition.Y + wordToPlace.Size - 1
                        : wordToPlace.StartingPosition.Y);

                    currentWords.RemoveAt(wordIndex);
                    if (currentWords.Count > 0)
                    {
                        wordIndex = wordIndex % currentWords.Count;
                    }
                }
                else
                {
                    wordIndex = (wordIndex + 1) % currentWords.Count;
                }
            }

            float gridSize = Mathf.Sqrt((maxX - minX) * (maxX - minX) + (maxY - minY) * (maxY - minY));
            int numWordsPlaced = currentPlacement.Count;

            if (gridSize - (numWordsPlaced - wordsPlaced) * 4 < bestGridSize && wordsPlaced < numWordsPlaced)
            {
                bestCrossWords = currentPlacement;
                bestGridSize = gridSize;
                wordsPlaced = numWordsPlaced;
                minGridX = minX;
                minGridY = minY;
            }
        }

        foreach (CrossedWord word in bestCrossWords)
        {
            word.StartingPosition.X -= minGridX;
            word.StartingPosition.Y = -word.StartingPosition.Y + minGridY;
        }

        m_CrossWordsToShow = bestCrossWords;
    }

    void SpawnTiles()
    {
        foreach (Transform child in tileArea)
        {
            Destroy(child.gameObject);
        }

        char[,] grid = GetCrossword();

        float tileSize = tilePrefab.GetComponent<RectTransform>().rect.width;

        Vector2 tileAreaSize = tileArea.rect.size;
        Vector2 areaCenter = tileAreaSize / 2;

        for (int y = 0; y < grid.GetLength(1); y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] != '0')
                {
                    CellData tile = Instantiate(tilePrefab, tileArea).GetComponent<CellData>();
                    RectTransform tileRectTransform = tile.GetComponent<RectTransform>();

                   
                    float posX = (x - grid.GetLength(0) / 2) * tileSize;
                    float posY = (y - grid.GetLength(1) / 2) * tileSize;

                    tileRectTransform.anchoredPosition = new Vector2(posX, posY);

                    tile.Letter = grid[x, y];
                    tile.Position = new Vector2(x, y- (grid.GetLength(1)-1));
                    m_AllCell.Add(tile);
                }
            }
        }
    }

    public char[,] GetCrossword()
    {
        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (CrossedWord word in m_CrossWordsToShow)
        {
            minX = Mathf.Min(minX, word.StartingPosition.X);
            maxX = Mathf.Max(maxX, word.StartingPosition.X + (word.WordDirection == CrossedWord.Direction.Horizontal ? word.Size - 1 : 0));
            minY = Mathf.Min(minY, word.StartingPosition.Y - (word.WordDirection == CrossedWord.Direction.Vertical ? word.Size - 1 : 0));
            maxY = Mathf.Max(maxY, word.StartingPosition.Y);
        }

        char[,] grid = new char[maxX - minX + 1, maxY - minY + 1];
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                grid[x, y] = '0';
            }
        }

        foreach (CrossedWord word in m_CrossWordsToShow)
        {
            for (int i = 0; i < word.Size; i++)
            {
                int x = word.StartingPosition.X;
                int y = word.StartingPosition.Y;

                if (word.WordDirection == CrossedWord.Direction.Horizontal)
                {
                    x += i;
                }
                else
                {
                    y -= i;
                }

                grid[x - minX, y - minY] = word.Word[i];
            }
        }

        return grid;
    }
}