using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ManagementCharacter : MonoBehaviour
{
    public ManagementCharacterInputs managementCharacterInputs;
    public ManagementCharacterMove managementCharacterMove;
    public ManagementCharacterActions managementCharacterActions;
    public ManagementCharacterCounter managementCharacterCounter;
    public CharacterInfo characterInfo = new CharacterInfo();
    void Update()
    {
        if (characterInfo.isActive)
        {
            HandleMove();            
            HandleActions();
            HandleCounter();
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
    public void HandleCounter()
    {
        managementCharacterCounter.Counter();
    }
    [Serializable] public class CharacterInfo
    {
        public GameObject characterObject;
        public bool isActive = false;
        public bool isGrounded => SetGrounded();
        public SerializedDictionary<TypeStatistic, int> statistics;
        public Rigidbody rb;
        public Transform hand;
        public ManagementInteractableObject _currentObjectInteract;
        public event Action<ManagementInteractableObject> OnCurrentObjectInteractChange;
        public ManagementInteractableObject currentObjectInteract
        {
            get => _currentObjectInteract;
            set
            {
                if (_currentObjectInteract != value)
                {
                    _currentObjectInteract = value;
                    OnCurrentObjectInteractChange?.Invoke(_currentObjectInteract);
                }
            }
        }
        public ManagementInteractableObject _currentObjectInHand;
        public event Action<ManagementInteractableObject> OnCurrentObjectInHandChange;
        public ManagementInteractableObject currentObjectInHand
        {
            get => _currentObjectInHand;
            set
            {
                if (_currentObjectInHand != value)
                {
                    _currentObjectInHand = value;
                    OnCurrentObjectInHandChange?.Invoke(_currentObjectInHand);
                }
            }
        }
        public Vector3 offset;
        public Vector3 size;
        public LayerMask layerMask;
        public int FindStatistics(TypeStatistic typeStatistic)
        {
            return statistics[typeStatistic];
        }
        protected bool SetGrounded()
        {
            return GetGroundHits().Count > 0;
        }
        public Dictionary<string, GameObject> GetGroundHits()
        {
            RaycastHit[] hits = Physics.BoxCastAll(characterObject.transform.position + offset, size / 2, Vector3.down, Quaternion.identity, 0, layerMask);

            Dictionary<string, GameObject> objects = new Dictionary<string, GameObject>(hits.Length);

            foreach (var hit in hits)
            {
                string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                if (!objects.ContainsKey(layerName))
                {
                    objects[layerName] = hit.collider.gameObject;
                }
            }
            return objects;
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
