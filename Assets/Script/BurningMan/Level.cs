using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerPrefab; //������ �� ������ ������

    [SerializeField]
    private Transform _spawn; //������ �� ������ ������

    void Start()
    {
        SpawnPlayer(); //������ ������ ��� ������
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

        GameObject newPlayer = Instantiate(_playerPrefab, transform); //������ ��������� ������
        newPlayer.transform.position = _spawn.transform.position; //���������� �� ���������� ������

        Flame flame = newPlayer.GetComponent<Player>()._flame; //��������� ������ �� ������ �������

        if (!flame) //���� ������ ������
        {
            return;
        }

        flame.onSwimming += OnPlayerSwimming; //������������� �� ������� ���������� ������� � ����
    }

    private void OnPlayerSwimming(Flame flame)
    {
        if (flame)
        {
            flame.onSwimming -= OnPlayerSwimming; //������������ �� ������� ���������� ������� � ����
        }

        SpawnPlayer(); //������� ������
    }
}
