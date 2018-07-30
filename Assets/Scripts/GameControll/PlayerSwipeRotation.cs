using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

// Система вращения персонажа с помощью свапа по экрану
public class PlayerSwipeRotation : MonoBehaviour {

    private Vector3 firstpoint; //Стартовая позиция тача
    private Vector3 secondpoint; // Последняя позиция тача
    private float xAngle = 0.0f; // угол поворота
    private float yAngle = 0.0f;
    private float xAngTemp = 0.0f; // накопленное смещение от стартовой позиции
    private float yAngTemp = 0.0f;
    private bool rotationStarted;
    [SerializeField]
    private SwipeUIController swipeUIController;

    void Update()
    {
            if (swipeUIController.touchPressed)
            {
                Vector2 touchPos = swipeUIController.touchPosition;
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = touchPos;

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                // Смотрим верхний элемент из тех что мы поймали лучом.
                // Если это не бекграунд то игнорируем
                if (results[0].gameObject.tag == "UIBack")
                {
                    // Информация о стартовом положении
                    if (!rotationStarted)
                    {
                        firstpoint = touchPos;
                        xAngTemp = xAngle;
                        yAngTemp = yAngle;
                        rotationStarted = true;
                    }
                    
                    // Логика вычисления поворота
                    secondpoint = touchPos;
                    
                    xAngle = xAngTemp + (secondpoint.x - firstpoint.x) * 180.0f / Screen.width;
                    yAngle = yAngTemp - (secondpoint.y - firstpoint.y) * 90.0f / Screen.height;

                    this.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
                }
            }
            else
            {
                rotationStarted = false;
            }
    }
}
