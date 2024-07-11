using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class TriggerPattern : MonoBehaviour
{
    [SerializeField]
    private List<CircleTrigger> triggers; //Список триггеров в этом шаблоне комбинаций

    public event Action<TriggerPattern, CircleTrigger> onPatternCheck; //Событие вызываемое после проверки комбинации

    public bool complete { get; private set; } = false; //Собрана ли комбинация

    void Start()
    {
        foreach (var trigger in triggers) //Проходим по всем триггерам в шаблоне
        {
            trigger.onCircleTrigger += OnCircleTrigger; //Подписываемся на событие входа шара в зону триггера
            trigger.IncreasePattern(); //Повышаем счетчик шаблонов у триггера
        }
    }

    private void OnCircleTrigger(Circle circle)
    {
        if (triggers == null || triggers.Count == 0)
        {
            return;
        }

        Color val = circle.circleColor.color; //Сохраняем цвет шара который вошёл в зону триггера и остановился

        //Если на всех триггерах в шаблоне находятся шары и они одного цвета
        if (!triggers.Any(x => x.circle == null) && triggers.All(x => x.circle.circleColor.color == val)) 
        {
            complete = true; //Комбинация собрана
        }

        onPatternCheck?.Invoke(this, circle.trigger); //Вызываем событие что комбинация проверена (Не важно собрана или нет)
    }

    public void RemoveCircles()
    {
        foreach (var trigger in triggers) //Проходим по всем триггерам
        {
            if (trigger.circle) //Если на нём есть шар
            {
                trigger.circle.Remove(); //Уничтожаем его
            }
        }

        complete = false; //Комбинация не собрана
    }

    void OnDestroy()
    {
        foreach (var trigger in triggers) //Для всех триггеров
        {
            trigger.onCircleTrigger -= OnCircleTrigger; //Отписываемся на событие входа шара в зону триггера
        }
    }
}
