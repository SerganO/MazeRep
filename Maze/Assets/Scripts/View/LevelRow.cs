using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelRow : MonoBehaviour
{
    public event StringFunc CellTapped;
    public void SetupRowForData(List<LevelData> data, int rowIndex, int cellsInRow = 4)
    {
        var startIndex = cellsInRow * rowIndex;
        var finishIndex = startIndex + cellsInRow;

        var actualFinishIndex = Mathf.Min(finishIndex, data.Count);

        for(int i = startIndex; i < actualFinishIndex; i++)
        {
            var cellTemplate = ResourcesSupplier<GameObject>.PrefabsSupplier.GetObjectForID("LevelCell");
            var cellObject = Instantiate(cellTemplate, transform);
            var cell = cellObject.GetComponent<LevelCell>();
            cell.SetupForData(data[i], i, data.Count, cellsInRow);
            cell.CellTapped += Cell_CellTapped;
        }

        if(finishIndex > actualFinishIndex)
        {
            for(int i = actualFinishIndex; i< finishIndex; i++)
            {
                var cellTemplate = ResourcesSupplier<GameObject>.PrefabsSupplier.GetObjectForID("EmptyLevelCell");
                Instantiate(cellTemplate, transform);
            }
        }


    }

    private void Cell_CellTapped(string value)
    {
        CellTapped?.Invoke(value);
    }
}
