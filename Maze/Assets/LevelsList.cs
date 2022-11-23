using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsList : MonoBehaviour
{
    public RectTransform Content;
    public GameObject Lock;

    public event StringFunc CellTapped;
    public void SetupForData(bool packAvaiable, List<LevelData> data, int cellsInRow = 4)
    {
        Content.DestroyAllChilds();
        if(packAvaiable)
        {
            var rowsCount = data.Count / cellsInRow;
            if (data.Count % cellsInRow != 0) rowsCount++;

            var rowTempate = ResourcesSupplier<GameObject>.PrefabsSupplier.GetObjectForID("LevelRow");
            for (int i = 0; i < rowsCount; i++)
            {
                var rowObject = Instantiate(rowTempate, Content);
                var row = rowObject.GetComponent<LevelRow>();
                row.SetupRowForData(data, i, cellsInRow);
                row.GetComponent<HorizontalLayoutGroup>().reverseArrangement = (i + 1) % 2 == 0;
                row.CellTapped += Row_CellTapped;
            }
        }
        Lock.SetActive(!packAvaiable);

    }

    private void Row_CellTapped(string value)
    {
        CellTapped?.Invoke(value);
    }
}
