using UnityEngine;

[CreateAssetMenu(
    fileName = "NPCFishData",
    menuName = "Fish/NPC Fish Data"
)]
public class NPCFishDataSO : ScriptableObject
{
    [Header("Spawn")]
    public float spawnInterval = 2f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Lifetime")]
    public float destroyX = 15f;

    [Header("Visual")]
    public RuntimeAnimatorController animatorController;
}
