using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private TMPro.TextMeshProUGUI _pointsText; //Ссылка на TMP с отображением числа очков

    void Start()
    {
        if (GameManager.instance) //Если экземпляр GameManager создан
        {
            _pointsText.text = GameManager.pendulumPoints.ToString(); //То берём значения набранных очков
        }
    }

    public void OnSceneOpen(string name)
    {
        if (GameManager.instance)
        {
            GameManager.LoadLevel(name); //Загружаем сцену с указанным именем
        }
    }
}
