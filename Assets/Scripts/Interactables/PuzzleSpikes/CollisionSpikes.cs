using UnityEngine;

public class CollisionSpikes : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            gameObject.transform.parent.transform.parent.GetComponent<Animator>().SetBool("isActive", true);
        }
    }
}
