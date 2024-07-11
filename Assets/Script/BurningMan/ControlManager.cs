using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour
{
    public static ControlManager instance = null;

    public static VirtualJoystick _virtualJoystick; //Храним ссылку на объект джойстика

    void Awake()
    {
        if (instance == null) //Если экземпляр не создан
        {
            instance = this; //Создаём экземпляр
        }
        else if (instance == this) //Если экземпляр создан
        {
            Destroy(gameObject); //Удаляем экземпляр
        }
    }

    public void OnNextSceneClick(string sceneName)
    {
        if (GameManager.instance)
        {
            GameManager.LoadLevel(sceneName); //При нажатии на кнопку перехода, загружаем нужную сцену
        }
    }
}
