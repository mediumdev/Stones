using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _obstacle; //Ссылка на препятствие

    public event Action<Circle> onCircleTrigger; //Событие если шар попал в зону триггера и остановился
    public Circle circle { get; private set; } //Ссылка на шар который вошёл в зону триггера
    public int patternsCount { get; private set; } //Число шаблонов с комбинацией триггеров в которых есть этот триггер

    void Start()
    {
        if (_obstacle)
        {
            _obstacle.SetActive(false); //Деактивируем препятствие
        }
    }

    public void SetCircle(Circle circle)
    {
        this.circle = circle; //Задаём ссылку на шар

        if (_obstacle)
        {
            bool showObstacle = circle; //Если ссылка на шар не нулевая то показываем препятствие
            _obstacle.SetActive(showObstacle); //Активируем препятствие
        }

        if (!circle)
        {
            return;
        }    

        onCircleTrigger?.Invoke(circle); //Вызываем событие при котором шар попадает в зону триггера и передаём этот шар
    }

    public void IncreasePattern()
    {
        patternsCount++; //Добавляем шаблон с комбинациями в которых есть этот триггер
    }
}
