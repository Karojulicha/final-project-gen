using System;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ManagementInteractableWhitAnimation : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public Animator animator;
    public bool startActive = false;
    public bool isOpen = false;
    public bool needKey = false;
    public bool needParts = false;

    public AudioClip[] openSoundClip;
    public AudioClip[] closeSoundClip;

    GameManagerHelper gameManagerHelper;

    public SerializedDictionary<ManagementKey.TypeKey, PartsInfo> parts = new SerializedDictionary<ManagementKey.TypeKey, PartsInfo>();
    public ManagementKey.TypeKey typeKeyNeeded = ManagementKey.TypeKey.General;

    void Start()
    {
        animator.SetBool("isActive", startActive);
        gameManagerHelper = FindAnyObjectByType<GameManagerHelper>();
    }
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (isOpen)
        {
            bool toggleState = !animator.GetBool("isActive");
            animator.SetBool("isActive", toggleState);
            if(toggleState)
            {
                foreach(var clip in openSoundClip)
                {
                    gameManagerHelper.PlayASound(clip, 1);
                }
            }else 
            {
                foreach (var clip in closeSoundClip)
                {
                    gameManagerHelper.PlayASound(clip, 1);
                }
            }
            
        }
        else if (!needKey && !needParts)
        {
            isOpen = true;
            animator.SetBool("isActive", true);
            foreach (var clip in openSoundClip)
            {
                gameManagerHelper.PlayASound(clip, 1);
            }
        }
        else if (needKey && managementCharacter.characterInfo.currentObjectInHand && managementCharacter.characterInfo.currentObjectInHand.TryGetComponent<ManagementKey>(out ManagementKey currentKey))
        {
            if (currentKey.typeKey == typeKeyNeeded)
            {
                isOpen = true;
                animator.SetBool("isActive", true);
                foreach (var clip in openSoundClip)
                {
                    gameManagerHelper.PlayASound(clip, 1);
                }
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
                    isOpen = true;
                    animator.SetBool("isActive", true);
                    foreach (var clip in openSoundClip)
                    {
                        gameManagerHelper.PlayASound(clip, 1);
                    }
                }
            }
        }
    }
    public void Drop(ManagementCharacter managementCharacter) { }
    public void UseObjectInteract(ManagementCharacter managementCharacter)
    {
        throw new System.NotImplementedException();
    }

    public bool CanUseObject()
    {
        return false;
    }

    [Serializable]
    public class PartsInfo
    {
        public Transform partPos;
        public bool isUse = false;
    }


}
