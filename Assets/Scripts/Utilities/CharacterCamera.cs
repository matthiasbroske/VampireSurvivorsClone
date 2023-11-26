using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private float width = 6.0f;
        private Camera characterCamera;
        
        void Awake()
        {
            characterCamera = GetComponent<Camera>();
            characterCamera.orthographicSize = width/characterCamera.aspect;
        }
    }
}
