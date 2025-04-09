using System.Collections;
using UnityEngine;

public class PuzzleSpikes : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public float TimeToRestore = 5;
    public Animator[] animator;
    public bool CanUseObject(){return false;}
    public void Drop(ManagementCharacter managementCharacter){}
    public void Interact(ManagementCharacter managementCharacter)
    {
        Enabled();
        StopAllCoroutines();
        StartCoroutine(Disabled());
    }
    public void UseObjectInteract(ManagementCharacter managementCharacter){}

    void Enabled()
    {
        foreach (Animator spike in animator)
        {
            spike.SetBool("isActive", true);
        }
    }
    IEnumerator Disabled()
    {
        yield return new WaitForSeconds(TimeToRestore);
        foreach (Animator spike in animator)
        {
            spike.SetBool("isActive", false);
        }
    }
}
