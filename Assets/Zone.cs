using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField]
    private bool isValid = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Note" && isValid)
        {
            Debug.Log("ENTER");
        }
    }

    public bool IsValid
    {
        get { return isValid; }
        set { isValid = value; }
    }
}
