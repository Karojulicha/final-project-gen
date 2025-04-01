using UnityEngine;

public class ManagementChets : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public ManagementInteractableObject objectInChest;
    public ManagementInteractableObject chest;
    public bool isOpen = false;
    public ManagementKey.TypeKey typeKeyNeeded;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (!isOpen)
        {
            if (managementCharacter.characterInfo.currentObjectInHand.TryGetComponent<ManagementKey>(out ManagementKey managementKey))
            {
                if (managementKey.typeKey == typeKeyNeeded)
                {
                    isOpen = true;
                    animator.SetBool("isActive", true);
                    Destroy(managementKey.gameObject);
                    objectInChest.canInteract = true;
                    chest.canInteract = false;
                }
            }
        }
    }
    public bool CanUseObject() { return false; }

    public void Drop(ManagementCharacter managementCharacter) { }


    public void UseObjectInteract(ManagementCharacter managementCharacter) { }
}
