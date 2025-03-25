using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ManagementInteractableWhitAnimation : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public bool needKey = false;
    public bool needParts = false;
    public SerializedDictionary<ManagementKey.TypeKey, PartsInfo> parts = new SerializedDictionary<ManagementKey.TypeKey, PartsInfo>();
    public ManagementKey.TypeKey typeKeyNeeded = ManagementKey.TypeKey.General;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (!needKey && !needParts)
        {
            bool toggleState = !animator.GetBool("isActive");
            animator.SetBool("isActive", toggleState);
        }
        else if (needKey && managementCharacter.characterInfo.currentObjectInHand && managementCharacter.characterInfo.currentObjectInHand.TryGetComponent<ManagementKey>(out ManagementKey currentKey))
        {
            if (currentKey.typeKey == typeKeyNeeded)
            {
                animator.SetBool("isActive", true);
                Destroy(managementCharacter.characterInfo.currentObjectInHand.gameObject);
                managementCharacter.characterInfo.currentObjectInHand = null;
            }
        }
        else if (needParts)
        {
            if (managementCharacter.characterInfo.currentObjectInHand && managementCharacter.characterInfo.currentObjectInHand.TryGetComponent<ManagementKey>(out ManagementKey currentPart))
            {
                if (parts.TryGetValue(currentPart.typeKey, out PartsInfo partsInfo))
                {
                    if (!partsInfo.isUse)
                    {
                        partsInfo.isUse = true;
                        managementCharacter.characterInfo.currentObjectInHand.gameObject.transform.SetParent(partsInfo.partPos);
                        managementCharacter.characterInfo.currentObjectInHand.gameObject.transform.localPosition = Vector3.zero;
                        managementCharacter.characterInfo.currentObjectInHand.gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                        managementCharacter.characterInfo.currentObjectInHand = null;
                    }
                }
            }
            else
            {
                bool canOpen = true;
                for (int i = 0; i < parts.Count; i++)
                {
                    if (!parts.ElementAt(i).Value.isUse)
                    {
                        canOpen = false;
                    }
                }
                if (canOpen)
                {
                    animator.SetBool("isActive", true);
                }
            }
        }
    }
    public void Drop(ManagementCharacter managementCharacter){}
    public void UseObjectInteract(ManagementCharacter managementCharacter)
    {
        throw new System.NotImplementedException();
    }
    [Serializable] public class PartsInfo
    {
        public Transform partPos;
        public bool isUse = false;
    }
}
