using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelConstructor : MonoBehaviour
{
    public GameObject[] slots = new GameObject[3];
    public GameObject levelPrefab;
    public GameObject prefab;
    public string levelName;

    private GameManager gameManager;
    private GameObject savedLevel;
    public List<GameObject> notes;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        notes = new List<GameObject>();

        savedLevel = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        //On déplace la camera pour suivre
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, transform.position.z - 30);

        transform.position += new Vector3(0, 0, gameManager.speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GameObject spawnedNote = Instantiate(prefab, slots[0].transform.position, Quaternion.identity, savedLevel.transform);
            notes.Add(spawnedNote);
            //spawnedNote.GetComponent<Note>().enabled = false;
        }
    }

    private void OnApplicationQuit()
    {
        PrefabUtility.CreatePrefab("Assets/"+levelName+".prefab", savedLevel);
    }
}
