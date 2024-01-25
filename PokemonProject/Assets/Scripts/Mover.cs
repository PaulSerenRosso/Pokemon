using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] float timeMovement;
    [SerializeField] private float timeJumping =0.2f;
    private bool isMoving;
    private bool isJumping;
    private Vector2 destination;
    private Vector2 startPosition;
    private float timer;
    private List<Vector2> moveDirections = new();
    public event Action<Mover> endMoveEvent;
    [SerializeField] private Vector2 currentDirection;
    [SerializeField] private Animator animator;
    [SerializeField] private SerializableDictionary<Vector2, Sprite> allSprites;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask rayCollisionLayerMask;
    [SerializeField] private GameObject moverRenderer;
    [SerializeField] private GameObject jumpShadowRenderer;
    [SerializeField] private AnimationCurve jumpCurve;
    public bool IsMoving => isMoving;
    public bool IsJumping => isJumping; 

    private void OnEnable()
    {
        StartCoroutine(Rotate(currentDirection));
    }

    public void AddDirection(Vector2 direction)
    {
  
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
        if (moveDirections.Count == 0 && !isMoving)
        {
            SetAnimationMovement(Vector2.zero);
            StartCoroutine(Rotate(currentDirection));
        }
    }

    private void Update()
    {
        if (isJumping)
        {
            Jump();
        }
        else if (isMoving)
        {
            Move();
        }
        else
        {
            if (moveDirections.Count != 0)
            {
                StartMove();
            }
        }
    }

    private void Jump()
    {
        timer += Time.deltaTime;
        transform.position = Vector2.Lerp(startPosition, destination, timer / timeJumping);
        var transformPosition = moverRenderer.transform.localPosition;
        transformPosition.y = jumpCurve.Evaluate(timer / timeJumping); 
        moverRenderer.transform.localPosition = transformPosition;
        jumpShadowRenderer.SetActive(true);
        if (timer >= timeJumping)
        {
            jumpShadowRenderer.SetActive(false);
            isJumping = false;
            endMoveEvent?.Invoke(this);
        }
    }

    private void Move()
    {
        timer += Time.deltaTime;
        transform.position = Vector2.Lerp(startPosition, destination, timer / timeMovement);
        if (timer >= timeMovement)
        {
            isMoving = false;
            transform.position = Vector2.Lerp(startPosition, destination, timer / timeMovement);
            timer = 0;
            endMoveEvent?.Invoke(this);
            if (moveDirections.Count != 0)
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
        Physics2D.queriesHitTriggers = false;
        var hit = Physics2D.Raycast(startPosition, currentDirection, 1f, rayCollisionLayerMask);
        if (!hit)
        {
            isMoving = true;
            SetAnimationMovement(currentDirection);
        }
        else if (hit.collider.CompareTag("Slope"))
        {
            if (hit.collider.GetComponent<Slope>().CheckSlopeDirection(currentDirection))
            {
                destination = startPosition + currentDirection * 2;
                isMoving = true;
                isJumping = true;
                SetAnimationMovement(currentDirection);
            }
            else
            {
                isMoving = false;
                RemoveDirection(currentDirection);
            }
        }
        else
        {
            isMoving = false;
            RemoveDirection(currentDirection);
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


    public IEnumerator Rotate(Vector2 forward)
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

    public static Direction LookDirection(Vector3 myPos, Vector3 otherPos)
    {
        if (myPos.x == otherPos.x)
        {
            if (myPos.y > otherPos.y) 
            {
                return Direction.North; // -> My pos est en haut
            }
            else
            {
                return Direction.South; // -> My pos est en haut
            }
        }
        else
        {
            if (myPos.x > otherPos.x) // Meme x 
            {
                return Direction.East;
            }
            else
            {
                return Direction.West;
            }
        }
    }
}

public enum Direction
{
    East, West, North, South
}