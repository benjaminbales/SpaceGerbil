using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Text timerText;

    public float timerLeft;

    private float flashTimer;

    private bool isVisible;

    private GameObject mashObject;

    private bool gameOver;
    public int CollectionToWin = 10;

    public GameObject LoseWindow;
    public GameObject WinWindow;
    public GameObject Player;
    private float distFromPlayer = -5f;

    public AudioSource MainMusic;
    public AudioSource WinMusic;
    public AudioSource LoseMusic;
    public AudioSource TenSecCountDown;
    private bool playOnce = false;

    // Start is called before the first frame update
    void Start()
    {
        flashTimer = .5f;
        isVisible = true;
        gameOver = false;
        MainMusic.gameObject.SetActive(true);
        MainMusic.enabled = true;
        MainMusic.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        TimerCountdown();

        if (timerLeft <= 0 && !gameOver)
        {
            WinLose();
            gameOver = true;
        }

        if ( timerLeft < 10f && !playOnce)
        {
            TenSecCountDown.gameObject.SetActive(true);
            TenSecCountDown.Play();
            playOnce = true;
        }
    }

    private void TimerCountdown()
    {
        if (timerLeft > 0f && !gameOver)
        {
            timerLeft -= Time.deltaTime;

            if (timerLeft > 10f)
            {
                timerText.text = "Time Remaining: " + Mathf.Ceil(timerLeft) + "s";
            }
            else
            {
                flashTimer -= Time.deltaTime;

                //flash red
                timerText.color = Color.red;

                if (isVisible)
                {
                    timerText.text = "Time Remaining: " + Mathf.Ceil(timerLeft) + "s";
                }
                else
                {
                    timerText.text = "";
                }

                if (flashTimer <= 0f)
                {
                    flashTimer = .5f;
                    isVisible = !isVisible;
                }
            }
        }
        else
        {
            gameOver = true;
        }
    }

    private void Win()
    {
        timerText.color = Color.green;
        timerText.text = "YOU WIN!!!";
        RectTransform rectXform = timerText.GetComponent<RectTransform>();
        rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 450, 160);
        rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 250, 30);
        gameOver = true;
        Vector3 playerPos = Player.transform.position;
        playerPos.z = distFromPlayer;
        Camera.main.transform.position = playerPos;
        Gerbil.HasWon();    // hehe
        MouseGrab.OnGameWon();
        timerText.gameObject.SetActive(false);
        MainMusic.Stop();
        MainMusic.gameObject.SetActive(false);
        MainMusic.enabled = false;

        WinWindow.SetActive(true);
        WinMusic.Play();
    }

    private void Lose()
    {
        timerText.color = Color.red;
        timerText.text = "YOU LOSE!!!";
        RectTransform rectXform = timerText.GetComponent<RectTransform>();
        rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 450, 160);
        rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 250, 30);
        MouseGrab.OnGameWon();  // just to disable mouse function

        timerText.gameObject.SetActive(false);
        LoseWindow.SetActive(true);

        // should be stopped.. 
        MainMusic.Stop();
        MainMusic.enabled = false;
        MainMusic.gameObject.SetActive(false);
        LoseMusic.Play();
    }

    private void WinLose()
    {
        // pretty expensive to call every frame... hmm 
        MashObject[] go = GameObject.FindObjectsOfType<MashObject>();
        //if (GameObject.Find("MashObject"))
        //{
        //    mashObject = GameObject.Find("MashObject");
        //}

        //Debug.Log("mashObject.transform.childCount: " + mo.transform.childCount);

        if( go.Length == 0 )
        {
            Lose();
        }
 

        foreach (MashObject mo in go)
        {
            //win condition... >= 5 objects collected.
            if (mo.transform.childCount >= CollectionToWin)
            {
                Win();
                return;
            }
        }

        Lose();
        
        //if( go.Length == 0 && !WinWindow.activeSelf )
        //{
        //    timerText.color = Color.red;
        //    timerText.text = "YOU LOSE!!!";
        //    RectTransform rectXform = timerText.GetComponent<RectTransform>();
        //    rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 450, 160);
        //    rectXform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 250, 30);
        //    MouseGrab.OnGameWon();  // just to disable the mouse function
        //    timerText.gameObject.SetActive(false);
        //    LoseWindow.SetActive(true);
        //    MainMusic.Stop();
        //    MainMusic.enabled = false;
        //    MainMusic.gameObject.SetActive(false);
        //    LoseMusic.Play();
        //}
    }
}
