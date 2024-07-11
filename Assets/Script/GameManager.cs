using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //������ �� ��������� ����� �������

    public static int pendulumPoints { get; set; } //���� ��������� � ���� � ���������

    private static string currentLevel = ""; //��� ������� �����
    private static string previousLevel = ""; //��� ���������� �����

    void Awake()
    {
        if (instance == null) //���� ������ �� ��������� �������
        {
            instance = this; //��������� ������
        }
        else if (instance == this) //���� ������ ����
        {
            Destroy(gameObject); //������� ������
        }

        DontDestroyOnLoad(gameObject); //��������� �� ��������� ��� �������� ������ �����
    }

    void Start()
    {
        LoadLevel("BurningMan"); //��������� ������ �����
    }

    public static void LoadLevel(string name)
    {
        if (currentLevel != name) //���� ����������� ����� �� ���������
        {
            LoadSceneAdditive(name); //��������� �

            previousLevel = currentLevel; //���������� ����� ��� ����������

            UnloadScene(previousLevel); //��������� ���������� �����
        }

        currentLevel = name; //���������� ����������� ����� ��� �������
    }

    private static void LoadSceneAdditive(string name)
    {
        if (name == "")
        {
            return;
        }

        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive); //��������� �������������� �����
    }

    private static void UnloadScene(string name)
    {
        if (name == "")
        {
            return;
        }

        SceneManager.UnloadSceneAsync(name); //��������� �����
    }
}
