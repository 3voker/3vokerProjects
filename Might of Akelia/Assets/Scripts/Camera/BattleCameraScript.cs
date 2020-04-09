using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCameraScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    Camera battleCamera;
    [SerializeField]
    Transform enemyTarget;
    [SerializeField]
    Transform playerTarget;

    bool isActive = false;
    bool isInRange = true;
	void Start () {
        battleCamera.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (isActive == false)
        { FollowMainCamera(); }
        else
            FollowEnemy();
	}

    private void FollowEnemy()
    {
        if (isInRange)
        {

        }
     
    }

    private void DeactivateBattleCamera()
    {
        battleCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        isActive = false;
    }

    public void ActivateBattleCamera()
    {
        battleCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        isActive = true;
    }

    private void FollowMainCamera()
    {
        battleCamera.transform.position = mainCamera.transform.position;
        battleCamera.transform.rotation = mainCamera.transform.rotation;
    }


}
