using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class InfiniteBackground : MonoBehaviour
    {
        private Transform playerTransform;
        [SerializeField] private Material backgroundMaterial;
        private Vector2 previousResetPosition = Vector2.zero;
        private Vector2 resetOffset = Vector2.zero;
        private float resetDistance = 15;
        private float resetDuration = 5;

        void Awake()
        {
            // Determine the screen size in world space so that we can spawn enemies outside of it
            Vector2 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector2 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));
            Vector3 screenSizeWorldSpace = new Vector3(topRight.x - bottomLeft.x, topRight.y - bottomLeft.y, 1);
            transform.localScale = screenSizeWorldSpace;
            GetComponent<MeshRenderer>().sharedMaterial = backgroundMaterial;
        }

        public void Init(Texture2D backgroundTexture, Transform playerTransform)
        {
            this.playerTransform = playerTransform;
            backgroundMaterial.mainTexture = backgroundTexture;
            backgroundMaterial.SetFloat("_Shockwave", 0);
            resetOffset = Vector2.zero;
            backgroundMaterial.SetVector("_ResetOffset", resetOffset);
            backgroundMaterial.SetInt("_Resetting", 0);
        }

        public IEnumerator Shockwave(float distance)
        {
            float d = 0;
            while (d < distance)
            {
                d += Time.deltaTime*16;
                backgroundMaterial.SetFloat("_Shockwave", d);
                backgroundMaterial.SetVector("_PlayerPosition", playerTransform.position);
                yield return null;
            }
            backgroundMaterial.SetFloat("_Shockwave", 0);
        }
        
        private void Update()
        {
            Vector2 toReset = previousResetPosition - (Vector2)playerTransform.position;
            if (toReset.sqrMagnitude > resetDistance * resetDistance)
            {
                StartCoroutine(ResetBackground(toReset));

                previousResetPosition = playerTransform.position;
            }
        }

        private IEnumerator ResetBackground(Vector2 toReset)
        {
            backgroundMaterial.SetInt("_Resetting", 1);
            backgroundMaterial.SetVector("_TempResetOffset", toReset);
            
            float t = 0;
            while (t < resetDuration)
            {
                t += Time.deltaTime;
                backgroundMaterial.SetFloat("_ResetBlend", t/resetDuration);
                yield return null;
            }
            
            // Update the reset offset
            resetOffset += toReset;
            backgroundMaterial.SetVector("_ResetOffset", resetOffset);
            backgroundMaterial.SetInt("_Resetting", 0);
        }
    }
}
