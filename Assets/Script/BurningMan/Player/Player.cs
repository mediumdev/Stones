using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Flame _flame; //������ � ��������

    [SerializeField]
    private ParticleSystem _smokeEffect; //������ ����

    private bool isBurning = true; //�������� ����� ��� ���

    void Start()
    {
        if (_flame)
        {
            _flame.onSwimming += OnPlayerSwimming; //������������� �� ������� ���������� � ����
        }
    }

    private void OnPlayerSwimming(Flame flame)
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();

        if (playerMovement)
        {
            playerMovement.StopMove(); //�������� �������� ���������
            playerMovement.enabled = false; //��������� ����������
        }

        if ((_smokeEffect) && (isBurning))
        {
            isBurning = false; //�������� ������

            _smokeEffect.Play(); //��������� ������� ����
        }
    }

    void OnDestroy()
    {
        if (_flame)
        {
            _flame.onSwimming -= OnPlayerSwimming; //������������ �� ������� ���������� � ����
        }
    }
}
