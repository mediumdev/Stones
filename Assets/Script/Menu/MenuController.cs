using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _pointsText; //������ �� TMP � ������������ ����� �����

    void Start()
    {
        if (GameManager.instance) //���� ��������� GameManager ������
        {
            _pointsText.text = GameManager.pendulumPoints.ToString(); //�� ���� �������� ��������� �����
        }
    }

    public void OnSceneOpen(string name)
    {
        if (GameManager.instance)
        {
            GameManager.LoadLevel(name); //��������� ����� � ��������� ������
        }
    }
}
