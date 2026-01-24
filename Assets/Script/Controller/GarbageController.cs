using UnityEngine;

public class GarbageController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float fallSpeed = 3f;

    float targetY;
    bool isFalling = true;

    public void Init(float stopY)
    {
        targetY = stopY;
    }

    void Update()
    {
        if (!isFalling) return;

        Vector3 pos = transform.position;
        pos.y -= fallSpeed * Time.deltaTime;

        if (pos.y <= targetY)
        {
            pos.y = targetY;
            isFalling = false;
        }

        transform.position = pos;
    }


}
