using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Vampire
{
    public class Book : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 1;
        private BookAbility bookAbility;
        private LayerMask monsterLayer;

        public void Init(BookAbility bookAbility, LayerMask monsterLayer)
        {
            this.bookAbility = bookAbility;
            this.monsterLayer = monsterLayer;
        }

        void Update()
        {
            transform.RotateAround(transform.position, Vector3.back, Time.deltaTime*100*rotationSpeed);
        }

        void OnTriggerEnter2D(Collider2D collider)
        {
            if ((monsterLayer & (1 << collider.gameObject.layer)) != 0)
            {
                bookAbility.Damage(collider.gameObject.GetComponentInParent<Monster>());
            }
        }
    }
}
