public enum MoveResult {
    None,
    Finish,
    Death,
}

public class MoveResultChecker {
    public MoveResult CheckMove(LevelTile groundTile, LevelTile objectTile, LevelTile unitTile)
    {
        switch(objectTile?.Type)
        {
            case TileType.Finish:
                return MoveResult.Finish;
            case TileType.Pit:
                return MoveResult.Death;
        }
        return MoveResult.None;
    }
}
