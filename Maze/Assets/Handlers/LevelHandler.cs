using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour, ISwipeHandler
{
    private ScriptableLevel _level;

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

        var startTile = StartTile();
        MoveToPosition(startTile);
        currentPosition = startTile;
        Bind();
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
            StartCoroutine(Helper.Wait(0.25f, () => { Unit.gameObject.transform.localScale = new Vector3(1, 1); }));

        }
        CheckFinish();
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

    void CheckFinish()
    {
        var finish = _level.ObjectTiles.Find(tile => { return tile.Tile.Type == TileType.Finish; });
        if (finish.Position.x == currentPosition.x && finish.Position.y == currentPosition.y)
            Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        else
            Unit.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }
        


    private void OnDestroy()
    {
        Unbind();
    }
}
