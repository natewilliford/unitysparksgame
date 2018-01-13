using GameSparks.Core;
using System.Collections.Generic;


public class GameData {

    private List<Building> buildings = new List<Building>();
    
    // public void SetBuildingsFromGSData(List<GSData> dataList) {
    //     List<Building> buildingsList = new List<Building>();
    //     foreach(GSData data in dataList) {
    //         Building building = (Building)GSDataHelpers.GSDataToObject(data);
    //         buildingsList.Add(building);
    //     }
    //     buildings = buildingsList;
    // }

    // public void AddBuildingFromGSData(GSData buildingGSData) {
    //     Building building = (Building)GSDataHelpers.GSDataToObject(buildingGSData);
    //     buildings.Add(building);
    // }

    public void AddBuilding(Building building) {
        buildings.Add(building);
    }
}