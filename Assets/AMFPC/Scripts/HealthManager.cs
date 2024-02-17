using UnityEngine.Events;
using UnityEngine;
[System.Serializable]
public class HealthManager : MonoBehaviour
{
    public int Health
    {
        get { return _health; }
        set
        {
            if (value < 0) { _health = 0; }
            else { _health = Mathf.Clamp(value, 0, maxHealth); }
        }
    }
    private int _health;
    public int maxHealth;

    public UnityEvent OnDeath;
    public UnityEvent OnDamage;
    public UnityEvent OnRestore;
    private void Awake()
    {
        Health = maxHealth;
    }
    public void Damage(int amount)
    {
        if (Health > 0)
        {
            Health -= amount;
            if(OnDamage!=null)
            {
                OnDamage.Invoke();
            }
            
            if (Health <= 0)
            {
                if(OnDeath!=null)
                {
                    OnDeath.Invoke();
                }
                
            }
        }
    }
    public void RestoreHealth()
    {
        Health = maxHealth;
        OnRestore.Invoke();
    }
}
