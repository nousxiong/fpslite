using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    public Slider healthBar;
    private EnemyController _controller;
    private HealthManager _healthManager;
    private void Start()
    {
        _healthManager = GetComponent<HealthManager>();
        _healthManager.OnDamage.AddListener(UpdateEnemyHealthUI);
        _healthManager.OnDeath.AddListener(Kill);
        _controller = GetComponent<EnemyController>();
        UpdateEnemyHealthUI();
    }
    public void UpdateEnemyHealthUI()
    {
        healthBar.value = (float)_healthManager.Health / (float)_healthManager.maxHealth;
    }
    public void Kill()
    {
        _controller.dead = true;
        _controller.KillEnemy();
    }
}
