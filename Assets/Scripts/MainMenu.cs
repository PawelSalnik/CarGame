using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Play()
        {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    public void Play1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void Play2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }

    public void Play3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }

    public void Play4()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }

    public void Play5()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 6);
    }


    public void Play6()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 7);
    }


    public void Play7()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 8);
    }

    public void Play8()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 9);
    }



    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player Has Quit The Game");
    }


}
