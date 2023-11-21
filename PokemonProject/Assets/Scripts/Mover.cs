using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float timeMovement;
    private bool isMoving;
    private Vector2 destination;
    private Vector2 startPosition;
    private float timer;
    private List<Vector2> moveDirections = new();
    public event Action<Mover> endMoveEvent;
    
    public void AddDirection(Vector2 direction)
    {
        if(moveDirections.Contains(direction)) return;
        moveDirections.Add(direction);
    }
    
    public void StopMove()
    {
        transform.position = Vector2.Lerp(startPosition, destination,1);
        moveDirections.Clear();
        isMoving = false;
        timer = 0;
    }
    
    public void RemoveDirection(Vector2 direction)
    {
        moveDirections.Remove(direction);
    }

    private void Update()
    {
        if (isMoving)
        {
            Move();
        }
        else
        {
            if ( moveDirections.Count != 0)
            {
                isMoving = true;
                startPosition = transform.position;
                destination = startPosition + moveDirections[0];
                Rotate(moveDirections[0]);
            }
        }
    }

    private void Move()
    {
        timer += Time.deltaTime;
        transform.position = Vector2.Lerp(startPosition, destination, timer/timeMovement);
        if (timer >= timeMovement)
        {
            isMoving = false;
            transform.position = Vector2.Lerp(startPosition, destination,timer/timeMovement);
            timer = 0;
            endMoveEvent?.Invoke(this);
        }
    }

    public void Rotate(Vector2 forward)
    {
        transform.right = forward;
    }
}
