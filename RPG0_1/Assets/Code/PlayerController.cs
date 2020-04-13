using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Code
{
    public class PlayerController
    {
        private Player player;
        private Interactable currentFocus;
        private Transform target;
        bool isTarget = false;
        bool isDestinationReached = false;
        Vector2 positionOfCursor;
        public PlayerController(Player player, CharacterCombat characterCombat)
        {
            this.player = player;
        }

        public void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            if (Input.GetMouseButton(0))
            {
                MoveOntoPosition(GetMousePosition().point);
                RemoveFocus();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                InteractWithScene(GetMousePosition().transform);
            }
            if (isTarget)
                CheckDistance();
            if (target != null)
            {
                isTarget = false;
                player.navMeshAgent.destination = target.position;
                TurnFaceToTarget();
            }
            if (currentFocus != null)
                if (currentFocus.TypeOfInteraction == TypesOfInteraction.Combat)
                {
                    currentFocus = currentFocus as EnemyInteraction;
                    float distance = Vector3.Distance(player.transform.position, currentFocus.transform.position);
                    if (distance <= player.navMeshAgent.stoppingDistance)
                    {
                        currentFocus.Interact();
                    }
                }
            // Debug.Log("isTarget:" + isTarget);
        }

        private void TurnFaceToTarget()
        {
            Vector3 direction = (target.position - player.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, lookRotation, Time.deltaTime * 4f);
        }

        private void InteractWithScene(Transform objectToInteractWith)
        {
            Interactable interactable = objectToInteractWith.GetComponent<Interactable>();
            if (interactable != null)
            {
                Debug.Log(interactable.name);
                SetFocus(interactable);
            }

        }

        private void RemoveFocus()
        {
            if (currentFocus != null)
                currentFocus.OnDefocused();
            currentFocus = null;
            target = null;
            player.navMeshAgent.stoppingDistance = 0;
            player.navMeshAgent.updateRotation = true;

        }
        private void SetFocus(Interactable interactable)
        {
            if (interactable != currentFocus)
            {
                if (currentFocus != null)
                    currentFocus.OnDefocused();

                currentFocus = interactable;
                target = interactable.interactionTransform;
                FollowTarget(interactable);
            }
            interactable.OnFocused(player.transform);
        }

        private void FollowTarget(Interactable interactable)
        {
            player.navMeshAgent.updateRotation = false;
            player.navMeshAgent.stoppingDistance = interactable.radius * .8f;
            player.navMeshAgent.isStopped = false;
        }

        private void CheckDistance()
        {
            if (Vector3.Distance(player.navMeshAgent.transform.position, player.navMeshAgent.destination) < 1f)
            {
                isDestinationReached = true;
                player.navMeshAgent.isStopped = true;
                isTarget = false;
            }
        }

        private RaycastHit GetMousePosition()
        {
            RaycastHit hit;
            Ray ray = player.mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << 10 | 1 << 11 | 1 << 12;
            //layerMask = ~layerMask;
            Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask, QueryTriggerInteraction.Ignore))
            {
                positionOfCursor = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                return hit;
            }
            return hit;
        }
        public void OnGUI()
        {
            if (isTarget)
            {
                GUI.DrawTexture(new Rect(Camera.main.WorldToScreenPoint(player.navMeshAgent.destination).x - 20, Screen.height - Camera.main.WorldToScreenPoint(player.navMeshAgent.destination).y -20, 40, 40), GameResources.Target);
            }
        }

        private void MoveOntoPosition(Vector3 targetOfMovement, bool targeting = true)
        {
            if (targetOfMovement != -Vector3.one)
            {
                player.navMeshAgent.destination = targetOfMovement;
                player.navMeshAgent.isStopped = false;
                isDestinationReached = false;
                isTarget = targeting;
            }
        }
    }
}