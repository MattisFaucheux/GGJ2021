using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject Camera;
    private Vector3 initialCameraPos;

    public GameObject submarine;
    private GameController submarineController;

    private Transform submarineInterior;

    public GameObject playerPrefab;
    private GameObject playerObject;
    private GameController playerObjectController;

    public float distanceToEnterInSubmarine = 3.0f;

    //private bool isUsingSubMarine = true;

    enum PlayerState
    {
        SUBMARINE,
        POD,
        INTERIOR
    }

    private PlayerState pState = PlayerState.SUBMARINE;

    // Start is called before the first frame update
    void Start()
    {
        submarineInterior = submarine.transform.Find("Interior");


        initialCameraPos = Camera.transform.position;
        submarineController = submarine.GetComponent<GameController>();

        if (submarineController)
        {
            submarineController.SetIsInputActivate(pState == PlayerState.SUBMARINE);
        }
        else if (submarineInterior)
        {
            submarineInterior.gameObject.SetActive(pState == PlayerState.INTERIOR);
        }
        else if(pState == PlayerState.POD)
        {
            SpawnPod();
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
        if (Input.GetButtonDown("SwitchInterior") && submarineInterior)
        {
            if (submarineInterior.gameObject.activeInHierarchy && pState == PlayerState.INTERIOR)
            {
                submarineInterior.gameObject.SetActive(false);
                pState = PlayerState.SUBMARINE;
                submarineController.SetIsInputActivate(true);
            }
            else if(pState == PlayerState.SUBMARINE)
            {
                submarineInterior.gameObject.SetActive(true);
                pState = PlayerState.INTERIOR;
                submarineController.SetIsInputActivate(false);
            }
        }

        if (Input.GetButtonDown("SwitchExterior"))
        {
            if (pState == PlayerState.SUBMARINE)
            {
                pState = PlayerState.POD;
                SpawnPod();
                submarineController.SetIsInputActivate(false);
            }
            else if (playerObject && Vector3.Distance(submarine.transform.position, playerObject.transform.position) < distanceToEnterInSubmarine && pState == PlayerState.POD)
            {
                pState = PlayerState.SUBMARINE;
                DispawnPlayer();
                submarineController.SetIsInputActivate(true);
            }
        }
    }

    void SpawnPod()
    {
        playerObject = Instantiate(playerPrefab, submarine.transform.position + new Vector3(0, submarine.transform.localScale.y / 2 + 1, 0), new Quaternion(0, 0, 0, 0));
        playerObjectController = playerObject.GetComponent<GameController>();

        if (playerObjectController)
        {
            playerObjectController.SetIsInputActivate(true);
        }
    }

    void DispawnPlayer()
    {
        DestroyImmediate(playerObject);
        playerObject = null;
        playerObjectController = null;
    }

    void UpdateCameraPosition()
    {
        if (pState != PlayerState.POD)
        {
            Camera.transform.position = submarine.transform.position + initialCameraPos;
        }
        else if (playerObject)
        {
            Camera.transform.position = playerObject.transform.position + initialCameraPos;
        }
    }
}
