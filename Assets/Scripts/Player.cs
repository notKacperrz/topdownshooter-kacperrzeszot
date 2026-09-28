using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 moveinput;
    Vector2 screenBoundery;
    [SerializeField] int playerHealth = 4;
    [SerializeField] float invinsibleTime = 3f;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float bulletSpeed = 7f;
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject gun;

    bool invinsible;

    float targetAngle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        screenBoundery = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void OnMove(InputValue value)
    {
        moveinput = value.Get<Vector2>();
    } // <-- added to close OnMove

    void OnAttack()
    {
        Rigidbody2D playerBullet = Instantiate(bullet, gun.transform.position, transform.rotation).GetComponent<Rigidbody2D>();
        playerBullet.AddForce(transform.up * bulletSpeed, ForceMode2D.Impulse);
    }

// Update is called once per frame
    void Update()
    {
        rb.linearVelocity = moveinput * moveSpeed;
        if (moveinput != Vector2.zero)
        {
            targetAngle = Mathf.Atan2(moveinput.y, moveinput.x) * Mathf.Rad2Deg;
        }


        transform.position = new Vector2(
            Mathf.Clamp(transform.position.x, -screenBoundery.x, screenBoundery.x),
            Mathf.Clamp(transform.position.y, -screenBoundery.y, screenBoundery.y)
        );
    }

    void FixedUpdate()
    {
        if (moveinput != Vector2.zero)
        {
            float rotation = Mathf.MoveTowardsAngle(rb.rotation, targetAngle - 90, rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rotation);
        }
    }
    void ResetInvinsibility()
    {
        invinsible = false; 
    }
    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemies") && !invinsible)
        {
            if (playerHealth <= 1)
            {
                Destroy(gameObject);
            }
            else
            {
                playerHealth--;
                invinsible = true;
                Invoke("ResetInvinsibility", invinsibleTime);
                Debug.Log("Player Health: " + playerHealth);
            }
        }
    }
}
