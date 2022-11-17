using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour, ISwipeHandler
{
    private ScriptableLevel _level;
    private MoveResultChecker moveResultChecker = new MoveResultChecker();
    private LevelManager levelManager = new LevelManager();
    public TilemapManager TilemapManager;
    public GestureManager GestureManager;
    public GameObject UnitWorldObject;
    public Unit Unit;

    Vector3Int currentPosition;

    public string levelId;
    public string levelPack = "Base";
    public string levelType = "Base";
    // Start is called before the first frame update
    void Start()
    {
        Unit.levelHandler = this;
        LoadLevel(levelId, levelPack, levelType);

        MoveToStart();
        Bind();
    }

    private void MoveToStart()
    {
        var startTile = StartTile();
        InstantlyMoveToPosition(startTile);
        currentPosition = startTile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Bind()
    {
        GestureManager.UpSwipe += UpSwipe;
        GestureManager.DownSwipe += DownSwipe;
        GestureManager.RightSwipe += RightSwipe;
        GestureManager.LeftSwipe += LeftSwipe;

        GestureManager.DoubleClick += GestureManager_DoubleClick;
    }

    private void GestureManager_DoubleClick()
    {
        TilemapManager.ToggleMark(currentPosition, levelType);
    }

    void Unbind()
    {
        GestureManager.UpSwipe -= UpSwipe;
        GestureManager.DownSwipe -= DownSwipe;
        GestureManager.RightSwipe -= RightSwipe;
        GestureManager.LeftSwipe -= LeftSwipe;

        GestureManager.DoubleClick -= GestureManager_DoubleClick;
    }

    public void LoadLevel(string levelId, string levelPack, string levelType)
    {
        TilemapManager.LoadMap(levelId, levelPack, levelType);
        _level = TilemapManager.LastLoadedLevel;
    }

    public Vector3Int StartTile()
    {
        if(_level != null)
        {
            return _level.ObjectTiles.Find(tile => tile.Tile.Type == TileType.Start).Position;
        } else
        {
            return new Vector3Int();
        }
    }

    public void UpSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.y++;
        HandleSwipe(newTilePosition, SwipeDirection.Up);
    }

    public void DownSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.y--;
        HandleSwipe(newTilePosition, SwipeDirection.Down);
    }

    public void LeftSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.x--;
        HandleSwipe(newTilePosition, SwipeDirection.Left);
    }

    public void RightSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.x++;
        HandleSwipe(newTilePosition, SwipeDirection.Right);
    }

    void HandleSwipe(Vector3Int position, SwipeDirection direction)
    {
        if (MoveToPositionIfCan(position))
        {
            switch (direction)
            {
                case SwipeDirection.Up:
                    Unit.UpSwipe();
                    break;
                case SwipeDirection.Down:
                    Unit.DownSwipe();
                    break;
                case SwipeDirection.Right:
                    Unit.RightSwipe();
                    break;
                case SwipeDirection.Left:
                    Unit.LeftSwipe();
                    break;
            }

            currentPosition = position;

        }
        else
        {
            Unit.WrongSwipe();

        }
        CheckMove();
    }

    void InstantlyMoveToPosition(Vector3Int position)
    {
        Camera.main.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, Camera.main.transform.position.z);
        UnitWorldObject.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
    }

    void MoveToPosition(Vector3Int position)
    {
        StartCoroutine(Move(UnitWorldObject, new Vector3(position.x + 0.5f, position.y + 0.5f, 0)));
        StartCoroutine(Move(Camera.main.gameObject, new Vector3(position.x + 0.5f, position.y + 0.5f, Camera.main.transform.position.z)));
    }

    bool MoveToPositionIfCan(Vector3Int position)
    {
        if(_level.GroundTiles.Find(tile => { return tile.Position.x == position.x && tile.Position.y == position.y; }) != null) {
            MoveToPosition(position);
            return true;
        }
        return false;
    }

    void CheckMove()
    {
        var ground = _level.GroundTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });
        var objectTile = _level.ObjectTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });
        var unit = _level.UnitTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });

        var result = moveResultChecker.CheckMove(ground?.Tile, objectTile?.Tile, unit?.Tile);

        switch (result)
        {
            case MoveResult.None:
                Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case MoveResult.Finish:
                Unbind();
                Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                Unit.Win();
                break;
            case MoveResult.Death:
                Unbind();
                Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Unit.Death();
                break;
        }
           
    }

    IEnumerator Move(GameObject gameObject, Vector3 position)
    {
        SwipeStarted();
        while (Mathf.Abs(gameObject.transform.position.sqrMagnitude - position.sqrMagnitude) > 0.25f)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, position, 0.5f);
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = position;
        SwipeFinished();
    }

    public void AfterUnitDeathFunction()
    {
        MoveToStart();
        Bind();
        Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Unit.Idle();
    }

    public void AfterWinFunction()
    {
        if (!levelManager.IsLastLevelInPack(levelId, levelPack))
        {
            var newLevelId = levelManager.NextLevelId(levelId, levelPack);
            LoadLevel(newLevelId, levelPack, levelType);
            levelId = newLevelId;
            MoveToStart();
            Bind();
            Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void SwipeStarted()
    {
        GestureManager.SetSwipeInProgress(true);
    }

    public void SwipeFinished()
    {
        GestureManager.SetSwipeInProgress(false);
    }

    private void OnDestroy()
    {
        Unbind();
    }
}
