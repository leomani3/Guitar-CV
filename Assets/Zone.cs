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
            gameManager.score += 50;
            gameManager.scoreMultiplier += 0.2f;
            Destroy(other.gameObject);
            //Debug.Log("ENTER");
        }
        else
        {
            //Debug.Log("LOUPÉ ! ");
        }
    }

    public void SetIsValid(bool b)
    {
        isValid = b;
    }
}
