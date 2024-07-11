using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameController;

public class Circle : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionEffect; //Ссылка на эффект взрыва

    [SerializeField]
    private float _appearSpeed; //Скорость появления

    public event Action onStay; //Событие если шар остановился
    public event Action<Circle> onRemove; //Событие если шар исчез

    public CircleTrigger trigger { get; private set; } //Ссылка на триггер в зону которого вошёл круг и остановился

    public CircleColor circleColor { get; private set; } //Ссылка CircleColor, хранит цвет и количество очков


    private SpriteRenderer spriteRenderer = null; //Ссылка на спрайт
    private Rigidbody2D rb = null; //Ссылка на физическое тело

    private bool isFalling = false; //Падает или нет
    private float alpha = 0f; //Прозрачность

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //Сохраняем спрайт
        rb = GetComponent<Rigidbody2D>(); //Сохраняем физическое тело
    }

    void Update()
    {
        Appear(); //Показываем появление шара
    }

    void FixedUpdate()
    {
        if (!rb)
        {
            return;
        }

        if (!isFalling) //Если шар не падает
        {
            if (rb.velocity.magnitude > 1f) //Но шар движется
            {
                isFalling = true; //Значит он падает
            }
        }
        else //Если шар падает
        {
            if (rb.velocity.magnitude < 0.001f && !rb.isKinematic) //Скорость движения очень мала или её нет
            {
                rb.velocity = Vector2.zero; //Обнуляем скорость движения на всякий случай
                rb.angularVelocity = 0f; //И обнуляем скорость вращения
                isFalling = false; //Шар не падает

                onStay?.Invoke(); //Вызываем событие остановки шара

                if (trigger) //Если вошли в зону триггер
                {
                    trigger.SetCircle(this); //Передаём ему ссылку на этот шар
                }
            }
        }
    }

    public void SetCircleColor(CircleColor circleColor)
    {
        if (!spriteRenderer)
        {
            return;
        }

        this.circleColor = circleColor; //Задаём CircleColor

        spriteRenderer.color = circleColor.color; //Сразу меняем цвет шара
    }

    public void Drop()
    {
        if (!rb)
        {
            return;
        }

        transform.parent = null; //Забываем родителя
        rb.isKinematic = false; //Включаем физику
    }

    public void Remove()
    {
        if (_explosionEffect)
        {
            GameObject explosionEffect = Instantiate(_explosionEffect, transform.position, Quaternion.identity) as GameObject; //Создаём экземпляр эффекта взрыва
            explosionEffect.transform.position = transform.position; //Перемещаем эффект на координаты шара
            var main = explosionEffect.gameObject.GetComponent<ParticleSystem>().main; //Сохраняем главный модуль эффекта
            main.startColor = spriteRenderer.color; //Меняем его цвет на цвет спрайта шара
        }

        if (trigger)
        {
            trigger.SetCircle(null); //Удаляем у триггера ссылку на шар
        }

        onRemove?.Invoke(this); //Вызываем событие удаления шара

        Destroy(gameObject); //Удаляем шар
    }

    private void Appear()
    {
        if (alpha < 1f)
        {
            alpha += Time.deltaTime * _appearSpeed; //Уменьшение прозрачности шара
        }
        else
        {
            alpha = 1f;
        }

        Color color = spriteRenderer.color; //Сохраняем цвет шара
        color.a = alpha; //Задаём значение альфа-канала
        spriteRenderer.color = color; //Применяем цвет
    }

    private void OnTriggerEnter2D(Collider2D collision) //Если сработало событие входа в зону триггера
    {
        trigger = collision.GetComponent<CircleTrigger>(); //Сохраняем триггер
    }

    private void OnTriggerExit2D(Collider2D collision) //Если сработало событие выхода из зоны триггера
    {
        CircleTrigger circleTrigger = collision.GetComponent<CircleTrigger>();

        if (circleTrigger)
        {
            circleTrigger.SetCircle(null); //Обнуляем у триггера ссылку на шар
        }
    }
}
