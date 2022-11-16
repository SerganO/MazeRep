using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour, ISwipeHandler
{
    private ScriptableLevel _level;
    private MoveResultChecker moveResultChecker = new MoveResultChecker();
    public TilemapManager TilemapManager;
    public SwipeManager SwipeManager;
    public Unit Unit;

    Vector3Int currentPosition;

    public string levelId;
    public string levelType = "Base";
    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(levelId);

        MoveToStart();
        Bind();
    }

    private void MoveToStart()
    {
        var startTile = StartTile();
        MoveToPosition(startTile);
        currentPosition = startTile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Bind()
    {
        SwipeManager.UpSwipe += UpSwipe;
        SwipeManager.DownSwipe += DownSwipe;
        SwipeManager.RightSwipe += RightSwipe;
        SwipeManager.LeftSwipe += LeftSwipe;
    }

    void Unbind()
    {
        SwipeManager.UpSwipe -= UpSwipe;
        SwipeManager.DownSwipe -= DownSwipe;
        SwipeManager.RightSwipe -= RightSwipe;
        SwipeManager.LeftSwipe -= LeftSwipe;
    }

    public void LoadLevel(string levelId)
    {
        TilemapManager.LoadMap(levelId, levelType);
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
        HandleSwipe(newTilePosition);
    }

    public void DownSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.y--;
        HandleSwipe(newTilePosition);
    }

    public void LeftSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.x--;
        HandleSwipe(newTilePosition);
    }

    public void RightSwipe()
    {
        var newTilePosition = currentPosition;
        newTilePosition.x++;
        HandleSwipe(newTilePosition);
    }

    void HandleSwipe(Vector3Int position)
    {
        if (MoveToPositionIfCan(position)) currentPosition = position;
        else
        {
            Unit.gameObject.transform.localScale = new Vector3(0.5f, 0.5f);

            Helper.Wait(this, 0.25f, () => { Unit.gameObject.transform.localScale = new Vector3(1, 1); });

        }
        CheckMove();
    }
    void MoveToPosition(Vector3Int position)
    {
        Camera.main.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, Camera.main.transform.position.z);
        Unit.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
    }

    bool MoveToPositionIfCan(Vector3Int position)
    {
        if(_level.GroundTiles.Find(tile => { return tile.Position.x == position.x && tile.Position.y == position.y; }) != null) {
            Camera.main.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, Camera.main.transform.position.z);
            Unit.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, 0);
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
                Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                break;
            case MoveResult.Death:
                Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Helper.Wait(this, 0.25f, () => {
                    MoveToStart();
                    Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                });
                break;
        }
           
    }



    private void OnDestroy()
    {
        Unbind();
    }
}
