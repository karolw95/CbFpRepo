using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Code
{
    public class Player : MonoBehaviour
    {
        public Camera mainCamera;
        public NavMeshAgent navMeshAgent { get; set; }


        private PlayerController controller;
        private PlayerCamera playerCamera;
        private PlayerAnimator playerAnimator;
        private CharacterCombat playerCombat;



        private void Start()
        {
            playerCombat = GetComponent<CharacterCombat>();
            controller = new PlayerController(this,playerCombat);
            playerCamera = new PlayerCamera(this, mainCamera);
            navMeshAgent = GetComponent<NavMeshAgent>();
            playerAnimator = GetComponent<PlayerAnimator>();
        }
        private void Update()
        {
            controller.Update();
            playerCamera.Update();
            playerAnimator.Update();
            
        }
        private void LateUpdate()
        {
            playerCamera.LateUpdate();
        }
        private void OnGUI()
        {
            controller.OnGUI();
        }

    }
}