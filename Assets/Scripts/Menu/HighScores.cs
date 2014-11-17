using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighScores : MonoBehaviour {
	
	public Texture2D[] slides = new Texture2D[1];
	public float changeTime = 10.0f;
	private int currentSlide = 0;
	private float timeSinceLast = 1.0f;
	private float buttonWidth;
	private float buttonHeight;
	private float screenLeft;
	private float screenTop;
	public int scoreCount = 5;
	public Texture labelTexture;
	
	void Awake() {
		Screen.showCursor = true;
		buttonWidth = Screen.width / 5;
		buttonHeight = Screen.height / 10;
		screenLeft = Screen.width / 2 - buttonWidth / 2;
		screenTop = Screen.height / 2 - buttonHeight /2;
	}
	
	void Start()
	{
		guiTexture.texture = slides[currentSlide];
		guiTexture.pixelInset = new Rect(Screen.width/2, - Screen.height/2, 0,  0);
		currentSlide++;
	}
	
	void FixedUpdate()
	{
		guiTexture.pixelInset = new Rect(Screen.width/2, - Screen.height/2, 0,  0);
		buttonWidth = Screen.width / 5;
		buttonHeight = Screen.height / 10;
		screenLeft = Screen.width / 2 - buttonWidth / 2;
		screenTop = Screen.height / 2 - buttonHeight /2;
		
		// Fade to next slide
		if(timeSinceLast > changeTime && currentSlide < slides.Length) {
			guiTexture.texture = slides[currentSlide];
			timeSinceLast = 0.0f;
			currentSlide++;
		}
		
		// Slide loop
		if(currentSlide == slides.Length) {
			currentSlide = 0;
		}
		
		timeSinceLast += Time.deltaTime;
	}

	void highscore() {
		List<HighScore> highscores = getTop (scoreCount);
		for (int i = 1; i < highscores.Count; i++) {
			GUI.DrawTexture(new Rect(screenLeft - buttonWidth / 2, screenTop / 2 + 50 * i, buttonWidth * 2, buttonHeight - 30), labelTexture);
			GUI.Label(new Rect(screenLeft - buttonWidth / 3 , screenTop / 2 + 50 * i, buttonWidth, buttonHeight), highscores[i-1].PlayerName);
			GUI.Label(new Rect(screenLeft - buttonWidth / 3, screenTop / 2 + 50 * i + 15, buttonWidth, buttonHeight), highscores[i-1].DatePlayed);
			GUI.Label(new Rect(screenLeft + buttonWidth, screenTop / 2 + 50 * i, buttonWidth, buttonHeight), highscores[i-1].TimePlayed.ToString());
			GUI.Label(new Rect(screenLeft + buttonWidth, screenTop / 2 + 50 * i + 15, buttonWidth, buttonHeight), "Level: " + highscores[i-1].Level.ToString());

		}
	}
	
	void OnGUI() {
		buttonWidth = Screen.width / 5;
		buttonHeight = Screen.height / 10;
		screenLeft = Screen.width / 2 - buttonWidth / 2;
		screenTop = Screen.height / 2 - buttonHeight /2;

		highscore ();
		
		if(GUI.Button(new Rect(screenLeft, screenTop + 3 * buttonHeight, buttonWidth, buttonHeight), "Back")) {
			Application.LoadLevel(0);
		}
	}

	public List<HighScore> getTop(int count = 5) {
		HighscoreManager hm = new HighscoreManager ();
		//hm.write ("Andrzej", "Dzisiaj", 100, 3);
		//hm.write ("Andrzej", "Dzisiaj", 50, 5);

		List<HighScore> hsc = hm.read ();
		if (hsc == null) {
			hsc = new List<HighScore> ();
		}

		hsc.Sort ();

		List<HighScore> highscore = new List<HighScore> ();
		for (int i = 0; i < count; i++) {
			try {
				highscore.Add(hsc[i]);	
			} catch {
				hsc.Add(new HighScore("", "", 0f, 0));				
			}

		}
		return highscore;
	}
}
