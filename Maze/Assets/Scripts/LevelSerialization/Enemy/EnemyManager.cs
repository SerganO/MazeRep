using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    private EnemyScriptableObject _lastLoadedEnemy;
    private ResourcesSupplier<EnemyScriptableObject> enemySupplier = new ResourcesSupplier<EnemyScriptableObject>("Enemies");

    public Transform EnemiesTransform;

    [SerializeField] private string _levelPack = "Base";
    [SerializeField] private string _levelId;
    public List<EnemyData> enemiesDatas = new List<EnemyData>();

    public List<EnemyDevObject> enemiesDevObjects = new List<EnemyDevObject>();

    public EnemyScriptableObject scriptableObject;


    public void ConvertObjectToDatas()
    {
        enemiesDatas = new List<EnemyData>();
        Debug.Log(enemiesDevObjects.Count);
        foreach(var enemy in enemiesDevObjects)
        {
            var data = new EnemyData();
            data.enemyId = enemy.enemyId;
            var startPointPosition = enemy.startPoint.position;
            data.startPoint = new Position((int)startPointPosition.x, (int)startPointPosition.y);

            foreach(var point in enemy.pathPoints)
            {
                var pointPosition = point.position;
                data.path.Add(new Position((int)pointPosition.x, (int)pointPosition.y));
            }

            enemiesDatas.Add(Helper.DeepClone(data));
        }

        var newEnemies = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        newEnemies.enemiesDatas = Helper.DeepClone(enemiesDatas);
        newEnemies.name = $"Level {_levelId}";
#if UNITY_EDITOR
        ScriptableEnemyObjectUtility.SaveEnemyFile(newEnemies);
#endif
        
    }

    public void ConvertScriptableObjectToDatas()
    {
        Debug.Log(scriptableObject.enemiesDatas.Count);
        enemiesDatas = Helper.DeepClone(scriptableObject.enemiesDatas);
    }

    public EnemyScriptableObject LastLoadedEnemy
    {
        get
        {
            return _lastLoadedEnemy;
        }
    }

    public void ClearMap()
    {
        EnemiesTransform.DestroyAllChilds();
        enemiesDatas = new List<EnemyData>();
    }
    public void SaveMap()
    {
        SaveMap(_levelId, _levelPack);
    }

    public void SaveMap(string levelId)
    {
        var newEnemies = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        newEnemies.enemiesDatas = Helper.DeepClone(enemiesDatas);
        newEnemies.name = $"Level {levelId}";
#if UNITY_EDITOR
        ScriptableEnemyObjectUtility.SaveEnemyFile(newEnemies);
#endif
    }

    public void SaveMap(string levelId, string levelPack)
    {
        var newEnemies = ScriptableObject.CreateInstance<EnemyScriptableObject>();

        newEnemies.enemiesDatas = Helper.DeepClone(enemiesDatas);
        newEnemies.name = $"Level {levelId}";
#if UNITY_EDITOR
        ScriptableEnemyObjectUtility.SaveEnemyFile(newEnemies, levelPack);
#endif
    }


    public void LoadMap()
    {
        LoadMap(_levelId, _levelPack);
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