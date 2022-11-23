using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsScreenHandler : BaseHandler
{
    private LevelPackData SelectedPack;
    public LevelsList LevelsList;
    public DLCListScript DLCList;

    private void Awake()
    {
        DLCList.SelectedIndexChanged += DLCList_SelectedIndexChanged;
        LevelsList.CellTapped += LevelsList_CellTapped;
    }

    private void LevelsList_CellTapped(string value)
    {
        var currentContext = GetCurrentContext();
        currentContext.levelId = value;
        currentContext.packId = SelectedPack.packId;
        LoadLevelScreen();

    }

    private void DLCList_SelectedIndexChanged(LevelPackData value)
    {
        var data = LevelPackManager.sharedInstance.levelsData[value.packId];
        LevelsList.SetupForData(value.available, data);
        SelectedPack = value;
    }

    private void OnDestroy()
    {
        DLCList.SelectedIndexChanged -= DLCList_SelectedIndexChanged;
        LevelsList.CellTapped -= LevelsList_CellTapped;
    }

    void LoadLevelScreen()
    {
        SceneLoader.instance.LoadScene("LevelScene");
    }
}
