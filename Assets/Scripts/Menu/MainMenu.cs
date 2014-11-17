using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture2D[] slides = new Texture2D[1];
	public float changeTime = 10.0f;
	private int currentSlide = 0;
	private float timeSinceLast = 1.0f;
	private float buttonWidth;
	private float buttonHeight;
	private float screenLeft;
	private float screenTop;

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

    void OnGUI() {
    	buttonWidth = Screen.width / 5;
    	buttonHeight = Screen.height / 10;
    	screenLeft = Screen.width / 2 - buttonWidth / 2;
    	screenTop = Screen.height / 2 - buttonHeight /2;

    	if(GUI.Button(new Rect(screenLeft, screenTop - 2 * buttonHeight, buttonWidth, buttonHeight), "Single Player")) {
			Application.LoadLevel(1);
    	}
    	if(GUI.Button(new Rect(screenLeft, screenTop - buttonHeight + 5, buttonWidth, buttonHeight), "Highscore")) {
	        Application.LoadLevel(2);
    	}
    	if(GUI.Button(new Rect(screenLeft, screenTop + buttonHeight, buttonWidth, buttonHeight), "Exit")) {
			Application.Quit();
    	}
    }
}
