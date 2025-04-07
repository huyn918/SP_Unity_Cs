using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        Application.targetFrameRate = 60;
        Time.timeScale = 1.0f;
    }

    public GameObject normalcat;
    public GameObject fatcat;
    public GameObject piratecat;
    public GameObject retrybtn;
    public GameObject catlevel;
    public Text catleveltext;
    public RectTransform levelfront;
    int level = 0;
    int score = 0;

    void Start()
    {
        InvokeRepeating("MakeCat", 0.0f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void MakeCat()
    {
        Instantiate(normalcat);

        if (level == 1)
        {
            int p = Random.Range(0, 10);
            if (p < 2) Instantiate(normalcat);
        }
        if (level >= 2)
        {
            int p = Random.Range(0, 10);
            if (p < 3) Instantiate(normalcat);
        }
        if (level >= 3)
        {
            int p = Random.Range (0, 10);
            if (p < 5) Instantiate (fatcat);
        }
        if (level >= 4)
        {
            int p = Random.Range(0, 10);
            if (p < 5) Instantiate (piratecat);
        }
           
    }

    public void GameOver()
    {
        retrybtn.SetActive(true);
        Time.timeScale = 0f;
    }

    public void AddScore()
    {
        score++;
        level = score / 5;
        catleveltext.text = level.ToString();
        levelfront.localScale = new Vector3((score-level*5)/5.0f, 1.0f, 1.0f);
    }
}
