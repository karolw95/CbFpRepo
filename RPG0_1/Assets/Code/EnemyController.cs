using System;
using UnityEngine;
using UnityEngine.AI;
namespace Assets.Code
{
    public class EnemyController : MonoBehaviour
    {
        public float lookRadius = 10f;
        Transform target;
        NavMeshAgent agent;
        CharacterCombat combat;
        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            target = PlayerManager.instance.player.transform;
            combat = GetComponent<CharacterCombat>();
        }
        private void Update()
        {
            float distance = Vector3.Distance(transform.position, target.position);
            if( distance <= lookRadius)
            {
                agent.SetDestination(target.position);
                if(distance <= agent.stoppingDistance)
                {
                    CharacterStats targetStats = target.GetComponent<CharacterStats>();
                    if(targetStats!=null)
                    {
                        combat.Attack(targetStats);
                    }
                    FaceTarget();
                }
            }
        }

        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 4f);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, lookRadius);
        }
    }
}
