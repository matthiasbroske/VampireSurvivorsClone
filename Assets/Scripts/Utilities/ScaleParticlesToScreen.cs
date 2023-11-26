using UnityEngine;

namespace Vampire
{
    public class ScaleParticlesToScreen : MonoBehaviour
    {
        void Awake()
        {
            ParticleSystem particleSystem = GetComponent<ParticleSystem>();
            // Determine the screen size in world space so that we can correctly size the particle system
            Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            Vector3 screenSizeWorldSpace = new Vector3(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y, 1);
            ParticleSystem.ShapeModule shape = particleSystem.shape;
            shape.radius = screenSizeWorldSpace.x/2;
            transform.localPosition = Vector3.up * screenSizeWorldSpace.y/2;
        }
    }
}
