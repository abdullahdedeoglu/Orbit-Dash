using UnityEngine;

public class Ball : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Just eat itttt eat ittt eat itttt eat ittt");
            ScoreManager.Instance.AddScore(1); 
            Destroy(other.gameObject); 
        }
    }
}
