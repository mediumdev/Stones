using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Flame _flame; //Объект с пламенем

    [SerializeField]
    private ParticleSystem _smokeEffect; //Эффект дыма

    private bool isBurning = true; //Персонаж горит или нет

    void Start()
    {
        if (_flame)
        {
            _flame.onSwimming += OnPlayerSwimming; //Подписываемся на событие погружение в воду
        }
    }

    private void OnPlayerSwimming(Flame flame)
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement)
        {
            playerMovement.StopMove(); //Персонаж перестаёт двигаться
            playerMovement.enabled = false; //Отключаем управление
        }

        if ((_smokeEffect) && (isBurning))
        {
            isBurning = false; //Перестаёт гореть

            _smokeEffect.Play(); //Запускаем частицы дама
        }
    }

    void OnDestroy()
    {
        if (_flame)
        {
            _flame.onSwimming -= OnPlayerSwimming; //Отписываемся от события погружения в воду
        }
    }
}
