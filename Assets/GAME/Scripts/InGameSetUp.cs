using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameSetUp : MonoBehaviour {

    public Text playerNumberText;
    public Text shadowLordText;

    int playerNumber = 1;
    int shadowLord = 0;

    public void SetCharacterParameters() {
        for (int i = 0; i < playerNumber; i++) {
            PlayerPrefs.SetInt("Player_" + i + "_Hitpoints", Random.Range(6, 10));
            PlayerPrefs.SetInt("Player_" + i + "_AttackFactor", Random.Range(1, 8));
            PlayerPrefs.SetInt("Player_" + i + "_MovementPoints", Random.Range(5, 10));
            PlayerPrefs.SetInt("Player_" + i + "_ActionPoints", Random.Range(1, 3));
            PlayerPrefs.SetInt("Player_" + i + "_DefenceFactor", Random.Range(0, 3));
        }
        PlayerPrefs.SetInt("ShadowLord_Active", shadowLord);
    }

    public void SetShadowLord() {
        shadowLord = shadowLord == 0 ? 1 : 0;
        shadowLordText.text = shadowLord == 1 ? "Shadow Lord: \n Yes" : "Shadow Lord: \n No";
    }

    public void SetPlayerNumber() {
        if (playerNumber < 6)
            playerNumber++;
        else
            playerNumber = 1;
        playerNumberText.text = "Number of Players: \n" + playerNumber;
    }
}
