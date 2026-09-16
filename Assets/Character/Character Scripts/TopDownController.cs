using UnityEngine;

public class TopDownController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;

        // Horizontal facing has priority while A/D are held
        if (Input.GetKey(KeyCode.A))
        {
            Face(Vector2.left);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            Face(Vector2.right);
        }
        // Otherwise face vertically
        else if (Input.GetKey(KeyCode.W))
        {
            Face(Vector2.up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Face(Vector2.down);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed;
    }

    void Face(Vector2 direction)
    {
        if (direction == Vector2.up)
            transform.rotation = Quaternion.Euler(0, 0, 0);

        else if (direction == Vector2.down)
            transform.rotation = Quaternion.Euler(0, 0, 180);

        else if (direction == Vector2.left)
            transform.rotation = Quaternion.Euler(0, 0, 90);

        else if (direction == Vector2.right)
            transform.rotation = Quaternion.Euler(0, 0, -90);
    }
}