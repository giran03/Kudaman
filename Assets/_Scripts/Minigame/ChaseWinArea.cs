using UnityEngine;

public class ChaseWinArea : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player Wins the Chase Minigame!");
            SceneHandler.Instance.LoadSceneWithTransition("Scene_6");
        }
    }
}
