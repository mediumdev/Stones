using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class TriggerPattern : MonoBehaviour
{
    [SerializeField]
    private List<CircleTrigger> triggers; //������ ��������� � ���� ������� ����������

    public event Action<TriggerPattern, CircleTrigger> onPatternCheck; //������� ���������� ����� �������� ����������

    public bool complete { get; private set; } = false; //������� �� ����������

    void Start()
    {
        foreach (var trigger in triggers) //�������� �� ���� ��������� � �������
        {
            trigger.onCircleTrigger += OnCircleTrigger; //������������� �� ������� ����� ���� � ���� ��������
            trigger.IncreasePattern(); //�������� ������� �������� � ��������
        }
    }

    private void OnCircleTrigger(Circle circle)
    {
        if (triggers == null || triggers.Count == 0)
        {
            return;
        }

        Color val = circle.circleColor.color; //��������� ���� ���� ������� ����� � ���� �������� � �����������

        //���� �� ���� ��������� � ������� ��������� ���� � ��� ������ �����
        if (!triggers.Any(x => x.circle == null) && triggers.All(x => x.circle.circleColor.color == val)) 
        {
            complete = true; //���������� �������
        }

        onPatternCheck?.Invoke(this, circle.trigger); //�������� ������� ��� ���������� ��������� (�� ����� ������� ��� ���)
    }

    public void RemoveCircles()
    {
        foreach (var trigger in triggers) //�������� �� ���� ���������
        {
            if (trigger.circle) //���� �� �� ���� ���
            {
                trigger.circle.Remove(); //���������� ���
            }
        }

        complete = false; //���������� �� �������
    }

    void OnDestroy()
    {
        foreach (var trigger in triggers) //��� ���� ���������
        {
            trigger.onCircleTrigger -= OnCircleTrigger; //������������ �� ������� ����� ���� � ���� ��������
        }
    }
}
