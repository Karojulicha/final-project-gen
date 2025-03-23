using UnityEngine;

public class ManagementInteractableWhitAnimation : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public bool needKey = false;
    public ManagementKey.TypeKey typeKeyNeeded = ManagementKey.TypeKey.General;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (!needKey)
        {
            bool toggleState = !animator.GetBool("isActive");
            animator.SetBool("isActive", toggleState);
        }
        else if (managementCharacter.characterInfo.currentObjectInHand && managementCharacter.characterInfo.currentObjectInHand.TryGetComponent<ManagementKey>(out ManagementKey currentKey))
        {
            if (currentKey.typeKey == typeKeyNeeded)
            {
                animator.SetBool("isActive", true);
                Destroy(managementCharacter.characterInfo.currentObjectInHand.gameObject);
                managementCharacter.characterInfo.currentObjectInHand = null;
            }
        }
    }
    public void Drop(ManagementCharacter managementCharacter){}
    public void UseObjectInteract(ManagementCharacter managementCharacter)
    {
        throw new System.NotImplementedException();
    }
}
