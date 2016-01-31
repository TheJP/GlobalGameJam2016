using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameSetUp : MonoBehaviour
{

	public Text playerNumberText;
	public Text shadowLordText;

	int playerNumber = 1;
	int shadowLord = 0;

	void Start ()
	{
        playerNumber = PlayerPrefs.GetInt("Players_Number", playerNumber);
        shadowLord = PlayerPrefs.GetInt("Enemy_Controlled", shadowLord);
        UpdatePlayerNumber();
        UpdateShadowLord();
	}

	public void SetCharacterParameters ()
	{
		for (int i = 0; i < playerNumber; i++) {
			PlayerPrefs.SetInt ("Player_" + i + "_Hitpoints", Random.Range (6, 10));
			PlayerPrefs.SetInt ("Player_" + i + "_AttackFactor", Random.Range (1, 8));
			PlayerPrefs.SetInt ("Player_" + i + "_MovementPoints", Random.Range (5, 10));
			PlayerPrefs.SetInt ("Player_" + i + "_ActionPoints", Random.Range (1, 3));
			PlayerPrefs.SetInt ("Player_" + i + "_DefenceFactor", Random.Range (0, 3));
		}
		PlayerPrefs.SetInt ("ShadowLord_Active", shadowLord);
	}

	public void ToggleShadowLord ()
	{
		shadowLord = 1 - shadowLord;
        UpdateShadowLord();
	}

    public void UpdateShadowLord()
    {
        shadowLordText.text = shadowLord == 1 ? "Shadow Lord: " + "Yes" : "Shadow Lord: " + "No";
        PlayerPrefs.SetInt("Enemy_Controlled", shadowLord);
    }

	public void TogglePlayerNumber ()
	{
        playerNumber = (playerNumber % 6) + 1;
        UpdatePlayerNumber();
	}

    public void UpdatePlayerNumber()
    {
        PlayerPrefs.SetInt("Players_Number", playerNumber);
        playerNumberText.text = "No of Players: " + playerNumber;
    }
}
