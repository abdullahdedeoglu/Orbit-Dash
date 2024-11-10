using UnityEngine;

public class Ball : MonoBehaviour
{
    private int scorePoint = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {

            GameManager.Instance.AddScore(scorePoint);

            Vector3 collectiblePosition = other.transform.position;

            SpawnManager.Instance.RemoveCollectible(other.gameObject, collectiblePosition);

            if (!GameManager.Instance.isTutorialCompleted)
            {
                TutorialManager.Instance.NextStep();
            }
        }
        else if (other.CompareTag("Death Trap"))
        {
            if (!GameManager.Instance.isTutorialCompleted)
            {
                TutorialManager.Instance.DeadInTutorial();
            }
            else
            {
                GameManager.Instance.EndGame();
                Destroy(this.gameObject);
            } 
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") && TutorialManager.Instance != null && !GameManager.Instance.isTutorialCompleted && TutorialManager.Instance.currentStep == 0)
        {
            TutorialManager.Instance.NextStep();
        }
    }

}
