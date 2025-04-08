using System;
using UnityEngine;

public class ManagementCharacterCounter : MonoBehaviour
{
    public AudioClip explosion;
    public GameObject particles;
    public GameObject armature;
    public Animator am;
    public float _counter;
    public event Action<float> CounterChanged;
    public Rigidbody[] ragdols;
    private Vector3 lastPosition;
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
        counter = 1;
        CounterChanged += GameOver;
    }
    public void GameOver(float amount)
    {
        if (amount <= 0 && managementCharacter.characterInfo.isActive)
        {
            managementCharacter.characterInfo.isActive = false;
            am.Play("Die", -1, 0f);
            ActiveDismember();
            Invoke("GoToMenu", 3);
        }
    }
    public void ActiveDismember()
    {
        GameObject particlesInstance = Instantiate(particles, transform.position, Quaternion.identity);
        Destroy(particlesInstance, 2);
        foreach (Rigidbody rb in ragdols)
        {
            rb.transform.SetParent(null);
            rb.isKinematic = false;
            Vector3 randomDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
            rb.AddExplosionForce(100, transform.position + randomDirection, 10);
        }
        GameManager.Instance.PlayASound(explosion, 1);
        Destroy(armature);
    }
    public void GoToMenu()
    {
        GameManager.Instance.ChangeSceneSelector(GameManager.TypeScene.GameOver);
    }
    public bool add = false;
    public ManagementCharacter managementCharacter;
    public void Counter()
    {
        if (managementCharacter.characterInfo.GetGroundHits().Count > 0 && managementCharacter.characterInfo.GetGroundHits().TryGetValue("SafeZone", out GameObject objectHited) ||
            managementCharacter.characterInfo.rb.linearVelocity != Vector3.zero)
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            counter = 0;
        }
    }
}