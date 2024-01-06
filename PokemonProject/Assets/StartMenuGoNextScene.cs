using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuGoNextScene : MonoBehaviour
{
    private bool canGoNextScene = false;
    
    void Update()
    {
        if (Input.anyKey)
        {
            if (canGoNextScene)
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void SetCanGoNext() => canGoNextScene = true;
}
