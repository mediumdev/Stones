using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _flameEffect; //Эффект пламени

    public event Action<Flame> onSwimming; //Событие погружения в воду

    void OnTriggerEnter2D(Collider2D col) //Если входим в зону триггера
    {
        if (col.tag == "Water") //Если объект с триггером имеет тег воды
        {
            if (_flameEffect)
            {
                _flameEffect.Stop(); //Останавливаем частицы пламени
            }

            onSwimming?.Invoke(this); //Вызываем событие погружения в воду
        }
    }
}
