using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Code
{
    public class CharacterStats : MonoBehaviour
    {
        public float maxHealth = 100;
        public float currentHealth { get; private set; }
        public Stat damage;
        public Stat armor;

        public event System.Action<float, float> OnHealthChanged;

        public void Awake()
        {
            currentHealth = maxHealth;
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
                TakeDamage(3);
        }
        public void TakeDamage(float damage)
        {
            damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, float.MaxValue);
            currentHealth -= damage;
            Debug.Log(transform.name + " takes " + damage + " damage.");
            if(OnHealthChanged!=null)
            {
                OnHealthChanged(maxHealth, currentHealth);
            }

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Debug.Log(transform.name + " died");

        }
    }
}