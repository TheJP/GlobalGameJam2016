﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameSetUp : MonoBehaviour
{

	public Text playerNumberText;
	public Text shadowLordText;
    public Button creditsButton;

    public GameObject credits;

	int playerNumber = 1;
	int shadowLord = 0;

	void Start ()
	{
        playerNumber = PlayerPrefs.GetInt("Players_Number", playerNumber);
        shadowLord = PlayerPrefs.GetInt("Enemy_Controlled", shadowLord);
        PlayerPrefs.SetInt("Worlds_Escaped", 0);
        PlayerPrefs.SetInt("Enemies_Killed", 0);
        PlayerPrefs.SetInt("Killed_Players", 0);
        PlayerPrefs.SetInt("Damage_Dealt", 0);
        PlayerPrefs.SetInt("Damage_Taken", 0);
        UpdatePlayerNumber();
        UpdateShadowLord();
	}

    void OnLevelWasLoaded(int scene) {
        if (scene == 0) {
            PlayerPrefs.SetInt("Worlds_Escaped", 0);
            PlayerPrefs.SetInt("Enemies_Killed", 0);
            PlayerPrefs.SetInt("Killed_Players", 0);
            PlayerPrefs.SetInt("Damage_Dealt", 0);
            PlayerPrefs.SetInt("Damage_Taken", 0);
        }
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

    public void ShowCredits() {
        credits.SetActive(true);
    }

    public void HideCredits()
    {
        credits.SetActive(false);
    }
}
