using System.Collections;
using UnityEngine;

namespace Vampire
{
    public abstract class BossAbility : MonoBehaviour
    {
        [Header("Ability Details")]
        protected BossMonster monster;
        protected EntityManager entityManager;
        protected Character playerCharacter;
        protected bool active = false;
        protected float useTime;

        public virtual void Init(BossMonster monster, EntityManager entityManager, Character playerCharacter)
        {
            this.monster = monster;
            this.entityManager = entityManager;
            this.playerCharacter = playerCharacter;
        }

        public abstract IEnumerator Activate();

        public virtual void Deactivate()
        {
            active = false;
            StopAllCoroutines();
        }

        // Gives the ability a score indicating how suitable it
        // would be to use the ability at the given moment
        public abstract float Score();
    }
}
