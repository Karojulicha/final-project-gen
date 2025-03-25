using System;
using UnityEngine;
using UnityEngine.UI;

public class ManagementCharacterHUD : MonoBehaviour
{
    public ManagementCharacter managementCharacter;
    public Image fillBar;
    void Start()
    {
        managementCharacter.managementCharacterCounter.CounterChanged += UpdateBar;
    }

    private void UpdateBar(float amount)
    {
        fillBar.fillAmount = amount / 1f;
    }
}
