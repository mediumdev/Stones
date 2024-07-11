using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _obstacle; //������ �� �����������

    public event Action<Circle> onCircleTrigger; //������� ���� ��� ����� � ���� �������� � �����������
    public Circle circle { get; private set; } //������ �� ��� ������� ����� � ���� ��������
    public int patternsCount { get; private set; } //����� �������� � ����������� ��������� � ������� ���� ���� �������

    void Start()
    {
        if (_obstacle)
        {
            _obstacle.SetActive(false); //������������ �����������
        }
    }

    public void SetCircle(Circle circle)
    {
        this.circle = circle; //����� ������ �� ���

        if (_obstacle)
        {
            bool showObstacle = circle; //���� ������ �� ��� �� ������� �� ���������� �����������
            _obstacle.SetActive(showObstacle); //���������� �����������
        }

        if (!circle)
        {
            return;
        }    

        onCircleTrigger?.Invoke(circle); //�������� ������� ��� ������� ��� �������� � ���� �������� � ������� ���� ���
    }

    public void IncreasePattern()
    {
        patternsCount++; //��������� ������ � ������������ � ������� ���� ���� �������
    }
}
