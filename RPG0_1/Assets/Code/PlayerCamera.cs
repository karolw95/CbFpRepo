using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Code
{
    class PlayerCamera
    {
        private Player player;
        private Camera mainCamera;

        private Vector3 offsetCamera = new Vector3(0, 5, -5);
        private float minZoom = 3f;
        private float maxZoom = 8f;
        private float smoothMovement = 3f;
        private float currentYaw = 0f;

        public PlayerCamera(Player player, Camera mainCamera)
        {
            this.player = player;
            this.mainCamera = mainCamera;
        }
        public void Update()
        {
            mainCamera.orthographicSize -= (Input.GetAxis("Mouse ScrollWheel"))*3f;
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        }
        public void LateUpdate()
        {
            MoveCameraWithPlayer();
        }
        private void MoveCameraWithPlayer()
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.transform.position + offsetCamera, Time.deltaTime * smoothMovement);
        }
    }
}
