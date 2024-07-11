using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance = null;

    public static VirtualJoystick _virtualJoystick; //������ ������ �� ������ ���������

    void Awake()
    {
        if (instance == null) //���� ��������� �� ������
        {
            instance = this; //������ ���������
        }
        else if (instance == this) //���� ��������� ������
        {
            Destroy(gameObject); //������� ���������
        }
    }

    public void OnNextSceneClick(string sceneName)
    {
        if (GameManager.instance)
        {
            GameManager.LoadLevel(sceneName); //��� ������� �� ������ ��������, ��������� ������ �����
        }
    }
}
