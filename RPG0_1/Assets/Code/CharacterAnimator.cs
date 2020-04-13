using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
    public class CharacterAnimator : MonoBehaviour
    {
        protected Animator animator;
        private NavMeshAgent navMeshAgent;
        protected CharacterCombat combat;

        protected virtual void Start()
        {
            this.animator = GetComponentInChildren<Animator>();
            this.navMeshAgent = GetComponent<NavMeshAgent>();
            combat = GetComponent<CharacterCombat>();

            combat.OnAttack += OnAttack;
        }


        public void Update()
        {
            // if(navMeshAgent.velocity.magnitude <= .1f)
            animator.SetFloat(Helpers.AnimationMoveSpeed, navMeshAgent.velocity.magnitude / navMeshAgent.speed * navMeshAgent.speed);

            animator.SetBool(Helpers.InCombat, combat.InCombat);
        }
        protected virtual void OnAttack()
        {
            animator.SetTrigger(Helpers.AttackTrigger);
        }

    }
}