using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance { get; private set; }

    public GameObject tutorialCanvas;
    public TextMeshProUGUI tutorialText;

    public GameObject ballPrefab;
    private GameObject tutorialBall;

    private bool tutorialInProgress = false;
    public int currentStep = 0;

    public GameObject collectiblePrefab;
    public GameObject obstaclePrefab;
    public GameObject deathTrapPrefab;

    public Transform plain;

    private bool died = false;
    int count = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Update()
    {
        if (!tutorialInProgress) return;

        if (currentStep == 0 && Input.GetMouseButtonDown(0))
        {
            tutorialCanvas.SetActive(false);
            ActivateGame();
        }
        else if (currentStep == 1 && Input.GetMouseButtonDown(0))
        {
            tutorialCanvas.SetActive(false);
            ActivateGame();
        }
        else if (currentStep == 2 && Input.GetMouseButtonDown(0))
        {
            tutorialCanvas.SetActive(false);
            NextStep();
        }
        else if(currentStep == 3 && Input.GetMouseButtonDown(0))
        {
            tutorialCanvas.SetActive(false);
            NextStep();
        }
        else if(currentStep == 4 && Input.GetMouseButtonDown(0))
        {
            tutorialCanvas.SetActive(false);
            ActivateGame();
        }
        else if (currentStep == 5 && Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1;
            tutorialCanvas.SetActive(false);
            NextStep();
        }
        else if (died && Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1;
            tutorialCanvas.SetActive(false);
            died = false;
        }
    }

    private void ActivateGame()
    {

        Time.timeScale = 1;
        DragAndRotate dragScript = FindObjectOfType<DragAndRotate>();
        if (dragScript != null)
        {
            dragScript.isActive = true;
        }
    }

    public void StartTutorial()
    {
        tutorialInProgress = true;
        
        SpawnBall();

        tutorialCanvas.SetActive(true);
        currentStep = 0;

        ShowCurrentStep();
    }

    private void ShowCurrentStep()
    {
        switch (currentStep)
        {
            case 0:
                tutorialText.text = "Guide the ball to the wall.";
                break;
            case 1:
                tutorialCanvas.SetActive(true);
                tutorialText.text = "Collect items to earn points!";
                break;
            case 2:
                tutorialCanvas.SetActive(true);
                tutorialText.text = "Great! Your score increased.";
                break;
            case 3:
                tutorialCanvas.SetActive(true);
                tutorialText.text = "Obstacles (White ones) will stop you and Death Traps (Red Ones) will kill you!";
                break;
            case 4:
                tutorialCanvas.SetActive(true);
                tutorialText.text = "Collect the item with avoiding obstacles and death traps!";
                break;
            case 5:
                tutorialCanvas.SetActive(true);
                tutorialText.text = "Congrats! You can play the game now";
                break;
        }
    }
    private void SpawnBall()
    {
        count++;
        Debug.Log("I'm here " + count + " times");
        tutorialBall = Instantiate(ballPrefab, new Vector3(0, 0.6f, 0), Quaternion.identity);
    }
    private void SpawnTutorialObject(GameObject spawnedPrefab)
    {
        Vector3 randomPosition;
        Vector3 spawnPosition;

        randomPosition = new Vector3(Random.Range(-0.45f, 0.45f), 1f, Random.Range(-0.45f, 0.45f));

        spawnPosition = plain.TransformPoint(randomPosition);

        GameObject spawnedObject = Instantiate(spawnedPrefab, spawnPosition, plain.rotation);
        spawnedObject.transform.parent = plain;
    }

    public void NextStep()
    {
        currentStep++;
        if (currentStep == 1) 
        {
            Time.timeScale = 0;
            ShowCurrentStep();
            SpawnTutorialObject(collectiblePrefab);
        }
        else if (currentStep == 2)
        {
            Time.timeScale = 0;
            ShowCurrentStep();
        }
        else if (currentStep == 3) 
        {
            ShowCurrentStep();
            SpawnTutorialObject(obstaclePrefab);
            SpawnTutorialObject(deathTrapPrefab);
        }
        else if(currentStep == 4)
        {
            ShowCurrentStep();
            SpawnTutorialObject(collectiblePrefab);
        }
        else if(currentStep == 5)
        {
            Time.timeScale = 0;
            ShowCurrentStep();
        }
        else if(currentStep == 6)
        {
            CompleteTutorial();
        }
    }

    public void DeadInTutorial()
    {
        died = true;
        GameObject temp = tutorialBall;
        Destroy(temp);
        SpawnBall();
        tutorialCanvas.SetActive(true);
        tutorialText.text = "You died, try again!";
        Time.timeScale = 0;
    }

    private void CompleteTutorial()
    {
        tutorialInProgress = false;
        PlayerPrefs.SetInt("TutorialCompleted", 1);
        PlayerPrefs.Save();

        Destroy(tutorialBall);
        GameManager.Instance.RestartGame();
    }
}
