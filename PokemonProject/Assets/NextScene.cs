using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string sceneToLoad;
    public void GoNextScene() => SceneManager.LoadScene(sceneToLoad);
}
