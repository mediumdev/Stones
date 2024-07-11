using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IDragHandler
{
    [SerializeField]
    private RectTransform joystickTransform; //������ �� RectTransform ���������

    [SerializeField]
    private float _dragThreshold = 0.6f; //����� ����� �������� ����� ����� ��������

    [SerializeField]
    private int _dragMovementDistance = 30; //���������� �� ������� ������������ ��������

    [SerializeField]
    private int _dragOffsetDistance = 100; //������������ �������� ��������� �� ������ (������ ���������)

    public event Action<Vector2> onMove; //������� ��������
    public Vector3 inputDirection { get; private set; } //������ �������� �� ������� ��������� ��������
    public bool jump { get; private set; } //������ ������ ������ ��� ���

    void Start() 
    {
        ControlManager._virtualJoystick = this; //������� ������ �� ���� �������� � ����������

        inputDirection = Vector3.zero; //�������� �������� �������� ���������
    }

    public void OnDrag(PointerEventData eventData) //�� ����� ������� ��������������
    {
        Vector2 offset;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickTransform, eventData.position, null, out offset); //�������� ��������� ������������ ���������
        offset = Vector2.ClampMagnitude(offset, _dragOffsetDistance) / _dragOffsetDistance; //����������� �������� � �� -1 �� 1
        joystickTransform.anchoredPosition = offset * _dragMovementDistance; //���������� �������� 

        inputDirection = CalculateMovementInput(offset); //���������� ����������� ��������
    }

    public void OnPointerDown(PointerEventData eventData) //��������� ���������� ������� ���������, ����� ����������� ���������� ������� :)
    {

    }

    public void OnPointerUp(PointerEventData eventData) //������� ����� ��������� ���������
    {
        joystickTransform.anchoredPosition = Vector2.zero; //���������� �������� � ���������� "�����" (������� ������� �� ���������� � ����������)
        inputDirection = Vector2.zero; //�������� ����������� ��������
    }

    private Vector2 CalculateMovementInput(Vector2 offset)
    {
        float x = Mathf.Abs(offset.x) > _dragThreshold ? offset.x : 0; //������������ ���� �������� �� X �� -1 �� 1 ������������ ������
        float y = Mathf.Abs(offset.y) > _dragThreshold ? offset.y : 0; //������������ ���� �������� �� Y �� -1 �� 1 ������������ ������
        return new Vector2(x, y); //���������� ��������
    }

    public void OnJumpPressed()
    {
        jump = true; //������ ������ ������
    }

    public void OnJumpReleased()
    {
        jump = false; //������ ������ ��������
    }
}