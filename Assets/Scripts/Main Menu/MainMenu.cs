using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private CharacterSelector characterSelector;

        void Start()
        {
            characterSelector.Init();
        }
    }
}
