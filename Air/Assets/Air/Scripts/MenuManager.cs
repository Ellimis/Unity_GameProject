using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject tips;

    public void GameMenu()
    {
        SceneManager.LoadScene("Start");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("game");
    }

    public void HowToDo()
    {
        tips.transform.gameObject.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }
}
