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
    private float _runSpeed; //Скорость бега
    
    private float horizontalMove = 0; //Направление движения

    private bool jump = false; //Прыгает или нет

    void Update()
    {
        if (!_animator)
        {
            return;
        }

        if (Input.GetAxisRaw("Horizontal") != 0f) //При управлении с клавиатуры или геймпада
        {
            horizontalMove = Input.GetAxisRaw("Horizontal") * _runSpeed; //Двигаем персонажа
        }
        else //При управлении с виртуального контроллера
        {
            horizontalMove = ControlManager._virtualJoystick.inputDirection.x * _runSpeed; //Двигаем персонажа
        }

        _animator.SetFloat("Speed", Mathf.Abs(horizontalMove)); //Проигрываем анимацию бега со скоростью персонажа

        if ((Input.GetButton("Jump")) || (ControlManager._virtualJoystick.jump)) //При управлении с клавиатуры, геймпада или виртуального контроллера
        {
            jump = true; //Персонаж прыгнул
            _animator.SetBool("IsJumping", true); //Анимация прыжка
        }
    }

    public void OnLanding()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetBool("IsJumping", false); //Если стали на землю останавливаем анимацию прыжка
    }

    public void StopMove()
    {
        if (!_animator)
        {
            return;
        }

        _animator.SetFloat("Speed", 0f); //Останавливаем анимацию бегу
        _animator.SetBool("IsJumping", false); //Останавливаем анимацию прыжка
    }

    void FixedUpdate()
    {
        if (!_controller)
        {
            return;
        }

        _controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump); //Двигаем персонажа
        jump = false; //Каждый раз останавливаем прыжок
    }
}
