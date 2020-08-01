using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("load scene");
        SceneManager.LoadScene("MainScene");
    }

    public void ShowHistory()
    {
        Debug.Log("show history");
        SceneManager.LoadScene("GuideScene");
    }
}
