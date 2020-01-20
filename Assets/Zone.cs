using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField]
    private bool isValid = false;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Note" && isValid)
        {
            //la note est validée
            gameManager.score += Mathf.RoundToInt(50 * gameManager.scoreMultiplier);
            gameManager.scoreMultiplier += 0.2f;
            gameManager.scoreText.text = "Score : " + gameManager.score;
            gameManager.multiplierText.text = "Multiplier : " + gameManager.multiplierText;
            Destroy(other.gameObject);
        }
        else
        {
            //La note n'est pas validée
            gameManager.scoreMultiplier = 1;
            gameManager.multiplierText.text = "Multiplier : " + gameManager.multiplierText;
        }
    }

    public void SetIsValid(bool b)
    {
        isValid = b;
    }
}
