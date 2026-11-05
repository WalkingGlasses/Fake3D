using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 itemPosition;

    [Header("Obstacle Settings")]
    public int lane;
    public float requiredJumpHeight = 0.8f;

    [HideInInspector]
    public bool collisionResolved = false;

    private void Update()
    {
        float perspective = CameraComponent.focalLength /
                            (CameraComponent.focalLength + itemPosition.z);

        Vector3 vanishingPoint = new Vector3(
            CameraComponent.VanishingPoint.x,
            CameraComponent.VanishingPoint.y,
            0f
        );

        Vector3 originalPosition = new Vector3(
            itemPosition.x,
            itemPosition.y,
            0f
        );

        transform.position = vanishingPoint +
                             (originalPosition - vanishingPoint) * perspective;

        transform.localScale = Vector3.one * perspective;
    }
}