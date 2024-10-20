using UnityEngine;

public class Ball : MonoBehaviour
{
    private int scorePoint = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            //ScoreManager.Instance.AddScore(1);

            GameManager.Instance.AddScore(scorePoint);

            // Yem'in pozisyonunu al
            Vector3 collectiblePosition = other.transform.position;

            // SpawnManager'a collectible'ý ve pozisyonunu silmesi talimatýný ver
            SpawnManager.Instance.RemoveCollectible(other.gameObject, collectiblePosition);
        }
        else if (other.CompareTag("Death Trap"))
        {
            Destroy(this.gameObject);
            GameManager.Instance.EndGame();
        }
    }

}
