using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarVisuals : MonoBehaviour
{
    [SerializeField] private GameObject healthObj;
    [SerializeField] private CombatStateManager combatStateManager;

    private float healthScale;

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {

    }

    public void UpdateUI()
    {
        healthScale = combatStateManager.health / 100;
        healthObj.transform.localScale = Vector3.one - Vector3.right * (1 - healthScale);
    }
}
