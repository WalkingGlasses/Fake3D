using UnityEngine;

public class CameraComponent : MonoBehaviour
{
    public static float focalLength = 100f;

    public Vector2 vanishingPoint = Vector2.zero;

    public static Vector2 VanishingPoint;

    private void Awake()
    {
        VanishingPoint = vanishingPoint;
    }
}