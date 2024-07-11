using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab; //Ссылка на префаб игрока

    [SerializeField]
    private Transform _spawn; //Ссылка на объект спавна

    void Start()
    {
        SpawnPlayer(); //Создаём игрока при старте
    }

    private void SpawnPlayer()
    {
        if (!_playerPrefab)
        {
            return;
        }

        if (!_spawn)
        {
            return;
        }

        GameObject newPlayer = Instantiate(_playerPrefab, transform); //Создаём экземпляр игрока
        newPlayer.transform.position = _spawn.transform.position; //Перемещаем на координаты спавна

        Flame flame = newPlayer.GetComponent<Player>()._flame; //Сохраняем ссылку на объект пламени

        if (!flame) //Если объект создан
        {
            return;
        }

        flame.onSwimming += OnPlayerSwimming; //Подписываемся на событие погружение пламени в воду
    }

    private void OnPlayerSwimming(Flame flame)
    {
        if (flame)
        {
            flame.onSwimming -= OnPlayerSwimming; //Отписываемся на события погружения пламени в воду
        }

        SpawnPlayer(); //Спавним игрока
    }
}
