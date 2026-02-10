using UnityEngine;

public class NPCFishController : MonoBehaviour
{
    [SerializeField] Animator animator;

    NPCFishDataSO data;
    int moveDirection;

    public void Init(NPCFishDataSO fishData, int direction)
    {
        data = fishData;
        moveDirection = direction;

        // Apply animator
        animator.runtimeAnimatorController = data.animatorController;

        // Face direction (left/right only)
        if (moveDirection > 0)
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

    void Update()
    {
        transform.position +=
            Vector3.right * moveDirection * data.moveSpeed * Time.deltaTime;

        if (Mathf.Abs(transform.position.x) > data.destroyX)
        {
            Destroy(gameObject);
        }
    }
}
