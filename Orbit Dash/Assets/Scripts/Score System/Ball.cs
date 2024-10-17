using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            ScoreManager.Instance.AddScore(1);

            // Yem'in pozisyonunu al
            Vector3 collectiblePosition = other.transform.position;

            // SpawnManager'a collectible'ý ve pozisyonunu silmesi talimatýný ver
            SpawnManager.Instance.RemoveCollectible(other.gameObject, collectiblePosition);
        }
        else if (other.CompareTag("Death Trap"))
        {
            CanvasManager.Instance.ShowDeathMessage(); 
            Destroy(this.gameObject);
        }
    }

}
