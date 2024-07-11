using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController2D _controller;

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private float _runSpeed; //�������� ����
    
    private float horizontalMove = 0; //����������� ��������

    private bool jump = false; //������� ��� ���

    void Update()
    {
        if (!_animator)
        {
            return;
        }

        if (Input.GetAxisRaw("Horizontal") != 0f) //��� ���������� � ���������� ��� ��������
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed; //������� ���������
        }
        else //��� ���������� � ������������ �����������
        {
            horizontalMove = ControlManager._virtualJoystick.inputDirection.x * _runSpeed; //������� ���������
        }

        _animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //����������� �������� ���� �� ��������� ���������

        if ((Input.GetButton("Jump")) || (ControlManager._virtualJoystick.jump)) //��� ���������� � ����������, �������� ��� ������������ �����������
        {
            jump = true; //�������� �������
            _animator.SetBool("IsJumping", true); //�������� ������
        }
    }

    public void OnLanding()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetBool("IsJumping", false); //���� ����� �� ����� ������������� �������� ������
    }

    public void StopMove()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetFloat("Speed", 0f); //������������� �������� ����
        _animator.SetBool("IsJumping", false); //������������� �������� ������
    }

    void FixedUpdate()
    {
        if (!_controller)
        {
            return;
        }

        _controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump); //������� ���������
        jump = false; //������ ��� ������������� ������
    }
}
