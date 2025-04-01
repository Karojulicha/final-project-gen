using UnityEngine;

public class ManagementSpikeGrade : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public float timeToActive = 5f;
    public float currentTime = 0;

    public AudioClip audioSkipe;
    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            animator.SetFloat("isTimeActive", currentTime);
        }

    }
    public void Interact(ManagementCharacter managementCharacter)
    {
        animator.SetFloat("isTimeActive", currentTime);
        currentTime = timeToActive;
        GameManager.Instance.PlayASound(audioSkipe);
    }
    public bool CanUseObject() { return false; }
    public void Drop(ManagementCharacter managementCharacter) { }
    public void UseObjectInteract(ManagementCharacter managementCharacter) { }
}
