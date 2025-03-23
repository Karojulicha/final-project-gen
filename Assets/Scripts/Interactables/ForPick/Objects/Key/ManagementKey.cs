using UnityEngine;

public class ManagementKey : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public TypeKey typeKey = TypeKey.General;
    public Rigidbody rb;
    public ManagementInteractableObject managementInteractableObject;
    public Collider cll;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (!managementCharacter.characterInfo.currentObjectInHand)
        {
            managementInteractableObject.canInteract = false;
            managementCharacter.characterInfo.currentObjectInHand = managementInteractableObject;
            transform.SetParent(managementCharacter.characterInfo.hand.transform);
            managementInteractableObject.canInteract = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
            rb.isKinematic = true;
            cll.enabled = false;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
    public void Drop(ManagementCharacter managementCharacter)
    {
        managementInteractableObject.canInteract = true;
        managementCharacter.characterInfo.currentObjectInHand = null;
        transform.SetParent(null);
        rb.isKinematic = false;
        cll.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
    }
    public void UseObjectInteract(ManagementCharacter managementCharacter){}
    public enum TypeKey
    {
        None = 0,
        General = 1,
        Special = 2
    }
}
