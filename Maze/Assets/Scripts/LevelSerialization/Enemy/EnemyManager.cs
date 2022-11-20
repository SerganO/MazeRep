using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private EnemyScriptableObject _lastLoadedEnemy;
    private ResourcesSupplier<EnemyScriptableObject> enemySupplier = new ResourcesSupplier<EnemyScriptableObject>("Enemies");
    private ResourcesSupplier<GameObject> resourcesSupplier = new ResourcesSupplier<GameObject>("Prefabs");

    public Transform EnemiesTransform;

    public string levelPack = "Base";
    public string levelId;
    private List<EnemyData> enemiesDatas = new List<EnemyData>();

    public void ConvertObjectToDatas()
    {
        enemiesDatas = new List<EnemyData>();
        for (int i = 0; i < EnemiesTransform.childCount; i++)
        {
            var enemy = EnemiesTransform.GetChild(i).gameObject.GetComponent<EnemyDevObject>();

            if(enemy != null)
            {
                var data = new EnemyData();
                data.enemyId = enemy.enemyId;
                var startPointPosition = enemy.startPoint.position;
                data.startPoint = new Position((int)startPointPosition.x, (int)startPointPosition.y);

                foreach (var point in enemy.pathPoints)
                {
                    var pointPosition = point.position;
                    data.path.Add(new Position((int)pointPosition.x, (int)pointPosition.y));
                }

                enemiesDatas.Add(Helper.DeepClone(data));
            }
        }
        
    }

    public EnemyScriptableObject LastLoadedEnemy
    {
        get
        {
            return _lastLoadedEnemy;
        }
    }

    public void ClearMapDev()
    {
        EnemiesTransform.DestroyImmediateAllChilds();
        enemiesDatas = new List<EnemyData>();
    }
    public void ClearMap()
    {
        EnemiesTransform.DestroyAllChilds();
        enemiesDatas = new List<EnemyData>();
    }
    public void SaveMap()
    {
        SaveMap(levelId, levelPack);
    }

    public void SaveMap(string levelId)
    {
        ConvertObjectToDatas();
        var newEnemies = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        newEnemies.enemiesDatas = Helper.DeepClone(enemiesDatas);
        newEnemies.name = $"Level {levelId}";
#if UNITY_EDITOR
        ScriptableEnemyObjectUtility.SaveEnemyFile(newEnemies);
#endif
    }

    public void SaveMap(string levelId, string levelPack)
    {
        ConvertObjectToDatas();
        var newEnemies = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        newEnemies.enemiesDatas = Helper.DeepClone(enemiesDatas);
        newEnemies.name = $"Level {levelId}";
#if UNITY_EDITOR
        ScriptableEnemyObjectUtility.SaveEnemyFile(newEnemies, levelPack);
#endif
    }


    public void LoadMap()
    {
        LoadMap(levelId, levelPack);
    }

    public void LoadDevObjects()
    {
        LoadDevObjects(levelId, levelPack);
    }

    public void LoadMap(string levelId, string levelPack = "", string levelType = "")
    {
        var enemy = levelPack == "" ? enemySupplier.GetObjectForID($"Level {levelId}") :
           enemySupplier.GetObjectForID($"Level {levelId}", levelPack);
        if (enemy == null)
        {
            Debug.LogError($"Level {levelId} does not exist.");
            return;
        }

        ClearMap();

        _lastLoadedEnemy = enemy;
    }

    public void LoadDevObjects(string levelId, string levelPack = "", string levelType = "")
    {
        LoadMap(levelId, levelPack, levelType);
        foreach (var enemyData in _lastLoadedEnemy.enemiesDatas)
        {
            var enemyTemplate = resourcesSupplier.GetObjectForID("EnemyDevTemplate");
            var enemyWorldObject = Instantiate(enemyTemplate, EnemiesTransform);

            enemyWorldObject.transform.position = enemyData.startPoint.Vector3;
            var devObject = enemyWorldObject.GetComponent<EnemyDevObject>();
            var startPoint = resourcesSupplier.GetObjectForID("StartPoint");
            var startPointWorldObject = Instantiate(startPoint, enemyWorldObject.transform);
            startPointWorldObject.transform.position = enemyData.startPoint.Vector3;
            devObject.startPoint = startPointWorldObject.transform;
            devObject.pathPoints = new List<Transform>();
            foreach (var point in enemyData.path)
            {
                var pathPoint = resourcesSupplier.GetObjectForID("Point");
                var pathPointWorldObject = Instantiate(pathPoint, enemyWorldObject.transform);
                pathPointWorldObject.transform.position = point.Vector3;
                devObject.pathPoints.Add(pathPointWorldObject.transform);
            }
        }
    }

}

#if UNITY_EDITOR

public static class ScriptableEnemyObjectUtility
{
    public static void SaveEnemyFile(EnemyScriptableObject enemy)
    {
        AssetDatabase.CreateAsset(enemy, $"Assets/Resources/Enemies/_{enemy.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void SaveEnemyFile(EnemyScriptableObject enemy, string packName)
    {
        if (!Directory.Exists($"Assets/Resources/Enemies/{packName}"))
        {
            Directory.CreateDirectory($"Assets/Resources/Enemies/{packName}");
        }
        AssetDatabase.CreateAsset(enemy, $"Assets/Resources/Enemies/{packName}/{enemy.name}.asset");

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

#endif