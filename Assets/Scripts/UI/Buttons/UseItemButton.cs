using UnityEngine;

public class UseItemButton : MonoBehaviour
{
    public ManagementCharacter managementCharacter;
    public GameObject button;
    void Start()
    {
        managementCharacter.characterInfo.OnCurrentObjectInHandChange += ToggleButton;
    }

    private void ToggleButton(ManagementInteractableObject @object)
    {
        button.SetActive(@object != null && @object.CanUseObject());
    }
}
