using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class EnemyStats : CharacterStats
    {
        public override void Die()
        {
            base.Die();
            Destroy(gameObject);
        }
    }
}
