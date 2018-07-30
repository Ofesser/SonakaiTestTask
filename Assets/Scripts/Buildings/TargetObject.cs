using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Логика отображения состояния таргетного объекта
public class TargetObject : BuildingComponent {

    [HideInInspector]
	public GameObject availableTargetPlace;
    private readonly Color wrongPositionColor = Color.red;
    private readonly Color correctPositionColor = Color.green;

    // Помимо стандартной логики вычисления параметров объекта, для таргета мы подписываемся на делегат, который
    //  вызывается по апдейту и передаёт объект, на который в данный момент попадает луч.
	protected override void Start()
	{
        base.Start();
        PlayerRaycastSystem.Instance.UpdateTargetState = CheckTargetState;
	}

    private void CheckTargetState(GameObject targetPlace)
    {
        Material targetMat = GetComponent<Renderer>().material;

        // Если луч попадает на ячейку для строительства то проверяем на возможность строительства.
        //  Если нет, то строить нельзя.
        if (targetPlace.tag == "BuildingPlace")
        {
            if (targetPlace.GetComponent<BuildingCollider>().colliderType != buildingType ||
                targetPlace.GetComponent<BuildingCollider>().placeOccupied)
            {
                if (targetMat.color != wrongPositionColor)
                {
                    targetMat.SetColor("_Color", wrongPositionColor);
                    availableTargetPlace = null;
                }
            }
            else
            {
                if (targetMat.color != correctPositionColor)
                {
                    targetMat.SetColor("_Color", correctPositionColor);
                    availableTargetPlace = targetPlace;
                }
            }
        }
        else
        { 
            if (targetMat.color != wrongPositionColor)
            {
                targetMat.SetColor("_Color", wrongPositionColor);
                availableTargetPlace = null;
            }
        }
    }
}
