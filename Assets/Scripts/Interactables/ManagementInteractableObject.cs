using UnityEngine;

public class ManagementInteractableObject : MonoBehaviour
{
    public bool canInteract = false;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (TryGetComponent<ICharacterAction>(out ICharacterAction characterAction))
        {
            characterAction.Interact(managementCharacter);
        }
    }

    public void UseObjectInteract(ManagementCharacter managementCharacter)
    {
        if (TryGetComponent<ICharacterAction>(out ICharacterAction characterAction))
        {
            characterAction.UseObjectInteract(managementCharacter);
        }
    }
    public void DropObjectInteract(ManagementCharacter managementCharacter)
    {
        if (TryGetComponent<ICharacterAction>(out ICharacterAction characterAction))
        {
            characterAction.Drop(managementCharacter);
        }
    }
    public bool CanUseObject()
    {
        if (TryGetComponent<ICharacterAction>(out ICharacterAction characterAction))
        {
            return characterAction.CanUseObject();
        }
        return false;
    }
    public interface ICharacterAction
    {
        public void Interact(ManagementCharacter managementCharacter);
        public void Drop(ManagementCharacter managementCharacter);
        public void UseObjectInteract(ManagementCharacter managementCharacter);
        public bool CanUseObject();
    }
}
