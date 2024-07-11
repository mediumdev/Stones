using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pendulum;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Pendulum _pendulum; //Ссылка на маятник

    [SerializeField]
    private TriggerContainer _triggerContainer; //Ссылка на контейнер с триггерами и стенками

    [SerializeField]
    private TMPro.TextMeshProUGUI _pointsText; //Ссылка на текст с количеством очков

    [SerializeField]
    private GameObject _circlePrefab; //Ссылка на префаб шара

    [SerializeField]
    private List<CircleColor> _circleColors; //Список используемых шаров и очков которые за них дают

    private Circle currentCircle = null; //Шар на маятнике

    private bool dropActive = true; //Можно ли скинуть шар

    private int circleCount = 0; //Количество шаров в игре

    private int points = 0; //Количество очков

    [Serializable]
    public struct CircleColor //Структура данных о цвете
    {
        public Color color; //Цвет
        public int points; //Количество очков
    }

    void Start()
    {
        if (!_triggerContainer)
        {
            return;
        }

        SpawnSphere(); //Спавним шар

        _triggerContainer.onPatternsCheck += OnPatternsCheck; //Подписываемся на событие после проверки комбинаций
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) //При касании экрана или нажатии мыши
        {
            if (dropActive) //Если можем скинуть шар
            {
                DropSphere(); //Скидываем

                dropActive = false; //Скидывать больше не можем
            }
        }
    }

    private void OnPatternsCheck()
    {
        if (circleCount < _triggerContainer.triggerCount) //Если шаров меньше чем триггеров
        {
            SpawnSphere(); //Спавним шар на маятнике
        }
        else //Иначе
        {
            GameEnd(); //Конец игры
        }
    }

    private void SpawnSphere()
    {
        if (currentCircle)
        {
            return;
        }

        if (!_pendulum)
        {
            return;
        }

        if (!_triggerContainer)
        {
            return;
        }

        if (!_circlePrefab)
        {
            return;
        }

        currentCircle = Instantiate(_circlePrefab, _pendulum.GetSphereSpawn()).GetComponent<Circle>(); //Создаём экземпляр шара и сохраняем ссылку на него
        CircleColor randomColor = GetRandomColor(); //Находим случайный цвет из возможных
        currentCircle.SetCircleColor(randomColor); //Задаём цвет
        currentCircle.GetComponent<Rigidbody2D>().isKinematic = true; //Выключаем физику шара
        currentCircle.onRemove += OnCircleRemove; //Подписываемся на событие уничтожения шара

        circleCount++; //Увеличиваем счётчик шаров

        dropActive = true; //Можно скидывать
    }

    private void GameEnd()
    {
        if (GameManager.instance)
        {
            GameManager.pendulumPoints = points; //Сохраняем количество очков в GameManager
            GameManager.LoadLevel("Replay"); //Переходим на экран рестарта
        }
    }

    private void OnCircleRemove(Circle circle)
    {
        circleCount--; //Уменьшаем счётчик шаров

        points += circle.circleColor.points; //Увеличиваем количество очков

        if (_pointsText)
        {
            _pointsText.text = points.ToString(); //Отображаем количество очков
        }

        circle.onRemove -= OnCircleRemove; //Отписываемся от события уничтожения шара
    }

    private void DropSphere()
    {
        if (!currentCircle)
        {
            return;
        }

        currentCircle.Drop(); //Скидываем шар
        currentCircle.onStay += CurrentSphereStay; //Подписываемся на событие остановки шара
    }

    private void CurrentSphereStay()
    {
        if (!currentCircle)
        {
            return;
        }

        currentCircle.onStay -= CurrentSphereStay; //Отписываемся на событие остановки шара
        currentCircle = null; //Обнуляем ссылку шара на маятнике
    }

    private CircleColor GetRandomColor()
    {
        if (_circleColors.Count > 0)
        {
            return _circleColors[Random.Range(0, _circleColors.Count)]; //Выбираем рандомный цвет
        }

        return new CircleColor(); //Возвращаем его
    }

    void OnDestroy()
    {
        _triggerContainer.onPatternsCheck -= OnPatternsCheck; //Отписываемся от события после проверки комбинаций
    }
}
