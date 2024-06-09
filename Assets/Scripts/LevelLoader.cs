using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;
    Scene currentScene;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
    }
    public void OnClickLoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadLevel(sceneIndex));
        FindObjectOfType<AudioManager>().Play("boom");
    }
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
    public void QuitGame()
    {

        Application.Quit();

    }
}
