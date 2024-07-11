using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pendulum;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private Pendulum _pendulum; //������ �� �������

    [SerializeField]
    private TriggerContainer _triggerContainer; //������ �� ��������� � ���������� � ��������

    [SerializeField]
    private TMPro.TextMeshProUGUI _pointsText; //������ �� ����� � ����������� �����

    [SerializeField]
    private GameObject _circlePrefab; //������ �� ������ ����

    [SerializeField]
    private List<CircleColor> _circleColors; //������ ������������ ����� � ����� ������� �� ��� ����

    private Circle currentCircle = null; //��� �� ��������

    private bool dropActive = true; //����� �� ������� ���

    private int circleCount = 0; //���������� ����� � ����

    private int points = 0; //���������� �����

    [Serializable]
    public struct CircleColor //��������� ������ � �����
    {
        public Color color; //����
        public int points; //���������� �����
    }

    void Start()
    {
        if (!_triggerContainer)
        {
            return;
        }

        SpawnSphere(); //������� ���

        _triggerContainer.onPatternsCheck += OnPatternsCheck; //������������� �� ������� ����� �������� ����������
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0)) //��� ������� ������ ��� ������� ����
        {
            if (dropActive) //���� ����� ������� ���
            {
                DropSphere(); //���������

                dropActive = false; //��������� ������ �� �����
            }
        }
    }

    private void OnPatternsCheck()
    {
        if (circleCount < _triggerContainer.triggerCount) //���� ����� ������ ��� ���������
        {
            SpawnSphere(); //������� ��� �� ��������
        }
        else //�����
        {
            GameEnd(); //����� ����
        }
    }

    private void SpawnSphere()
    {
        if (currentCircle)
        {
            return;
        }

        if (!_pendulum)
        {
            return;
        }

        if (!_triggerContainer)
        {
            return;
        }

        if (!_circlePrefab)
        {
            return;
        }

        currentCircle = Instantiate(_circlePrefab, _pendulum.GetSphereSpawn()).GetComponent<Circle>(); //������ ��������� ���� � ��������� ������ �� ����
        CircleColor randomColor = GetRandomColor(); //������� ��������� ���� �� ���������
        currentCircle.SetCircleColor(randomColor); //����� ����
        currentCircle.GetComponent<Rigidbody2D>().isKinematic = true; //��������� ������ ����
        currentCircle.onRemove += OnCircleRemove; //������������� �� ������� ����������� ����

        circleCount++; //����������� ������� �����

        dropActive = true; //����� ���������
    }

    private void GameEnd()
    {
        if (GameManager.instance)
        {
            GameManager.pendulumPoints = points; //��������� ���������� ����� � GameManager
            GameManager.LoadLevel("Replay"); //��������� �� ����� ��������
        }
    }

    private void OnCircleRemove(Circle circle)
    {
        circleCount--; //��������� ������� �����

        points += circle.circleColor.points; //����������� ���������� �����

        if (_pointsText)
        {
            _pointsText.text = points.ToString(); //���������� ���������� �����
        }

        circle.onRemove -= OnCircleRemove; //������������ �� ������� ����������� ����
    }

    private void DropSphere()
    {
        if (!currentCircle)
        {
            return;
        }

        currentCircle.Drop(); //��������� ���
        currentCircle.onStay += CurrentSphereStay; //������������� �� ������� ��������� ����
    }

    private void CurrentSphereStay()
    {
        if (!currentCircle)
        {
            return;
        }

        currentCircle.onStay -= CurrentSphereStay; //������������ �� ������� ��������� ����
        currentCircle = null; //�������� ������ ���� �� ��������
    }

    private CircleColor GetRandomColor()
    {
        if (_circleColors.Count > 0)
        {
            return _circleColors[Random.Range(0, _circleColors.Count)]; //�������� ��������� ����
        }

        return new CircleColor(); //���������� ���
    }

    void OnDestroy()
    {
        _triggerContainer.onPatternsCheck -= OnPatternsCheck; //������������ �� ������� ����� �������� ����������
    }
}
