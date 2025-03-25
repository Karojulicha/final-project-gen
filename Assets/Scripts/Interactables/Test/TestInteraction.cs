using UnityEngine;

public class TestInteraction : MonoBehaviour, ManagementInteractableObject.ICharacterAction
{
    public bool canRotate = false;
    public float speed = 5;
    public bool isUse = false;
    public Transform rotateObject;
    public void Update()
    {            
        if (canRotate) rotateObject.transform.Rotate(Vector3.left * speed);
    }
    public void Drop(ManagementCharacter managementCharacter){}
    public void UseObjectInteract(ManagementCharacter managementCharacter){}
    public void Interact(ManagementCharacter managementCharacter)
    {
        canRotate = !canRotate;
    }
}
