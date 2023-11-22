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
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private Animator animator;
    [SerializeField] private SerializableDictionary<Vector2, Sprite> allSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public bool IsMoving => isMoving;
    
    private void OnEnable()
    {
        StartCoroutine(Rotate(currentDirection));
    }

    public void AddDirection(Vector2 direction)
    {
        if(moveDirections.Contains(direction)) return;
        moveDirections.Add(direction);
    }
    
    public bool CheckMoverLookAtPosition(Vector2 position)
    {
        var tempDirection = (position - (Vector2)transform.position).normalized;
        return tempDirection == currentDirection; 
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
                StartMove();
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
            if ( moveDirections.Count != 0)
            {
                StartMove();
            }
            else
            {
                SetAnimationMovement(Vector2.zero);
                StartCoroutine(Rotate(currentDirection));
            }
        }
    }

    private void StartMove()
    {
        currentDirection = moveDirections[0];
        startPosition = transform.position; 
        destination = startPosition + currentDirection;
        if(!Physics2D.Raycast(startPosition, currentDirection, 1f, LayerMask.GetMask("Environment")))
        {
            isMoving = true;
            SetAnimationMovement(currentDirection);
        }
    }
    
    

    public GameObject GetObjectForward()
    {
        var hit = Physics2D.Raycast(transform.position, currentDirection, 1f, LayerMask.GetMask("CollideWithPlayer"));
        if (hit.collider == null)
        {
            return null;
        }
        return hit.collider.gameObject;
    }


    public IEnumerator  Rotate(Vector2 forward)
    {
        yield return new WaitForEndOfFrame();
        animator.enabled = false;
        spriteRenderer.sprite = allSprites[forward];
    }
    
    public void SetAnimationMovement(Vector2 forward)
    {
        animator.enabled = true;
        switch (forward)
        {
            case Vector2 v when v.Equals(Vector2.up):
            {
                animator.Play("MoveUp"); 
                break;
            }
            case Vector2 v when v.Equals(Vector2.down):
            {
                animator.Play("MoveDown"); 
                break;
            }
            case Vector2 v when v.Equals(Vector2.left):
            {
                animator.Play("MoveLeft"); 
                break;
            }
            case Vector2 v when v.Equals(Vector2.right):
            {
                animator.Play("MoveRight"); 
                break;
            }
            case Vector2 v when v.Equals(Vector2.zero):
            {
                animator.Play("Idle"); 
                break;
            }
            
        }
      
    }
    
    
}
