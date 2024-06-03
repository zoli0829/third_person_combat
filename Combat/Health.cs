using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDie;

    [SerializeField] private int maxHealth = 100;

    private int health;
    private bool isInvulnerable = false;
    public bool isDead => health == 0;

    private void Start()
    {
        health = maxHealth;
    }

    public void SetInvulnerable(bool isInvulnerable)
    {
        this.isInvulnerable = isInvulnerable;
    }

    public void DealDamage(int damageAmount)
    {
        if (health == 0) 
        { 
            return; 
        }

        if(isInvulnerable) { return; }

        health = Mathf.Max(health - damageAmount, 0);

        OnTakeDamage?.Invoke();

        if( health == 0 )
        {
            OnDie?.Invoke();
        }

        Debug.Log(health);
    }
}
