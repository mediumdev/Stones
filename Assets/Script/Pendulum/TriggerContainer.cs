using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using static GameController;

public class TriggerContainer : MonoBehaviour
{
    [SerializeField]
    private List<TriggerPattern> _triggerPatterns; //������ �� ������� �������� � ������������

    public event Action onPatternsCheck; //������� ����� �������� ���� ����������

    private List<TriggerPattern> checkPatterns; //������ ����������� ����������

    public int triggerCount { get; private set; } //���������� ���� ���������

    void Start()
    {
        checkPatterns = new List<TriggerPattern>(); //������ ������

        foreach (var patterns in _triggerPatterns) //�������� �� ���� �����������
        {
            patterns.onPatternCheck += OnPatternCheck; //������������� �� ������� ����� �������� ����������
        }

        CircleTrigger[] CircleTriggers = GetComponentsInChildren<CircleTrigger>(); //������ ������ �� ����� ����������
        triggerCount = CircleTriggers.Length; //���������� ���������� ���������
    }

    private void OnPatternCheck(TriggerPattern pattern, CircleTrigger trigger)
    {
        checkPatterns.Add(pattern); //��������� � ������ ����������� ����������

        if (checkPatterns.Count >= trigger.patternsCount) //���� ��� ���������� � ������� ���� ������� ���������
        {
            foreach (var checkPattern in checkPatterns) //�������� �� ���� ����������� �����������
            {
                if (checkPattern.complete) //���� ���������� �������
                {
                    checkPattern.RemoveCircles(); //���������� � ��� ��� ����
                }
            }

            onPatternsCheck?.Invoke(); //�������� ������� �������� ����������
            checkPatterns.Clear(); //������� ������ ����������
        }
    }

    void OnDestroy()
    {
        foreach (var patterns in _triggerPatterns) //�������� �� ���� �����������
        {
            patterns.onPatternCheck -= OnPatternCheck; //������������ �� ������� ����� �������� ����������
        }
    }
}
