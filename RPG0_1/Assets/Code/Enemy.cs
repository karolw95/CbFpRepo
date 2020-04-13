using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
    public class Enemy : MonoBehaviour
    {

        [SerializeField] float viewRange = 110;
        [SerializeField] float densityOfRays = 30;
        [SerializeField] float rotationSpeed = 50f;
        [SerializeField] float rangeOfAttack = 2f;
        [SerializeField] float rangeOfSoundDetection = 5f;
        Vector3 enemiesPosition;


        private GameObject player;
        private NavMeshAgent navMeshAgent;

        private enum typeOfAttitude {neutral = 1,cautious= 2, hostile=3, };
        private int attitude=3;
        private Vector3 rootPosition;
        private Quaternion rootRotation;
        private bool isInRootPosition = true;
        private bool isTargetInFieldView;
        private bool isTargetVisible;
        private bool wasTargetVisible = false;
        private bool isCoroutineStarted = false;
        private bool DEBUGMODE=true;
        private bool isNeedToRotate = false;
        private bool isTargetInCollider;

        private void Start()
        {
            player = PlayerManager.instance.player;
            navMeshAgent = GetComponent<NavMeshAgent>();
            rootPosition = transform.position;
            rootRotation = transform.rotation;
        }
        private void Update()
        {
            if(isNeedToRotate && transform.rotation != rootRotation)
            {
                if (transform.rotation == rootRotation)
                    isNeedToRotate = false;
                SetRootRotation();
            }
            if(wasTargetVisible && navMeshAgent.velocity.magnitude <= .2f &&Vector3.Distance(transform.position,player.transform.position) > GetComponent<SphereCollider>().radius)
            {
                MoveToRootPosition();
                Debug.Log("Wracamy do roota");
                if (Vector3.Distance(transform.position, navMeshAgent.destination) < 1)
                {
                    wasTargetVisible = false;
                    isNeedToRotate = true;
                }
            }

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.tag == player.tag)
            {
                if(DEBUGMODE)
                {
                    print("IsMoving "+IsMoving());
                    print("IsRoot " + isInRootPosition);
                    print("IsTargetFOV " + isTargetInFieldView);
                    print("IsTargetVisi " + isTargetVisible);
                }
                LookingForTarget(other);
                CheckRootPosition();
                ReactOnDetection();
            }
        }

        private void ReactOnDetection()
        {
            switch(attitude)
            {
                case (int)typeOfAttitude.neutral:
                    
                    break;
                case (int)typeOfAttitude.cautious:
                    TrackingTarget();
                    break;
                case (int)typeOfAttitude.hostile:
                    PrepareToAttack(); break;
            }
        }

        private void PrepareToAttack()
        {
            LookingForSoundsOfTarget();
            if (isTargetVisible)
            {
                StopCoroutine("LookAround");
                isCoroutineStarted = false;
                MoveToPlayerPosition();
                if (Vector3.Distance(transform.position, navMeshAgent.destination) <= rangeOfAttack)
                {
                    navMeshAgent.isStopped = true;
                    Attack();
                }
            }
            else
            {
                if (wasTargetVisible)
                {
                    if (!isCoroutineStarted)
                        StartCoroutine("LookAround");
                }
            }
        }

        private void LookingForSoundsOfTarget()
        {
            if (Vector3.Distance(transform.position, player.transform.position) <= rangeOfSoundDetection)
            {
                enemiesPosition = player.transform.position;
                RotateFaceToPlayer();

                StopCoroutine("LookAround");
                isCoroutineStarted = false;
                if (Vector3.Distance(transform.position, player.transform.position) <= rangeOfAttack)
                {
                    navMeshAgent.isStopped = true;
                    Attack();
                }
                else
                {
                    MoveToPlayerPosition();
                }

            }
        }

        private void Attack()
        {
            RotateFaceToPlayer();
            print("ciul ciul");
        }

        private void TrackingTarget()
        {
            if (isTargetVisible)
                RotateFaceToPlayer();
        }

        private void SetRootRotation()
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, Vector3.forward, Time.deltaTime, 0f));
        }

        private void CheckRootPosition()
        {
            isInRootPosition = (Vector3.Distance(transform.position, rootPosition) <= 1f) ? true : false;
        }

        private void LookingForTarget(Collider other)
        {
            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < viewRange * 0.5f)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction.normalized, out hit, GetComponent<SphereCollider>().radius))
                {
                    if (hit.collider.gameObject.tag == player.tag)
                    {
                        isTargetInFieldView = true;
                        isTargetVisible = true;
                        wasTargetVisible = true;
                        enemiesPosition = player.transform.position;
                        Debug.DrawLine(transform.position, hit.point, Color.red);
                        Debug.Log("Jest widoczny");
                    }
                    else
                    {
                        isTargetInFieldView = true;
                        isTargetVisible = false;
                        Debug.DrawLine(transform.position, hit.point, Color.green);
                        Debug.Log("Jest w polu widzenia ale coś zasłania");

                    }

                }
            }
            else
            {
                isTargetInFieldView = false;
                isTargetVisible = false;
            }
        }

        private bool IsInLastTargetPosition()
        {
            if(Vector3.Distance(transform.position,enemiesPosition)<=0.001f)
            {
                return true;
            }
            return false;
        }

        private void MoveToPlayerPosition()
        {
            isInRootPosition = false;
            navMeshAgent.destination = enemiesPosition;
            navMeshAgent.isStopped = false;
        }

        private bool IsMoving()
        {
            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.radius)
                return true;
            return false;

        }

        private void MoveToRootPosition()
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.destination = rootPosition;
       //     isWalkingBack = true;
            enemiesPosition = rootPosition;
        }
        

        IEnumerator LookAround()
        {
            isCoroutineStarted = true;
            print("W koroutinie");
            while (Vector3.Distance(transform.position, navMeshAgent.destination) > rangeOfAttack)
            {
                yield return null;
            };
            navMeshAgent.isStopped = true;
            float currentY = transform.localEulerAngles.y;
            print("Obrót w lewo");
            yield return new WaitForSeconds(1);
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                currentY-45, transform.localEulerAngles.z);
            yield return new WaitForSeconds(2);
            // transform.rotation.SetLookRotation(transform.forward);
            print("Obrót w prawo");

            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x,
                currentY+45, transform.localEulerAngles.z);

            yield return new WaitForSeconds(2);
            print("Wracamy");

            MoveToRootPosition();
            yield return new WaitForSeconds(.1f);

            while(true){
                if (Vector3.Distance(transform.position, navMeshAgent.destination) > 1)

                    yield return new WaitForSeconds(2);
                else
                    break;
                
            }
            print("Odwracamy się");


            isNeedToRotate = true;
            wasTargetVisible = false;
            isCoroutineStarted = false;
            
        }

        private void RotateFaceToPlayer()
        {
            var targetRotation = Quaternion.LookRotation(enemiesPosition - transform.position);
            targetRotation = targetRotation.normalized;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime*rotationSpeed);
        }

        private bool CheckFieldView() // moja wersja
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, viewRange);
            for (float i = 0; i <= densityOfRays; i++)
            {
                var startAngle = -transform.right + transform.forward / 2;
                var dirLeft = startAngle - (-transform.right * i / densityOfRays) + transform.forward / 2 * i / densityOfRays;
                if (Physics.Raycast(transform.position, dirLeft, out hit, viewRange, 1 << 8))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    enemiesPosition = hit.point;
                    return true;
                }
                startAngle = transform.right + transform.forward / 2;
                var dirRight = startAngle - transform.right * i / densityOfRays + transform.forward / 2 * i / densityOfRays;
                if (Physics.Raycast(transform.position, dirRight, out hit, viewRange, 1 << 8))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    enemiesPosition = hit.point;
                    return true;
                }
            }
            return false;
        }
        private bool CheckFieldView2()
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, viewRange);
            for (float i = 0; i <= densityOfRays; i++)
            {
                var startAngle = -transform.right + transform.forward / 2;
                var dirLeft = startAngle - (-transform.right * i / densityOfRays) + transform.forward / 2 * i / densityOfRays;
                if (Physics.Raycast(transform.position, dirLeft, out hit, viewRange, 1 << 8))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    enemiesPosition = hit.point;
                    return true;
                }
                startAngle = transform.right + transform.forward / 2;
                var dirRight = startAngle - transform.right * i / densityOfRays + transform.forward / 2 * i / densityOfRays;
                if (Physics.Raycast(transform.position, dirRight, out hit, viewRange, 1 << 8))
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green);
                    enemiesPosition = hit.point;
                    return true;
                }
            }
            return false;
        }
    }
}