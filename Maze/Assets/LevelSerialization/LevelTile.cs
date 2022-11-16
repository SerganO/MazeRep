using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Level Tile", menuName = "2D/Tiles/Level Tile")]
public class LevelTile : RuleTile
{
    public TileType Type;

}

[Serializable]
public enum TileType
{
    // Ground
   Grass = 0,
   Some = 1,

   // Object

   Start = 500,
   Finish = 501,

   // Unit
   Snorlax = 1000,
   Quinn = 1001
}
