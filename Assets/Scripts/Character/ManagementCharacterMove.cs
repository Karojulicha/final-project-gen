using UnityEngine;

public class ManagementCharacterMove : MonoBehaviour
{
    public Animator am;
    public Transform model;
    public ManagementCharacter managementCharacter;
    Vector3 movementDirection = new Vector3();
    public void Move()
    {
        am.SetBool("isJump", !managementCharacter.characterInfo.isGrounded);
        Vector2 inputs = managementCharacter.managementCharacterInputs.characterActionsInfo.movement;
        if (inputs != Vector2.zero)
        {
            float angle = Mathf.Atan2(inputs.x, inputs.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, angle, 0);
            model.rotation = Quaternion.Lerp(model.rotation, targetRotation, Time.deltaTime * 10);
        }
        am.SetBool("isWalk", inputs != Vector2.zero);
        float speed = managementCharacter.characterInfo.FindStatistics(ManagementCharacter.TypeStatistic.Speed);
        HandleJump();
        movementDirection = new Vector3(inputs.x * speed, managementCharacter.characterInfo.rb.linearVelocity.y, inputs.y * speed);
        managementCharacter.characterInfo.rb.linearVelocity = movementDirection;
    }
    void HandleJump()
    {
        if (managementCharacter.characterInfo.isGrounded && managementCharacter.managementCharacterInputs.characterActionsInfo.jump.triggered)
        {
            managementCharacter.characterInfo.rb.AddForce(
                Vector3.up * managementCharacter.characterInfo.FindStatistics(ManagementCharacter.TypeStatistic.JumpForce),
                ForceMode.Impulse
            );
        }
    }
}
