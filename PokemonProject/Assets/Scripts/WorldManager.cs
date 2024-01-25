using System.Collections.Generic;
using UnityEngine;


public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    [SerializeField] List<GameObject> allSpaces;
    [SerializeField] private List<AudioClip> allMusics = new List<AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GameObject currentSpace;
    private void Awake()
    {
        audioSource.clip = allMusics[0];
        audioSource.Play();
        instance = this;
        foreach (var space in allSpaces)
        {
            space.SetActive(false);
        }
        currentSpace.SetActive(true);
    }

    public GameObject CurrentSpace => currentSpace;
    

    public void ChangeSpace(GameObject space)
    {
        currentSpace.SetActive(false);
        currentSpace = space;
        for (int i = 0; i < allMusics.Count; i++)
        {
            if (allSpaces[i] == space)
            {
                if(audioSource.clip == allMusics[i]) break;
                audioSource.clip = allMusics[i];
                audioSource.Play();
                break;
            }
        }
        currentSpace.SetActive(true);
    }
}
