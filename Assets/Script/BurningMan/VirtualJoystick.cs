using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private RectTransform joystickTransform; //Ссылка на RectTransform джойстика

    [SerializeField]
    private float _dragThreshold = 0.6f; //Порог после которого игрок начнёт движение

    [SerializeField]
    private int _dragMovementDistance = 30; //Расстояние на которое перемещается джойстик

    [SerializeField]
    private int _dragOffsetDistance = 100; //Максимальное смещение джойстика от центра (начала координат)

    public event Action<Vector2> onMove; //Событие движения
    public Vector3 inputDirection { get; private set; } //Хранит значение на которые сместился джойстик
    public bool jump { get; private set; } //Нажата кнопка прыжка или нет

    void Start() 
    {
        ControlManager._virtualJoystick = this; //Передаём ссылку на этот джойстик в контроллер

        inputDirection = Vector3.zero; //Обнуляем значения смещения джойстика
    }

    public void OnDrag(PointerEventData eventData) //Во время события перетаскивания
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, null, out offset); //Смещение указателя относительно джойстика
        offset = Vector2.ClampMagnitude(offset, _dragOffsetDistance) / _dragOffsetDistance; //Преобразуем значение в от -1 до 1
        joystickTransform.anchoredPosition = offset * _dragMovementDistance; //Перемещаем джойстик 

        inputDirection = CalculateMovementInput(offset); //Записываем направление движения
    }

    public void OnPointerDown(PointerEventData eventData) //Необходим обработчик нажатия указателя, чтобы срабатывало обработчик отжатия :)
    {

    }

    public void OnPointerUp(PointerEventData eventData) //Событие когда отпускаем указатель
    {
        joystickTransform.anchoredPosition = Vector2.zero; //Перемещаем джойстик в координаты "якоря" (Позиция которую мы установили в инспекторе)
        inputDirection = Vector2.zero; //Обнуляем направление движения
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > _dragThreshold ? offset.x : 0; //Рассчитываем силу движения по X от -1 до 1 относительно порога
        float y = Mathf.Abs(offset.y) > _dragThreshold ? offset.y : 0; //Рассчитываем силу движения по Y от -1 до 1 относительно порога
        return new Vector2(x, y); //Возвращаем значение
    }

    public void OnJumpPressed()
    {
        jump = true; //Кнопка прыжка нажата
    }

    public void OnJumpReleased()
    {
        jump = false; //Кнопка прыжка отпущена
    }
}