using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// Вспомогательный класс для кнопок строительства. Нужен для более удобной передачи данных при нажатии.
//
//  В более большом проекте я скорее всего бы делал родительский класс для кнопок, в котором описывал общее 
//      поведение, а в таких классах дописывал поведение относящееся к выбранному типу кнопок, но в данном
//          проекте решил этого не делать
public class BulidingButtons : MonoBehaviour {

    public BuildingTypes buildingTypes;
    public Action<BuildingTypes> onBuildingButtonPress;

    void Start ()
    {
        GetComponent<Button>().onClick.AddListener(BuildingChoose);
    }

    public void BuildingChoose()
    {
        if (onBuildingButtonPress != null)
        {
            onBuildingButtonPress(buildingTypes);
        }
    }
}
