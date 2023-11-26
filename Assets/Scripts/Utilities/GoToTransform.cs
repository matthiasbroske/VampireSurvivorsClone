using UnityEngine;

namespace Vampire
{
    public class GoToTransform : MonoBehaviour
    {
        [SerializeField] private Transform targetTransform;

        void LateUpdate()
        {
            transform.position = targetTransform.position;
        }
    }
}
