using System.Collections.Generic;
using UnityEngine;


public class WorldManager : MonoBehaviour
{
    public static WorldManager instance;
    [SerializeField] List<GameObject> allSpaces;
    [SerializeField] private GameObject currentSpace;
    private void Awake()
    {
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
        currentSpace.SetActive(true);
    }
}
