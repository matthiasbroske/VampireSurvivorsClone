using UnityEngine;

public class ScaleToScreen : MonoBehaviour
{
    void Awake()
    {
        // Determine the screen size in world space so that we can spawn enemies outside of it
        Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
        Vector3 screenSizeWorldSpace = new Vector3(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y, 1);
        transform.localScale = screenSizeWorldSpace;
    }
}
