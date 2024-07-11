using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pendulum : MonoBehaviour
{
    [SerializeField]
    private Transform _sphereSpawn; //������ �� ��������� ������, �� ��� ����������� ���������� ���

    [SerializeField]
    private float _speed; //�������� �������� ��������

    [SerializeField]
    private float _limit; //����������� �������� ��������

    void Update()
    {
        float angle = _limit * Mathf.Sin(Time.time * _speed); //������������ ������� ���� ��������
        transform.localRotation = Quaternion.Euler(0f, 0f, angle); //����� ���� �������� � ��������
    }

    public Transform GetSphereSpawn()
    {
        return _sphereSpawn; //���������� ��������� ������
    }
}
