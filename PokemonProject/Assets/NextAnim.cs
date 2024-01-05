using UnityEngine;

public class NextAnim : MonoBehaviour
{
    public GameObject GO;

    public void GoNextAnim()
    {
        GO.SetActive(true);
        gameObject.SetActive(false);
    }
}
