using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.UI;

public class ManagementCharacterHUD : MonoBehaviour
{
    public ManagementCharacter managementCharacter;
    public HudsInfo hudsInfo;
    public Image fillBar;
    void Start()
    {
        ChangeHUD(GameManager.Instance.currentDevice);
        managementCharacter.managementCharacterCounter.CounterChanged += UpdateBar;
        GameManager.Instance.OnDeviceChanged += ChangeHUD;
    }
    void OnDestroy()
    {
        GameManager.Instance.OnDeviceChanged -= ChangeHUD;
    }
    void UpdateBar(float amount)
    {
        fillBar.fillAmount = amount / 1f;
    }
    void ChangeHUD(GameManager.TypeDevice typeDevice)
    {
        bool isPc = typeDevice == GameManager.TypeDevice.PC || typeDevice == GameManager.TypeDevice.GAMEPAD;
        foreach (var hud in hudsInfo.PC)
        {
            hud.SetActive(isPc);
        }
        foreach (var hud in hudsInfo.Mobile)
        {
            hud.SetActive(!isPc);
        }
    }
    [Serializable] public class HudsInfo
    {
        public GameObject[] PC;
        public GameObject[] Mobile;
    }
}
