using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Code
{
    public class HealthUI : MonoBehaviour
    {
        public GameObject uiPrefab;
        public Transform target;
        float visibleTime = 5f;
        float lastMadeVisibleTime;
        Transform ui;
        Image healthSlider;
        Transform cam;

        private void Start()
        {
            cam = Camera.main.transform;
            foreach (Canvas c in FindObjectsOfType<Canvas>())
            {
                if (c.renderMode == RenderMode.WorldSpace)
                {
                    ui = Instantiate(uiPrefab, c.transform).transform;
                    healthSlider = ui.GetChild(0).GetChild(0).GetComponent<Image>();
                    ui.gameObject.SetActive(false);
                    break;
                }
            }
            GetComponent<CharacterStats>().OnHealthChanged += OnHealthChanged;
        }

        void OnHealthChanged(float maxHealth, float curHealth)
        {
            if (ui != null)
            {
                lastMadeVisibleTime = Time.time;
                ui.gameObject.SetActive(true);
                float healthPercent = curHealth / maxHealth;
                healthSlider.fillAmount = healthPercent;
                if(curHealth<=0)
                {
                    Destroy(ui.gameObject);
                }
            }
        }
        private void LateUpdate()
        {
            if (ui != null)
            {
                ui.position = target.position;
                ui.forward = -cam.forward;
                if (Time.time - lastMadeVisibleTime > visibleTime)
                    ui.gameObject.SetActive(false);
            }
        }
    }
}