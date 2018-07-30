using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Логика выбора объекта и строительства
public class Builder : MonoBehaviour
{

    [SerializeField]
    private Material targetMaterial;
    [SerializeField]
    private Material buildingMaterial;

    [SerializeField]
    private Button buildButton;
    [SerializeField]
    private Button[] figureButton;

    [SerializeField]
    private GameObject[] availableBuilding;



    #if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN)
    // Делаем список из доступных кнопок для выбора фигуры. Возможно не самый правильный подход для этой задачи
    //  но по крайней мере таким образом можно легко добавить новые объекты и выбор для них настроится автоматически
    private KeyCode[] numberButtons = {KeyCode.Alpha1, KeyCode.Alpha2,KeyCode.Alpha3,
        KeyCode.Alpha4,KeyCode.Alpha5,KeyCode.Alpha6,KeyCode.Alpha7,KeyCode.Alpha8,KeyCode.Alpha9};
    #endif

    void Start()
    {
        // Управление выбором для андройда
    #if UNITY_ANDROID
        buildButton.onClick.AddListener(BuildObject);
		// Нам необходимо передавать информацию при нажатии на кнопку выбора, в данном случае этой
		//  информацией является тип объекта. Для этого вводим вспомогательный класс BuldingButtons.
		//    в будущем его можно будет использовать для описания кастомного поведения при выборе объекта
        foreach (Button button in figureButton)
        {
            button.GetComponent<BulidingButtons>().onBuildingButtonPress += ChangeBuilding;
        }
    #endif
	}

    // Управление выбором для компьютера
    #if (UNITY_STANDALONE_OSX || UNITY_STANDALONE_WIN)
    private void Update()
    {
        int availableBuldingsLenght = availableBuilding.Length;
        for (int i = 0; i < availableBuldingsLenght; i++)
        {
            if (Input.GetKeyDown(numberButtons[i]))
            {
                ChangeBuilding((BuildingTypes)i);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            BuildObject();
        }

    }
    #endif


	public void ChangeBuilding(BuildingTypes buildingType)
    {
        Destroy(PlayerRaycastSystem.Instance.target);
        GameObject newTarget = Instantiate(availableBuilding[(int)buildingType]);
        newTarget.layer = PlayerRaycastSystem.Instance.targetLayerMask;
        Destroy(newTarget.GetComponent<BuildingComponent>());
        newTarget.AddComponent<TargetObject>();
        newTarget.GetComponent<TargetObject>().buildingType = buildingType;
        newTarget.GetComponent<MeshRenderer>().material = targetMaterial;
        PlayerRaycastSystem.Instance.target = newTarget;
    }

    private void BuildObject()
    {
        TargetObject targetObject = PlayerRaycastSystem.Instance.target.GetComponent<TargetObject>();
        if (targetObject.availableTargetPlace != null)
        {
			targetObject.availableTargetPlace.GetComponent<BuildingCollider>().placeOccupied = true;
            GameObject newBuildObject = Instantiate(availableBuilding[(int)targetObject.buildingType]);
            newBuildObject.transform.position = targetObject.transform.position;
            newBuildObject.transform.rotation = targetObject.transform.rotation;
            newBuildObject.GetComponent<MeshRenderer>().material = buildingMaterial;

        }
    }
}