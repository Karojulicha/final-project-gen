using UnityEngine;

public class EndLevel : MonoBehaviour
{
    public bool finisheLevel;
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player") && !finisheLevel)
        {
            finisheLevel = true;
            GameManager.Instance.ChangeSceneSelector(GameManager.TypeScene.HomeScene
            );
        }
    }
}
