using System.Reflection;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace Vampire
{
    public class WalkBossAbility : BossAbility
    {
        [SerializeField] protected float walkTime;

        void FixedUpdate()
        {
            if (active)
            {
                Vector2 moveDirection = (playerCharacter.transform.position - monster.transform.position).normalized;
                monster.Move(moveDirection, Time.fixedDeltaTime);
                entityManager.Grid.UpdateClient(monster);
            }
        }

        public override IEnumerator Activate()
        {
            active = true;
            yield return new WaitForSeconds(walkTime);
        }

        public override float Score()
        {
            return 1;
        }
    }
}
