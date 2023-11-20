using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] float timeMovement;
    private bool isMoving;
    private Vector2 destination;
    private Vector2 startPosition;
    private float timer;
    private Vector2 moveDirection;

    public void SetMoveDirection(Vector2 direction)
    {
        moveDirection = direction;
      
    }

    private void Update()
    {
        
        if (isMoving)
        {
            Move();
        }
        else
        {
            if (moveDirection != Vector2.zero)
            {
                isMoving = true;
                startPosition = transform.position;
                destination = startPosition + moveDirection;
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
        }
    }
}
