using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public float speed;
    public int score = 0;
    public float scoreMultiplier = 1;
    public TMP_Text scoreText;
    public TMP_Text multiplierText;

    private void Start()
    {
        scoreText.text = "Score : " + score;
        multiplierText.text = "Multiplier : " + scoreMultiplier;
    }
}
