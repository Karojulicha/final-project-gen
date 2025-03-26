using System;
using UnityEngine;

public class ManagementCharacterActions : MonoBehaviour
{
    public ManagementCharacter managementCharacter;
    public LayerMask layerMask;
    public Vector3 offset = Vector3.zero;
    public Vector3 size = Vector3.one;
    public void Actions()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position + offset, size / 2, Vector3.up, Quaternion.identity, 0, layerMask);
        if (hits.Length > 0)
        {
            if (hits[0].collider.gameObject.GetComponent<ManagementInteractableObject>().canInteract)
            {
                managementCharacter.characterInfo.currentObjectInteract = hits[0].collider.gameObject.GetComponent<ManagementInteractableObject>();
            }
            else
            {
                managementCharacter.characterInfo.currentObjectInteract = null;
            }
        }
        else
        {
            managementCharacter.characterInfo.currentObjectInteract = null;
        }
        if (managementCharacter.characterInfo.currentObjectInteract)
        {
            if (managementCharacter.managementCharacterInputs.characterActionsInfo.interact.triggered)
            {
                managementCharacter.characterInfo._currentObjectInteract.Interact(managementCharacter);
            }
        }
        if (managementCharacter.characterInfo.currentObjectInHand)
        {
            if (managementCharacter.managementCharacterInputs.characterActionsInfo.useObject.triggered)
            {
                managementCharacter.characterInfo.currentObjectInHand.UseObjectInteract(managementCharacter);
            }
            else if (managementCharacter.managementCharacterInputs.characterActionsInfo.dropObject.triggered)
            {
                managementCharacter.characterInfo.currentObjectInHand.DropObjectInteract(managementCharacter);
            }
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}
