using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour, ISwipeHandler
{
    private ScriptableLevel _level;
    private EnemyScriptableObject _enemy;
    private MoveResultChecker moveResultChecker = new MoveResultChecker();
    private LevelManager levelManager = new LevelManager();

    private Vector3Int currentPosition;
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
            var pos = new Vector3Int(enemy.data.startPoint.x, enemy.data.startPoint.y);
            InstantlyMoveToPosition(enemy.gameObject.transform.parent, pos);
            enemy.currentPointIndex = 0;
        }
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

        var enemySupplier = new ResourcesSupplier<GameObject>("Prefabs");
        var spriteSupplier = new ResourcesSpriteSupplier();
        EnemyParentObject.DestroyAllChilds();
        Enemies = new List<Enemy>();
        foreach (var enemyData in _enemy.enemiesDatas)
        {
            var enemyTemplate = enemySupplier.GetObjectForID("EnemyTemplate");
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
        if (MoveToPositionIfCan(position))
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
                var enemyDirection = enemy.NextStepDirection();
                Debug.Log(enemy.CurrentPos);
                Debug.Log(enemyDirection);
                var pos = enemy.CurrentPos;
                switch (enemyDirection)
                {
                    case SwipeDirection.Up:
                        enemy.UpSwipe();
                        enemy.gameObject.transform.parent.transform.position = new Vector3((int)pos.x, (int)(pos.y + 1));
                        break;
                    case SwipeDirection.Down:
                        enemy.DownSwipe();
                        enemy.gameObject.transform.parent.transform.position = new Vector3((int)pos.x, (int)(pos.y - 1));
                        break;
                    case SwipeDirection.Right:
                        enemy.RightSwipe();
                        enemy.gameObject.transform.parent.transform.position = new Vector3((int)(pos.x + 1), (int)pos.y);
                        break;
                    case SwipeDirection.Left:
                        enemy.LeftSwipe();
                        enemy.gameObject.transform.parent.transform.position = new Vector3((int)(pos.x - 1), (int)pos.y);
                        break;
                    case SwipeDirection.None:
                        break;
                }

            }

        }
        else
        {
            Hero.WrongSwipe();

        }
        CheckMove();
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

    bool MoveToPositionIfCan(Vector3Int position)
    {
        if(_level.GroundTiles.Find(tile => { return tile.Position.x == position.x && tile.Position.y == position.y; }) != null) {
            MoveToPosition(position);
            return true;
        }
        return false;
    }

    bool CheckEnemies()
    {
        return CheckEnemies(currentPosition);
    }

    bool CheckEnemies(Vector3 position)
    {
        return Enemies.Find(enemy => enemy.CurrentPos.x == position.x && enemy.CurrentPos.y == position.y) != null;
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
