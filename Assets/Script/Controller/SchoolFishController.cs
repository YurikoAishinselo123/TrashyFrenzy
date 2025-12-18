using UnityEngine;

public class SchoolFishController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed = 4f;

    [Header("Bounds")]
    [SerializeField] float minX = -12f;
    [SerializeField] float maxX = 12f;

    Vector2 direction;
    int laneIndex;
    SchoolFishSpawner spawner;

    public void Init(Vector2 moveDir, int lane, SchoolFishSpawner owner)
    {
        direction = moveDir.normalized;
        laneIndex = lane;
        spawner = owner;

        // Face movement direction
        float angle = direction.x > 0 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);

        if (OutOfBounds())
            DestroySelf();
    }

    bool OutOfBounds()
    {
        return (direction.x > 0 && transform.position.x > maxX) ||
               (direction.x < 0 && transform.position.x < minX);
    }

    void DestroySelf()
    {
        if (spawner != null)
            spawner.ReleaseLane(laneIndex);

        Destroy(gameObject);
    }

    public Vector2 GetVelocity()
    {
        return direction * speed;
    }
}
