using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Camera;
    private Vector3 initialCameraPos;

    public GameObject submarine;
    private GameController submarineController;

    public GameObject playerPrefab;
    private GameObject playerObject;
    private GameController playerObjectController;

    public float distanceToEnterInSubmarine = 3.0f;

    private bool isUsingSubMarine = true;


    // Start is called before the first frame update
    void Start()
    {
        initialCameraPos = Camera.transform.position;
        submarineController = submarine.GetComponent<GameController>();

        if (isUsingSubMarine && submarineController)
        {
            submarineController.SetIsInputActivate(isUsingSubMarine);
        }
        else
        {
            SpawnPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckChangeController();
        UpdateCameraPosition();
    }

    void CheckChangeController()
    {
        if (Input.GetButtonDown("Switch"))
        {
            if (isUsingSubMarine)
            {
                isUsingSubMarine = false;
                SpawnPlayer();
            }
            else if (Vector3.Distance(submarine.transform.position, playerObject.transform.position) < distanceToEnterInSubmarine)
            {
                isUsingSubMarine = true;
                DispawnPlayer();
            }
        }
    }

    void SpawnPlayer()
    {
        submarineController.SetIsInputActivate(false);

        playerObject = Instantiate(playerPrefab, submarine.transform.position + new Vector3(0, submarine.transform.localScale.y / 2 + 1, 0), new Quaternion(0, 0, 0, 0));
        playerObjectController = playerObject.GetComponent<GameController>();

        if (playerObjectController)
        {
            playerObjectController.SetIsInputActivate(true);
        }
    }

    void DispawnPlayer()
    {
        submarineController.SetIsInputActivate(true);

        DestroyImmediate(playerObject);
        playerObject = null;
        playerObjectController = null;
    }

    void UpdateCameraPosition()
    {
        if (isUsingSubMarine)
        {
            Camera.transform.position = submarine.transform.position + initialCameraPos;
        }
        else if (playerObject)
        {
            Camera.transform.position = playerObject.transform.position + initialCameraPos;
        }
    }
}
