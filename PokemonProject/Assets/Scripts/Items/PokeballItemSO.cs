using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeyItem", menuName = "Items/Pokeball", order = 2)]
public class PokeballItemSO : ItemSO
{
    public int numberOfItems = 1;
    public void IncrementItemCount()
    {
        numberOfItems++;
    }

    public void DecrementItemCount()
    {
        numberOfItems = Mathf.Max(0, numberOfItems - 1);
    }
}
