
using UnityEngine;

namespace Vampire
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;

        void Update()
        {
            transform.RotateAround(transform.position, Vector3.back, Time.deltaTime*100*rotationSpeed);
        }
    }
}