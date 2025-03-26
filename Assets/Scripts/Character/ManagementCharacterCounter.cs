using System;
using UnityEngine;

public class ManagementCharacterCounter : MonoBehaviour
{    
    public float _counter;
    public event Action<float> CounterChanged;
    public float counter
    {
        get => _counter;
        set
        {
            if (_counter != value)
            {
                _counter = value;
                CounterChanged?.Invoke(_counter);
            }
        }
    }
    void Start()
    {
        CounterChanged += GameOver;
    }
    public void GameOver(float amount)
    {
        if (amount <= 0 && managementCharacter.characterInfo.isActive)
        {
            managementCharacter.characterInfo.isActive = false;
            GameManager.Instance.ChangeSceneSelector(GameManager.TypeScene.GameOver);
        }
    }
    public bool add = false;
    public ManagementCharacter managementCharacter;
    public void Counter()
    {        
        if (managementCharacter.characterInfo.GetGroundHits().Count > 0 && managementCharacter.characterInfo.GetGroundHits().TryGetValue("SafeZone", out GameObject objectHited) || 
            managementCharacter.characterInfo.rb.linearVelocity.x != 0 || managementCharacter.characterInfo.rb.linearVelocity.z != 0)
        {
            add = true;
        }
        else
        {
            add = false;
        }

        counter += add ? Time.deltaTime : -Time.deltaTime;
        counter = Math.Clamp(counter, 0, 1);
    }
}
