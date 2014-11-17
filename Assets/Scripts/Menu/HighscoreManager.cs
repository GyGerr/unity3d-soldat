using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

[Serializable()]
public class HighScore : IComparable<HighScore>
{
	public string PlayerName { get; set; }
	public string DatePlayed { get; set; }
	public float TimePlayed { get; set; }
	public int Level { get; set; }

	public HighScore() { }

	public HighScore (string _PlayerName, string _DatePlayed, float _TimePlayed, int _Level) {
		PlayerName = _PlayerName;
		DatePlayed = _DatePlayed;
		TimePlayed = _TimePlayed;
		Level = _Level;
	}

	public int CompareTo(HighScore other)
	{
		if(other.Level >  Level && other.TimePlayed <= TimePlayed) {
			return 1;
		} else if (other.Level >  Level) {
			return 0;
		}
		
		return other.Level -  Level;
	}
}

public class HighscoreManager {

	public HighscoreManager () {

	}

	public void write(string PlayerName, string DatePlayed, float TimePlayed, int Level) {
		List<HighScore> highScores = this.read();
		if (highScores == null) {
			highScores = new List<HighScore>();
		}

		HighScore record = new HighScore();
		record.PlayerName = PlayerName;
		record.DatePlayed = DatePlayed;
		record.TimePlayed = TimePlayed;
		record.Level = Level;

		highScores.Add (record);

		XmlSerializer writer = new XmlSerializer(typeof(List<HighScore>));
		using (FileStream file = File.Open("highscores.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite))
		{
			writer.Serialize(file, highScores);
		}
	}

	public List<HighScore> read() {
		List<HighScore> rx;
		
		XmlSerializer reader = new XmlSerializer(typeof(List<HighScore>));
		using (FileStream input = File.Open("highscores.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite))
		{
			try {
				rx = reader.Deserialize(input) as List<HighScore>;	
			} catch {
				return null;
			}

		}

		return rx;
	}
}
