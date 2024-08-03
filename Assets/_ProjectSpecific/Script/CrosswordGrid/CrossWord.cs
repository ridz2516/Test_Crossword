using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

[Serializable]
public class CrossedWord
{
    public enum Direction { Horizontal, Vertical }

    [ShowInInspector,ReadOnly] public string Word { get; private set; } 
    [ShowInInspector, ReadOnly] public Direction WordDirection { get; set; } 
    [ShowInInspector, ReadOnly] public Tile StartingPosition { get; set; } 

    [ShowInInspector, ReadOnly] public int Size => Word.Length; 

    public bool IsCompleted { get; set; }


    public CrossedWord(string word, string clue)
    {
        this.Word = word;
        //this.Clue = clue;
        this.WordDirection = Direction.Horizontal;
        this.StartingPosition = new Tile(0, 0);
    }
    public CrossedWord(CrossedWord previous)
    {
        this.Word = previous.Word;
        this.WordDirection = previous.WordDirection;
        this.StartingPosition = new Tile(previous.StartingPosition.X, previous.StartingPosition.Y);
    }

    public List<Tile>[] SimilarLetterTiles(CrossedWord c)
    {
        List<Tile>[] tilesForEachLetter = new List<Tile>[c.Size];
        for (int i = 0; i < c.Size; i++)
        {
            List<Tile> TilesForCurrentLetter = new List<Tile>();
            for (int j = 0; j < this.Size; j++)
            {
                if (c.Word[i] == this.Word[j])
                    TilesForCurrentLetter.Add(this.TileAtWordPosition(j));
            }
            tilesForEachLetter[i] = TilesForCurrentLetter;
        }
        return tilesForEachLetter;
    }

    public Tile TileAtWordPosition(int pos)
    {
        if (pos >= 0 && pos < this.Size)
        {
            switch (this.WordDirection)
            {
                case Direction.Horizontal: return new Tile(this.StartingPosition.X + pos, this.StartingPosition.Y);
                case Direction.Vertical: return new Tile(this.StartingPosition.X, this.StartingPosition.Y + pos);
                default: throw new MissingMemberException("This Word has no direction");
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }

    public int CanAccept(CrossedWord c)
    {
        if (this.WordDirection == Direction.Horizontal && c.WordDirection == Direction.Horizontal)

            if (Math.Abs(c.StartingPosition.Y - this.StartingPosition.Y) > 1)
                return 0;

        if (Math.Abs(c.StartingPosition.Y - this.StartingPosition.Y) <= 1 && (this.StartingPosition.X > c.StartingPosition.X + c.Size || this.StartingPosition.X + this.Size < c.StartingPosition.X))
            return 2;

        if (this.WordDirection == Direction.Vertical && c.WordDirection == Direction.Vertical)

            if (Math.Abs(c.StartingPosition.X - this.StartingPosition.X) > 1)
                return 0;

        if (Math.Abs(c.StartingPosition.X - this.StartingPosition.X) <= 1 && (this.StartingPosition.Y > c.StartingPosition.Y + c.Size || this.StartingPosition.Y + this.Size < c.StartingPosition.Y))
            return 2;

        if (this.WordDirection == Direction.Horizontal && c.WordDirection == Direction.Vertical)
        {
            Tile potentialIntersection = new Tile(c.StartingPosition.X, this.StartingPosition.Y);

            char instanceChar = this.LetterOnTile(potentialIntersection);
            char otherChar = c.LetterOnTile(potentialIntersection);
            if (this.isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
            {
                if (instanceChar != '0')
                    return 1;
                else
                    return 0;
            }
            else if (instanceChar == '0' && (potentialIntersection.X < this.StartingPosition.X - 1 || potentialIntersection.X > this.StartingPosition.X + this.Size))
            {
                return 0;
            }
        }

        if (this.WordDirection == Direction.Vertical && c.WordDirection == Direction.Horizontal)
        {
            Tile potentialIntersection = new Tile(this.StartingPosition.X, c.StartingPosition.Y);

            char instanceChar = this.LetterOnTile(potentialIntersection);
            char otherChar = c.LetterOnTile(potentialIntersection);
            if (this.isWordOverTile(potentialIntersection) && c.isWordOverTile(potentialIntersection) && instanceChar == otherChar)
            {
                if (instanceChar != '0')
                    return 1;
                else
                    return 0;
            }
            else if (instanceChar == '0' && (potentialIntersection.Y < this.StartingPosition.Y - 1 || potentialIntersection.Y > this.StartingPosition.Y + this.Size))
            {
                return 0;
            }
        }


        return -1;
    }

    public bool isWordOverTile(Tile t)
    {
        return (this.WordDirection == Direction.Horizontal && t.Y == this.StartingPosition.Y && t.X >= this.StartingPosition.X && t.X < this.StartingPosition.X + this.Size)
            || (this.WordDirection == Direction.Vertical && t.X == this.StartingPosition.X && t.Y >= this.StartingPosition.Y && t.Y < this.StartingPosition.Y + this.Size);
    }

    public char LetterOnTile(Tile t)
    {
        if (isWordOverTile(t))
        {
            switch (this.WordDirection)
            {
                case Direction.Horizontal: return this.Word[t.X - this.StartingPosition.X];
                case Direction.Vertical: return this.Word[t.Y - this.StartingPosition.Y];
                default: return '0';
            }
        }
        else
        {
            return '0';
        }
    }
}