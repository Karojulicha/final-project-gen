using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ManagementCharacter : MonoBehaviour
{
    public ManagementCharacterInputs managementCharacterInputs;
    public ManagementCharacterMove managementCharacterMove;
    public ManagementCharacterActions managementCharacterActions;
    public CharacterInfo characterInfo = new CharacterInfo();
    void Update()
    {
        if (characterInfo.isActive)
        {
            HandleMove();            
            HandleActions();
        }
    }
    public void HandleMove()
    {
        managementCharacterMove.Move();
    }
    public void HandleActions()
    {
        managementCharacterActions.Actions();
    }
    [Serializable] public class CharacterInfo
    {
        public GameObject characterObject;
        public bool isActive = false;
        public bool isGrounded => SetGrounded();
        public SerializedDictionary<TypeStatistic, int> statistics;
        public Rigidbody rb;
        public Transform hand;
        public ManagementInteractableObject currentObjectInteract;
        public ManagementInteractableObject currentObjectInHand;
        public Vector3 offset;
        public Vector3 size;
        public LayerMask layerMask;
        public int FindStatistics(TypeStatistic typeStatistic)
        {
            return statistics[typeStatistic];
        }
        protected bool SetGrounded()
        {
            RaycastHit[] hits = Physics.BoxCastAll(characterObject.transform.position + offset, size / 2, Vector3.down, Quaternion.identity, 0, layerMask);
            return hits.Length > 0;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (characterInfo.isGrounded)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireCube(transform.position + characterInfo.offset, characterInfo.size);
    }
    public enum TypeStatistic
    {
        None = 0,
        Speed = 1,
        JumpForce = 2,
    }
}
