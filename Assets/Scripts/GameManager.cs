using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Camera;
    private Vector3 initialCameraPos;

    public GameObject submarine;
    private GameController submarineController;

    private Transform submarineInterior;
    private Transform podAttached;
    private Transform subMarineModel;

    public GameObject playerPrefab;
    private GameObject playerObject;
    private GameController playerObjectController;

    public float distanceToEnterInSubmarine = 3.0f;

    private float madnessPercentage = 0;
    public float madnessIncreaseValue = 1;

    private bool maxMadnessReached = false;

    private bool canSwitch = true;

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
        podAttached = submarine.transform.Find("PodAttached");
        subMarineModel = submarine.transform.Find("SubmarineModel");


        initialCameraPos = Camera.transform.position;
        submarineController = submarine.GetComponent<GameController>();

        if (submarineController)
        {
            submarineController.SetIsInputActivate(pState == PlayerState.SUBMARINE);
        }
        
        if (submarineInterior)
        {
            submarineInterior.gameObject.SetActive(pState == PlayerState.INTERIOR);
        }
        
        if(pState == PlayerState.POD)
        {
            SpawnPod();
        }

        if (podAttached)
        {
            podAttached.gameObject.SetActive(pState != PlayerState.POD);
        }

        if (subMarineModel)
        {
            subMarineModel.gameObject.SetActive(pState == PlayerState.SUBMARINE);
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
        if (Input.GetButtonDown("Interaction") && pState == PlayerState.SUBMARINE && canSwitch)
        {
            ExitDrivingMode();
            //if (submarineInterior.gameObject.activeInHierarchy && pState == PlayerState.INTERIOR)
            //{
            //    submarineInterior.gameObject.SetActive(false);
            //    pState = PlayerState.SUBMARINE;
            //    submarineController.SetIsInputActivate(true);
            //    subMarineModel.gameObject.SetActive(true);
            //}
            //else if(pState == PlayerState.SUBMARINE)
            //{
            //    submarineInterior.gameObject.SetActive(true);
            //    pState = PlayerState.INTERIOR;
            //    submarineController.SetIsInputActivate(false);
            //    subMarineModel.gameObject.SetActive(false);
            //}
        }

        //if (pState == PlayerState.POD && playerObject && Input.GetButtonDown("SwitchExterior"))
        //{
        //    if (Vector3.Distance(submarine.transform.position, playerObject.transform.position) < distanceToEnterInSubmarine)
        //    {
        //        pState = PlayerState.INTERIOR;
        //        submarineInterior.gameObject.SetActive(true);
        //        submarineController.SetIsInputActivate(false);
        //        subMarineModel.gameObject.SetActive(false);


        //        //pState = PlayerState.SUBMARINE;
        //        DispawnPlayer();
        //        //submarineController.SetIsInputActivate(true);
        //    }
        //}
    }

    void SpawnPod()
    {
        playerObject = Instantiate(playerPrefab, submarine.transform.position + new Vector3(-1, submarine.transform.localScale.y / 2 - 2.5f, 0), new Quaternion(0, 0, 0, 0));
        playerObjectController = playerObject.GetComponent<GameController>();

        if (playerObjectController)
        {
            playerObjectController.SetIsInputActivate(true);
        }

        if (podAttached)
        {
            podAttached.gameObject.SetActive(false);
        }
    }

    void DispawnPlayer()
    {
        playerObject.SetActive(false);
        Destroy(playerObject, 0.1f);
        playerObject = null;
        playerObjectController = null;

        if (podAttached)
        {
            podAttached.gameObject.SetActive(true);
        }
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

    public void SwitchToPod()
    {
        submarineInterior.gameObject.SetActive(false);
        subMarineModel.gameObject.SetActive(true);
        submarineController.SetIsInputActivate(false);
        SpawnPod();
        pState = PlayerState.POD;
    }

    public void EnterInSubmarine()
    {
        pState = PlayerState.INTERIOR;
        submarineInterior.gameObject.SetActive(true);
        submarineController.SetIsInputActivate(false);
        subMarineModel.gameObject.SetActive(false);
        DispawnPlayer();
    }

    public void EnterDrivingMode()
    {
        submarineInterior.gameObject.SetActive(false);
        pState = PlayerState.SUBMARINE;
        submarineController.SetIsInputActivate(true);
        subMarineModel.gameObject.SetActive(true);
        StartCoroutine(CanSwitchMode());
    }

    void ExitDrivingMode()
    {
        pState = PlayerState.INTERIOR;
        submarineInterior.gameObject.SetActive(true);
        submarineController.SetIsInputActivate(false);
        subMarineModel.gameObject.SetActive(false);
    }

    public void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void IncreaseMadness()
    {
        madnessPercentage += madnessIncreaseValue * Time.deltaTime;

        if (madnessPercentage >= 100 && !maxMadnessReached)
        {
            OnMaxMadness();
        }
    }

    void OnMaxMadness()
    {
        Debug.Log("Mais ti es fou!");
        maxMadnessReached = true;
    }

    IEnumerator CanSwitchMode()
    {
        canSwitch = false;
        yield return new WaitForSeconds(0.25f);
        canSwitch = true;
    }


}
