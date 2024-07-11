using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static GameController;

public class TriggerContainer : MonoBehaviour
{
    [SerializeField]
    private List<TriggerPattern> _triggerPatterns; //Ссылки на объекты шаблонов с комбинациями

    public event Action onPatternsCheck; //Событие после проверки всех комбинаций

    private List<TriggerPattern> checkPatterns; //Список проверенных комбинаций

    public int triggerCount { get; private set; } //Количество всех триггеров

    void Start()
    {
        checkPatterns = new List<TriggerPattern>(); //Создаём список

        foreach (var patterns in _triggerPatterns) //Проходим по всем комбинациям
        {
            patterns.onPatternCheck += OnPatternCheck; //Подписываемся на событие после проверки комбинации
        }

        CircleTrigger[] CircleTriggers = GetComponentsInChildren<CircleTrigger>(); //Создаём массив со всеми триггерами
        triggerCount = CircleTriggers.Length; //Записываем количество триггеров
    }

    private void OnPatternCheck(TriggerPattern pattern, CircleTrigger trigger)
    {
        checkPatterns.Add(pattern); //Добавляем в список проверенную комбинацию

        if (checkPatterns.Count >= trigger.patternsCount) //Если все комбинации в котором есть триггер проверены
        {
            foreach (var checkPattern in checkPatterns) //Проходим по всем проверенным комбинациям
            {
                if (checkPattern.complete) //Если комбинация собрана
                {
                    checkPattern.RemoveCircles(); //Уничтожаем в ней все шары
                }
            }

            onPatternsCheck?.Invoke(); //Вызываем событие проверки комбинаций
            checkPatterns.Clear(); //Очищаем список комбинаций
        }
    }

    void OnDestroy()
    {
        foreach (var patterns in _triggerPatterns) //Проходим по всем комбинациям
        {
            patterns.onPatternCheck -= OnPatternCheck; //Отписываемся от события после проверки комбинации
        }
    }
}
