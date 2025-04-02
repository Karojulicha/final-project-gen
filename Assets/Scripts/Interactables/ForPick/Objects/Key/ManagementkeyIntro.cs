using UnityEngine;
 
public class ManagementKeyIntro : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public float timeToActive = 5f;
    public float currentTime = 0;
 
    public AudioClip audioSkipe;

    void Start()
    {
         animator.SetBool("isActive", false);
    }
    public void Interact(ManagementCharacter managementCharacter)
    {
        animator.SetBool("isActive", true);
        Destroy(gameObject);
    }
    public bool CanUseObject() { return false; }
    public void Drop(ManagementCharacter managementCharacter) { }
    public void UseObjectInteract(ManagementCharacter managementCharacter) { }
}
