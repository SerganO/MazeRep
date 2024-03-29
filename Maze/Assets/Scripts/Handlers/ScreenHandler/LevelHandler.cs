using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : BaseHandler, ISwipeHandler
{
    private ScriptableLevel _level;
    private EnemyScriptableObject _enemy;
    private MoveResultChecker moveResultChecker = new MoveResultChecker();
    private LevelPackManager levelManager = new LevelPackManager();

    
    [HideInInspector]
    public Vector3Int currentPosition;

    public bool IsDebug = false;

    [Header("Managers")]
    public TilemapManager TilemapManager;
    public EnemyManager EnemyManager;
    public GestureManager GestureManager;

    [Header("Level settings")]
    public string levelId;
    public string levelPack = "Base";
    public string levelType = "Base";

    [Header("Hero")]
    public GameObject HeroWorldObject;
    public Hero Hero;

    [Header("Enemies")]
    public Transform EnemyParentObject;
    public List<Enemy> Enemies;


    private bool turnInProgress = false;
    // Start is called before the first frame update
    void Start()
    {
        Hero.levelHandler = this;
        if(!IsDebug)
        {
            var context = GetCurrentContext();
            levelId = context.levelId;
            levelPack = context.packId;
            levelType = context.levelType;
        }
        LoadLevel(levelId, levelPack, levelType);

        MoveToStart();
        Bind();
    }

    private void MoveToStart()
    {
        var startTile = StartTile();
        InstantlyMoveToPosition(startTile);
        currentPosition = startTile;
        foreach(var enemy in Enemies)
        {
            enemy.MoveToStartPosition();
            
        }
        Hero.CheckShadow();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) HandleSwipe(currentPosition, SwipeDirection.Right) ;
        if (Input.GetKeyDown(KeyCode.RightArrow)) RightSwipe();// HandleSwipe(new Vector3Int(currentPosition.x + 1, currentPosition.y), SwipeDirection.Right) ;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) LeftSwipe();// HandleSwipe(new Vector3Int(currentPosition.x - 1, currentPosition.y), SwipeDirection.Left) ;
        if (Input.GetKeyDown(KeyCode.UpArrow)) UpSwipe();// HandleSwipe(new Vector3Int(currentPosition.x, currentPosition.y + 1), SwipeDirection.Up) ;
        if (Input.GetKeyDown(KeyCode.DownArrow)) DownSwipe();// HandleSwipe(new Vector3Int(currentPosition.x, currentPosition.y - 1), SwipeDirection.Down) ;
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
        EnemyManager.LoadMap(levelId, levelPack, levelType);
        _level = TilemapManager.LastLoadedLevel;
        _enemy = EnemyManager.LastLoadedEnemy;

        EnemyParentObject.DestroyAllChilds();
        Enemies = new List<Enemy>();
        foreach (var enemyData in _enemy.enemiesDatas)
        {
            var enemyTemplate = ResourcesSupplier<Enemy>.PrefabsSupplier.GetObjectForID("EnemyTemplate");
            var enemyWorldObject = Instantiate(enemyTemplate, EnemyParentObject);
            var enemy = enemyWorldObject.transform.GetChild(0).GetComponent<Enemy>();
            Enemies.Add(enemy);
            InstantlyMoveToPosition(enemyWorldObject.transform, new Vector3Int(enemyData.startPoint.x, enemyData.startPoint.y));
            enemy.data = Helper.DeepClone(enemyData);
        }

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
        if (turnInProgress) return;
        var newTilePosition = currentPosition;
        newTilePosition.y++;
        HandleSwipe(newTilePosition, SwipeDirection.Up);
    }

    public void DownSwipe()
    {
        if (turnInProgress) return;
        var newTilePosition = currentPosition;
        newTilePosition.y--;
        HandleSwipe(newTilePosition, SwipeDirection.Down);
    }

    public void LeftSwipe()
    {
        if (turnInProgress) return;
        var newTilePosition = currentPosition;
        newTilePosition.x--;
        HandleSwipe(newTilePosition, SwipeDirection.Left);
    }

    public void RightSwipe()
    {
        if (turnInProgress) return;
        var newTilePosition = currentPosition;
        newTilePosition.x++;
        HandleSwipe(newTilePosition, SwipeDirection.Right);
    }

    void HandleSwipe(Vector3Int position, SwipeDirection direction)
    {
        if (turnInProgress) return;
        turnInProgress = true;

        var result = MoveToPositionIfCan(position, direction);

        if(result != null)
        {
            if (result == true)
            {
                switch (direction)
                {
                    case SwipeDirection.Up:
                        Hero.UpSwipe();
                        break;
                    case SwipeDirection.Down:
                        Hero.DownSwipe();
                        break;
                    case SwipeDirection.Right:
                        Hero.RightSwipe();
                        break;
                    case SwipeDirection.Left:
                        Hero.LeftSwipe();
                        break;
                }

                currentPosition = position;
                foreach (var enemy in Enemies)
                {
                    enemy.Move();

                }

            }
            else
            {
                Hero.WrongSwipe();

            }
            CheckMove();
        }

       
        turnInProgress = false;
       
    }

    void InstantlyMoveToPosition(Vector3Int position)
    {
        Camera.main.transform.position = new Vector3(position.x, position.y, Camera.main.transform.position.z);
        HeroWorldObject.transform.position = new Vector3(position.x, position.y, 0);
    }

    void MoveToPosition(Vector3Int position)
    {
        StartCoroutine(Move(HeroWorldObject, new Vector3(position.x, position.y , 0)));
        StartCoroutine(Move(Camera.main.gameObject, new Vector3(position.x, position.y , Camera.main.transform.position.z)));
    }

    void InstantlyMoveToPosition(Transform transform, Vector3Int position)
    {
        transform.position = new Vector3(position.x , position.y, 0);
    }

    void MoveToPosition(Transform transform, Vector3Int position)
    {
        StartCoroutine(Move(transform.gameObject, new Vector3(position.x , position.y, 0)));
    }

    bool TileExist(Vector3Int position)
    {
        return _level.GroundTiles.Find(tile => { return tile.Position.x == position.x && tile.Position.y == position.y; }) != null;
    }
    bool? MoveToPositionIfCan(Vector3Int position, SwipeDirection direction)
    {
        if(TileExist(position)) {
            var enemies = Enemies.FindAll(enemy => enemy.Position.x == position.x && enemy.Position.y == position.y);

            var enemy = enemies.Find(enemy =>
            {
                switch (direction)
                {
                    case SwipeDirection.Down:
                        return enemy.NextStepDirection() == SwipeDirection.Up;
                    case SwipeDirection.Up:
                        return enemy.NextStepDirection() == SwipeDirection.Down;
                    case SwipeDirection.Right:
                        return enemy.NextStepDirection() == SwipeDirection.Left;
                    case SwipeDirection.Left:
                        return enemy.NextStepDirection() == SwipeDirection.Right;
                    case SwipeDirection.None:
                        return false;
                }
                return false;

            });

            if(enemy == null)
            {
                MoveToPosition(position);
                return true;
            } else
            {
                enemy.Move();
                Unbind();
                Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Hero.Death();
                return null;
            }

            
        }
        return false;
    }

    bool CheckEnemies()
    {
        return CheckEnemies(currentPosition);
    }

    
    bool CheckEnemies(Vector3 position)
    {
        return Enemies.Find(enemy => enemy.Position.x == position.x && enemy.Position.y == position.y) != null;
    }
    void CheckMove()
    {
        if (CheckEnemies())
        {
            Unbind();
            Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            Hero.Death();
            return;
        }
        var ground = _level.GroundTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });
        var objectTile = _level.ObjectTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });
        var unit = _level.UnitTiles.Find(tile => { return tile.Position.x == currentPosition.x && tile.Position.y == currentPosition.y; });

        var result = moveResultChecker.CheckMove(ground?.Tile, objectTile?.Tile, unit?.Tile);

        switch (result)
        {
            case MoveResult.None:
                Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
            case MoveResult.Finish:
                Unbind();
                Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
                Hero.Win();
                break;
            case MoveResult.Death:
                Unbind();
                Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
                Hero.Death();
                break;
        }

        CheckEnemiesLight();
        Hero.CheckShadow();
    }

    public void CheckEnemiesLight()
    {
        foreach(var enemy in Enemies)
        {
            var dX = Mathf.Abs(currentPosition.x - enemy.Position.x);
            var dY = Mathf.Abs(currentPosition.y - enemy.Position.y);
            var distance = dX + dY;
            switch(distance)
            {
                case 0:
                case 1:
                    enemy.SetLightEnabled(true);
                    break;
                case 2:
                    enemy.SetLightEnabled(CanSeeSecondRadiusObject(currentPosition, new Vector3Int(enemy.Position.x, enemy.Position.y), dX, dY));
                    break;
                default:
                    enemy.SetLightEnabled(false);
                    break;
            }
            
        }
    }

    public bool CanSeeSecondRadiusObject(Vector3Int pos1, Vector3Int pos2, int dX, int dY)
    {
        if (dX == 0)
        {
            if (pos1.y < pos2.y)
            {
                return TileExist(new Vector3Int(pos1.x, pos1.y + 1));
            }
            else
            {
                return TileExist(new Vector3Int(pos1.x, pos1.y - 1));
            }
        }
        else if (dY == 0)
        {
            if (pos1.x < pos2.x)
            {
                return TileExist(new Vector3Int(pos1.x + 1, pos1.y));
            }
            else
            {
                return TileExist(new Vector3Int(pos1.x - 1, pos1.y));
            }
        }
        else
        {
            if (pos1.x < pos2.x)
            {
                if (pos1.y < pos2.y)
                {
                    return TileExist(new Vector3Int(pos1.x + 1, pos1.y)) ||
                        TileExist(new Vector3Int(pos1.x, pos1.y + 1));
                }
                else
                {
                    return TileExist(new Vector3Int(pos1.x + 1, pos1.y)) ||
                         TileExist(new Vector3Int(pos1.x, pos1.y - 1));
                }
            }
            else
            {
                if (pos1.y < pos2.y)
                {
                    return TileExist(new Vector3Int(currentPosition.x - 1, currentPosition.y)) ||
                        TileExist(new Vector3Int(currentPosition.x, currentPosition.y + 1));
                }
                else
                {
                    return TileExist(new Vector3Int(currentPosition.x - 1, currentPosition.y)) ||
                       TileExist(new Vector3Int(currentPosition.x, currentPosition.y - 1));
                }
            }
        }
    }

    IEnumerator Move(GameObject gameObject, Vector3 position)
    {
        while (Mathf.Abs(gameObject.transform.position.sqrMagnitude - position.sqrMagnitude) > 0.25f)
        {
            gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, position, 0.5f);
            yield return new WaitForEndOfFrame();
        }
        gameObject.transform.position = position;
    }

    public void AfterUnitDeathFunction()
    {
        MoveToStart();
        Bind();
        Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        Hero.Idle();
        CheckEnemiesLight();
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
            Hero.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            CheckEnemiesLight();
        } else
        {
            SceneLoader.instance.LoadPrev();
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
