using UnityEngine;

public class InteractButton : MonoBehaviour
{
    public ManagementCharacter managementCharacter;
    public GameObject button;
    void Start()
    {
        managementCharacter.characterInfo.OnCurrentObjectInteractChange += ToggleButton;
    }

    private void ToggleButton(ManagementInteractableObject @object)
    {
        button.SetActive(@object != null);
    }
}