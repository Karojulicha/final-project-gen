using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ManagementCharacterInputs : MonoBehaviour
{
    public CharacterActions characterActions;
    public CharacterActionsInfo characterActionsInfo;
    void OnEnable()
    {
        characterActions.Enable();        
    }
    void OnDisable()
    {
        characterActions.Disable();
    }
    public void Awake()
    {
        characterActions = new CharacterActions();
        characterActionsInfo = new CharacterActionsInfo();
        InitInputs();
    }
    void InitInputs()
    {
        characterActionsInfo.movementInput = characterActions.CharacterInputs.Movement;
        characterActionsInfo.movementInput.performed += OnMovementInput;
        characterActionsInfo.movementInput.canceled += OnMovementInput;
        characterActionsInfo.jump = characterActions.CharacterInputs.Jump;
        characterActionsInfo.interact = characterActions.CharacterInputs.Interact;
        characterActionsInfo.useObject = characterActions.CharacterInputs.UseObject;
        characterActionsInfo.dropObject = characterActions.CharacterInputs.DropObject;
    }
    private void OnMovementInput(InputAction.CallbackContext context)
    {
        characterActionsInfo.movement = context.ReadValue<Vector2>();
    }
    [Serializable] public class CharacterActionsInfo
    {
        public Vector2 movement = new Vector2();
        public InputAction movementInput = new InputAction();
        public InputAction jump = new InputAction();
        public InputAction interact = new InputAction();
        public InputAction useObject = new InputAction();
        internal InputAction dropObject = new InputAction();
    }
}
