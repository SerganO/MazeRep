using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class LevelCell : MonoBehaviour, IPointerDownHandler
{
    private LevelData _attachedData = null;

    public LevelCellInfoItem LevelCellInfoItem;

    public GameObject LeftPath;
    public GameObject RightPath;
    public GameObject TopPath;
    public GameObject BottomPath;

    public GameObject Lock;

    public event StringFunc CellTapped;


    public void SetupForData(LevelData data, int cellIndex, int packLevelsCount, int cellsInRow = 4)
    {
        _attachedData = data;

        LevelCellInfoItem.SetLevelText(data.levelName);
        LevelCellInfoItem.SetStar(data.reachedStars);

        var number = cellIndex + 1;
        var row = cellIndex / cellsInRow + 1;

        bool isLastInRow = number % cellsInRow == 0;
        bool isFirstInRow = number % cellsInRow == 1;
        bool isFirst = cellIndex == 0;
        bool isLast = number == packLevelsCount;


        TopPath.SetActive(isFirstInRow && !isFirst);
        BottomPath.SetActive(isLastInRow && !isLast);

        LeftPath.SetActive(row % 2 == 0 ? (!isLastInRow && !isLast): !isFirstInRow);
        RightPath.SetActive(row % 2 == 0 ? !isFirstInRow : (!isLastInRow && !isLast));

        Lock.SetActive(!data.available);

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_attachedData != null && _attachedData.available)
        {
            MoveToLevel(_attachedData.levelId);
        }
    }

    public void MoveToLevel(string id)
    {
        CellTapped?.Invoke(id);
    }
}

[System.Serializable]
public class LevelData {
    public string levelId;
    public string levelName;
    public bool available;
    public int reachedStars;

}
