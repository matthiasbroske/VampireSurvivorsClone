using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class EscapeToQuit : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}
