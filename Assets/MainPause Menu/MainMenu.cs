using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject creditsPanel;

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}