using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GameController;

public class Circle : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosionEffect; //������ �� ������ ������

    [SerializeField]
    private float _appearSpeed; //�������� ���������

    public event Action onStay; //������� ���� ��� �����������
    public event Action<Circle> onRemove; //������� ���� ��� �����

    public CircleTrigger trigger { get; private set; } //������ �� ������� � ���� �������� ����� ���� � �����������

    public CircleColor circleColor { get; private set; } //������ CircleColor, ������ ���� � ���������� �����


    private SpriteRenderer spriteRenderer = null; //������ �� ������
    private Rigidbody2D rb = null; //������ �� ���������� ����

    private bool isFalling = false; //������ ��� ���
    private float alpha = 0f; //������������

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); //��������� ������
        rb = GetComponent<Rigidbody2D>(); //��������� ���������� ����
    }

    void Update()
    {
        Appear(); //���������� ��������� ����
    }

    void FixedUpdate()
    {
        if (!rb)
        {
            return;
        }

        if (!isFalling) //���� ��� �� ������
        {
            if (rb.velocity.magnitude > 1f) //�� ��� ��������
            {
                isFalling = true; //������ �� ������
            }
        }
        else //���� ��� ������
        {
            if (rb.velocity.magnitude < 0.001f && !rb.isKinematic) //�������� �������� ����� ���� ��� � ���
            {
                rb.velocity = Vector2.zero; //�������� �������� �������� �� ������ ������
                rb.angularVelocity = 0f; //� �������� �������� ��������
                isFalling = false; //��� �� ������

                onStay?.Invoke(); //�������� ������� ��������� ����

                if (trigger) //���� ����� � ���� �������
                {
                    trigger.SetCircle(this); //������� ��� ������ �� ���� ���
                }
            }
        }
    }

    public void SetCircleColor(CircleColor circleColor)
    {
        if (!spriteRenderer)
        {
            return;
        }

        this.circleColor = circleColor; //����� CircleColor

        spriteRenderer.color = circleColor.color; //����� ������ ���� ����
    }

    public void Drop()
    {
        if (!rb)
        {
            return;
        }

        transform.parent = null; //�������� ��������
        rb.isKinematic = false; //�������� ������
    }

    public void Remove()
    {
        if (_explosionEffect)
        {
            GameObject explosionEffect = Instantiate(_explosionEffect, transform.position, Quaternion.identity) as GameObject; //������ ��������� ������� ������
            explosionEffect.transform.position = transform.position; //���������� ������ �� ���������� ����
            var main = explosionEffect.gameObject.GetComponent<ParticleSystem>().main; //��������� ������� ������ �������
            main.startColor = spriteRenderer.color; //������ ��� ���� �� ���� ������� ����
        }

        if (trigger)
        {
            trigger.SetCircle(null); //������� � �������� ������ �� ���
        }

        onRemove?.Invoke(this); //�������� ������� �������� ����

        Destroy(gameObject); //������� ���
    }

    private void Appear()
    {
        if (alpha < 1f)
        {
            alpha += Time.deltaTime * _appearSpeed; //���������� ������������ ����
        }
        else
        {
            alpha = 1f;
        }

        Color color = spriteRenderer.color; //��������� ���� ����
        color.a = alpha; //����� �������� �����-������
        spriteRenderer.color = color; //��������� ����
    }

    private void OnTriggerEnter2D(Collider2D collision) //���� ��������� ������� ����� � ���� ��������
    {
        trigger = collision.GetComponent<CircleTrigger>(); //��������� �������
    }

    private void OnTriggerExit2D(Collider2D collision) //���� ��������� ������� ������ �� ���� ��������
    {
        CircleTrigger circleTrigger = collision.GetComponent<CircleTrigger>();

        if (circleTrigger)
        {
            circleTrigger.SetCircle(null); //�������� � �������� ������ �� ���
        }
    }
}
