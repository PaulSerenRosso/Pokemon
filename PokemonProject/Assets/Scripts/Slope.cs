using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slope : MonoBehaviour
{
    [SerializeField]
    private Vector2 direction;
    public bool CheckSlopeDirection(Vector2 direction)
    {
        if (direction == -this.direction)
        {
            return true;
        }
        return false;
    }
}
