using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Vampire
{
    public class StartingAbilityContainer : MonoBehaviour
    {
        [field: SerializeField]
        public Image AbilityImage { get; private set; }
        [field: SerializeField]
        public RectTransform ImageRect { get; private set; }
        [field: SerializeField]
        public RectTransform ContainerRect { get; private set; }
    }
}
