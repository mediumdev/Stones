using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null; //Ссылка на экземпляр этого объекта

    public static int pendulumPoints { get; set; } //Очки набранные в игре с маятником

    private static string currentLevel = ""; //Имя текущей сцены
    private static string previousLevel = ""; //Имя предыдущей сцены

    void Awake()
    {
        if (instance == null) //Если ссылка на экземпляр нулевая
        {
            instance = this; //Сохраняем ссылку
        }
        else if (instance == this) //Если ссылка есть
        {
            Destroy(gameObject); //Удаляем объект
        }

        DontDestroyOnLoad(gameObject); //Экземпляр не удаляется при загрузке другой сцены
    }

    void Start()
    {
        LoadLevel("BurningMan"); //Загружаем первую сцену
    }

    public static void LoadLevel(string name)
    {
        if (currentLevel != name) //Если необходимая сцена не загружена
        {
            LoadSceneAdditive(name); //Загружаем её

            previousLevel = currentLevel; //Записываем сцену как предыдущую

            UnloadScene(previousLevel); //Выгружаем предыдущую сцену
        }

        currentLevel = name; //Записываем загруженную сцену как текущую
    }

    private static void LoadSceneAdditive(string name)
    {
        if (name == "")
        {
            return;
        }

        SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive); //Загружаем дополнительную сцену
    }

    private static void UnloadScene(string name)
    {
        if (name == "")
        {
            return;
        }

        SceneManager.UnloadSceneAsync(name); //Выгружаем сцену
    }
}
