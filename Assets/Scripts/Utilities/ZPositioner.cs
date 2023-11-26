using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class ZPositioner : MonoBehaviour
    {
        private Transform playerTransform;
        private float scale = 0.01f;
        private bool manuallySetZ = false;
        private float manualY;

        public void Init(Transform playerTransform)
        {
            this.playerTransform = playerTransform;
        }

        void LateUpdate()
        {
            Vector3 temp = transform.position;
            temp.z = scale*((manuallySetZ ? manualY : temp.y)-playerTransform.position.y);
            transform.position = temp;
        }

        public void ManuallySetZByY(float y)
        {
            manuallySetZ = true;
            manualY = y;
        }

        public void AutomaticallySetZ()
        {
            manuallySetZ = false;
        }
    }
}
