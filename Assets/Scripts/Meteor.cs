using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 moveDirection = Vector3.up;
    public float rotationSpeed = 120f;
    public float destroyY = 6f;

    void Update()
    {
        transform.Translate(moveDirection.normalized * speed * Time.deltaTime, Space.World);
        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);

        if (transform.position.y > destroyY)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance != null && !GameManager.Instance.IsGameOver)
            {
                GameManager.Instance.EndGame();
            }

            Destroy(gameObject);
        }
    }
}
