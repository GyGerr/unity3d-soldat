using UnityEngine;
using System.Collections;


public class CoreGameLoop : MonoBehaviour {

	private GameObject timeLeftText;

	public GameObject player;
	private HP playerHp;
	private GameObject playerHpText;
	private GameObject playerApText;

	public GameObject playerCarbine;
	private M4A1Controller carbine;
	private Amunition ammo;
	private GameObject playerAmmoText;

	private GameObject gameExitText;
    private bool playerPressedEscape = false;

    private GameObject cheatActivatedText;
	public bool cheatActivated = false;

    public int gameOver = -1;
	public int level = 0;

	private GameObject eventText;

    void Awake()
    {
		timeLeftText = new GameObject();
		timeLeftText.AddComponent<GUIText>();
		timeLeftText.transform.position = new Vector3(0.5f, 0.99f, 0.0f);
		timeLeftText.guiText.text = "20:00";
		timeLeftText.name = "TimeLeftText";

		gameExitText = new GameObject();
		gameExitText.AddComponent<GUIText>();
		gameExitText.transform.position = new Vector3(0.05f, 0.99f, 0.0f);
		gameExitText.guiText.text = "";
		gameExitText.name = "GameExitText";

		cheatActivatedText = new GameObject();
		cheatActivatedText.AddComponent<GUIText>();
		cheatActivatedText.transform.position = new Vector3(0.85f, 0.99f, 0.0f);
		cheatActivatedText.guiText.alignment = TextAlignment.Right;
		cheatActivatedText.guiText.text = "";
		cheatActivatedText.name = "CheatActivatedText";

		playerHp = player.GetComponent<HP> ();

		playerHpText = new GameObject ();
		playerHpText.AddComponent<GUIText>();
		playerHpText.transform.position = new Vector3(0.15f, 0.05f, 0.0f);
		playerHpText.guiText.text = "HP: 100";
		playerHpText.name = "PlayerHpText";

		playerApText = new GameObject ();
		playerApText.AddComponent<GUIText>();
		playerApText.transform.position = new Vector3(0.05f, 0.05f, 0.0f);
		playerApText.guiText.text = "AP: 100";
		playerApText.name = "PlayerApText";

		carbine = playerCarbine.GetComponent<M4A1Controller> ();

		playerAmmoText = new GameObject ();
		playerAmmoText.AddComponent<GUIText>();
		playerAmmoText.transform.position = new Vector3(0.9f, 0.05f, 0.0f);
		playerAmmoText.guiText.text = "0 / 0";
		playerAmmoText.guiText.alignment = TextAlignment.Right;
		playerAmmoText.name = "PlayerAmmoText";

		eventText = new GameObject();
		eventText.AddComponent<GUIText>();
		eventText.transform.position = new Vector3(0.5f, 0.5f, 0.0f);
    }

	void Update() 
	{
		ingameMenu();
	}

	void FixedUpdate () {
		if(player) {
	        showTimer();
			playerHealth();
			playerAmmo();
		}
		GameOver();
	}
	
    void showTimer()
    {
		float timeRemain = 1201 - Time.timeSinceLevelLoad;
		System.TimeSpan now = System.TimeSpan.FromSeconds(timeRemain);
		timeLeftText.guiText.text = System.String.Format("{0:00}:{1:00}", now.Minutes, now.Seconds);
		if (timeRemain <= 0) {
			gameOver = 2;
		}
    }

	void playerHealth() {
		playerHpText.guiText.text = "HP: " + Mathf.Ceil(playerHp.hp);
		playerApText.guiText.text = "AP: " + Mathf.Ceil(playerHp.ap);
	}

	void playerAmmo() {
		ammo = carbine.getAmmo();
		string reload = "\n        ";

		if (ammo.currentMag<=5) {
			reload = "\n[R]eload";
		}
		playerAmmoText.guiText.text = ammo.currentMag + " / " + ammo.reserve + reload;
	}

	/**
	 * -1 = OK
	 * 0 = Mission Complete
	 * +1 = Player was killed
	 * +2 = Time left
	 */
	void GameOver() {
		string eventname = "";

		switch (gameOver)
		{
			case 0:
				//Debug.LogWarning("Mision Complete");
				eventname = "Mision Complete";
				break;
		    case 1:
		        //Debug.LogWarning("Player was killed");
				eventname = "Player was killed";
		        break;
		    case 2:
		        //Debug.LogWarning("Time left");
				eventname = "Time left";
		        break;
		}

		if (eventname != "" && !cheatActivated) {
			HighscoreManager hm = new HighscoreManager();
			System.DateTime now = System.DateTime.Now;
			string timestamp = System.String.Format("{0:0000}-{1:00}-{2:00} {3:00}:{4:00}:{5:00}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);

			hm.write("Player1", timestamp, Time.time, level);
			eventText.guiText.text = eventname;
			gameOver = -1;
		}
	}

    void ingameMenu()
    {
        if (playerPressedEscape == true)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                //Debug.Log("Yes");
				gameExitText.guiText.text = "Are you sure to leave? Y/N\nYes";
				Application.LoadLevel(0);
            }
            if (Input.GetKeyDown(KeyCode.N))
            {
                //Debug.Log("No");
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                //Debug.Log("Menu closed");
				gameExitText.guiText.text = "";
                playerPressedEscape = false;
            }
            if (Input.GetKeyDown(KeyCode.C)) {
            	cheatActivated = true;
            	cheatActivatedText.guiText.text = "Cheat Activated";
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                playerPressedEscape = true;
                //Debug.Log("Are you sure to leave? Y/N");
				gameExitText.guiText.text = "Are you sure to leave? Y/N";
            }
        }
    }

	public bool nextLevel() {
		this.level++;
		if (this.level >= 10) {
			gameOver = 0;
			return true;
		} else {
			return false;
		}
	}
}
