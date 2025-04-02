using UnityEngine;

public class puzzleCanvaIntro : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public PuzzleHandler puzzleHandler;

    public bool isActive = false;
    public void Interact(ManagementCharacter managementCharacter)
    {
        if (!isActive)
        {
            isActive = true;
            puzzleHandler.OpenPuzzle();
        }
        
    }
    public void UseObjectInteract(ManagementCharacter managementCharacter) { }
    public bool CanUseObject() { return false; }
    public void Drop(ManagementCharacter managementCharacter) { }
}
