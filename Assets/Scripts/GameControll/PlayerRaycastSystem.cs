using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

// Логика поставления фигуры-цели
public class PlayerRaycastSystem : MonoBehaviour {

	public Action <GameObject> UpdateTargetState;
	public GameObject target;
	[HideInInspector]
	public LayerMask targetLayerMask = 9;
    
	private RaycastHit[] hits;
	private Transform choosedBuildingPlaceTransform;

    // Формируем синглтон для быстрого доступа
    public static PlayerRaycastSystem Instance 
    {
        get; private set;
    }

	private void Awake()
	{
        if (Instance == null)
        {
            Instance = this;
        }
	}

	void Update () {
        Transform cam = transform;
        Ray ray = new Ray(cam.position + cam.forward, cam.forward);
        hits = Physics.RaycastAll(ray, 500f, targetLayerMask);


        if (hits.Length > 0)
        {
            //Выполняем сортировку по дистанции луча, так как RaycastAll не гарантирует порядок.
            var sortedHits = from hit in hits
                         orderby hit.distance
                         select hit;

            // Мы можем ставить предметы только на верхнюю сторону объекта. 
            // Ищем первую попавшуюся такую сторону
            for (int i = 0; i < sortedHits.Count(); i++)
            {
                RaycastHit currentElement = sortedHits.ElementAt<RaycastHit>(i);
                if (IsTopSide(currentElement))
                {
					float targetMeshHeight = target.GetComponent<BuildingComponent>().MeshHeight;
                    Vector3 hitMeshPosition = currentElement.transform.position;
                    float hitMeshPosY = hitMeshPosition.y;

                    // Фигуры размещаются в специальных областях на других фигурах с соотв тегом
                    // Если луч попадает на такую область - фиксируем её на той же позиции и даём тот же поворот
                    // Если иначе - никак не фиксируем
                    if (currentElement.transform.tag == "BuildingPlace")
                    {
                        float yTargetPosition = targetMeshHeight / 2f + hitMeshPosY;
                        Quaternion hitMeshRotation = currentElement.transform.rotation;
                        target.transform.position = new Vector3(hitMeshPosition.x, yTargetPosition, hitMeshPosition.z);
                        target.transform.rotation = hitMeshRotation * Quaternion.Euler(0f,90f,0f);
                    }
                    else
                    {
                        float hitMeshHeight = currentElement.transform.GetComponent<BuildingComponent>().MeshHeight;
                        Vector3 pointPosition = currentElement.point;
                        float yTargetPosition = targetMeshHeight / 2f + hitMeshHeight / 2f + hitMeshPosY;
						target.transform.position = new Vector3(pointPosition.x, yTargetPosition, pointPosition.z);
                    }

                    // Вызов делегата обновления состояния цели
                    // Организация взаимодействия компонентов через подписки на делегаты подобным образом
                    //  позволяет сделать систему более раширяемой
                    if (UpdateTargetState != null)
                    {
                        UpdateTargetState(currentElement.collider.gameObject);
                    }
                    break;
                }
            }
        }
	}

    // Критерий верхней стороны - вектор нормали от точки нашего луча 
    // и локальный вектор (0,1,0) у объекта должны быть коллениарны
    private bool IsTopSide(RaycastHit hit)
    {
        Vector3 hitNormal = hit.normal;
        float dotUp = Vector3.Dot(hitNormal,hit.transform.up);

        return dotUp >= 0.99f ? true : false;
    }
}
