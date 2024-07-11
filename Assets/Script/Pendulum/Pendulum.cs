using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pendulum : MonoBehaviour
{
    [SerializeField]
    private Transform _sphereSpawn; //Ссылка на трансформ спавна, на его координатах появляется шар

    [SerializeField]
    private float _speed; //Скорость движения маятника

    [SerializeField]
    private float _limit; //Ограничение движения маятника

    void Update()
    {
        float angle = _limit * Mathf.Sin(Time.time * _speed); //Рассчитываем текущий угол маятника
        transform.localRotation = Quaternion.Euler(0f, 0f, angle); //Задаём угол маятника в градусах
    }

    public Transform GetSphereSpawn()
    {
        return _sphereSpawn; //Возвращаем трансформ спавна
    }
}
