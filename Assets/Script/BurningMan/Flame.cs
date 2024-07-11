using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _flameEffect; //������ �������

    public event Action<Flame> onSwimming; //������� ���������� � ����

    void OnTriggerEnter2D(Collider2D col) //���� ������ � ���� ��������
    {
        if (col.tag == "Water") //���� ������ � ��������� ����� ��� ����
        {
            if (_flameEffect)
            {
                _flameEffect.Stop(); //������������� ������� �������
            }

            onSwimming?.Invoke(this); //�������� ������� ���������� � ����
        }
    }
}
