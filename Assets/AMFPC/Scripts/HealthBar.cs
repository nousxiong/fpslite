using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private HealthManager _healthManager;
    private Slider _healthBarSlider;
    public Text healthcoutner;
    private void Awake()
    {
        _healthManager = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthManager>();
    }
    void Start()
    {
        _healthBarSlider = GetComponent<Slider>();
        _healthManager.OnDamage.AddListener(UpdatePlayerHealthUI);
        _healthManager.OnRestore.AddListener(UpdatePlayerHealthUI);
        UpdatePlayerHealthUI();

    }
    private void UpdatePlayerHealthUI()
    {
        _healthBarSlider.value = (float)_healthManager.Health / (float)_healthManager.maxHealth;
        healthcoutner.text = _healthManager.Health.ToString();
    }
}
