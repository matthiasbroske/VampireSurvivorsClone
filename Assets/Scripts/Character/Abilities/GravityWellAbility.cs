using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class GravityWellAbility : ThrowableAbility
    {
        [Header("Gravity Well Stats")]
        [SerializeField] protected UpgradeableDuration duration;
        [SerializeField] protected UpgradeableAOE wellRadius;
    }
}
