
using Sirenix.OdinInspector;
using System;

[Serializable]
public class Tile
{
    [ShowInInspector,ReadOnly] public int X { get; set; } // X position in the grid
    [ShowInInspector, ReadOnly] public int Y { get; set; } // Y position in the grid

    public Tile(int x, int y)
    {
        X = x;
        Y = y;
    }
}
