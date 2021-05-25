using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Battlefield_Zone_Manager : MonoBehaviour
{
    private GameObject[][] battlefieldMatrix;
    private GameObject[] frontRow;
    private GameObject[] middleRow;
    private GameObject[] backRow;
    //For convenience sake, lets add columns
    private GameObject[] leftColumn;
    private GameObject[] centerColumn;
    private GameObject[] rightColumn;

    //positions are abbreviated for convinience.
    [SerializeField] private GameObject FL;
    [SerializeField] private GameObject FC;
    [SerializeField] private GameObject FR;
    [SerializeField] private GameObject ML;
    [SerializeField] private GameObject MC;
    [SerializeField] private GameObject MR;
    [SerializeField] private GameObject BL;
    [SerializeField] private GameObject BC;
    [SerializeField] private GameObject BR;
    // Start is called before the first frame update
    void Start()
    {
        //FL = GameObject.FindGameObjectWithTag("Front Left");
        //FC = GameObject.FindGameObjectWithTag("Front Center");
        //FR = GameObject.FindGameObjectWithTag("Front Right");
        //ML = GameObject.FindGameObjectWithTag("Middle Left");
        //MC = GameObject.FindGameObjectWithTag("Middle Center");
        //MR = GameObject.FindGameObjectWithTag("Middle Right");
        //BL = GameObject.FindGameObjectWithTag("Back Left");
        //BC = GameObject.FindGameObjectWithTag("Back Center");
        //BR = GameObject.FindGameObjectWithTag("Back Right");

        //Assign Positions to each card zone's card.
        FL.GetComponent<Being>().CurrentPosition = CardPositions.FrontLeft;
        FL.GetComponent<Deployable>().CurrentPosition = CardPositions.FrontLeft;
        FL.GetComponent<Card>().CurrentPosition = CardPositions.FrontLeft;

        FC.GetComponent<Being>().CurrentPosition = CardPositions.FrontCenter;
        FC.GetComponent<Deployable>().CurrentPosition = CardPositions.FrontCenter;
        FC.GetComponent<Card>().CurrentPosition = CardPositions.FrontCenter;

        FR.GetComponent<Being>().CurrentPosition = CardPositions.FrontRight;
        FR.GetComponent<Deployable>().CurrentPosition = CardPositions.FrontRight;
        FR.GetComponent<Card>().CurrentPosition = CardPositions.FrontRight;

        ML.GetComponent<Being>().CurrentPosition = CardPositions.MiddleLeft;
        ML.GetComponent<Deployable>().CurrentPosition = CardPositions.MiddleLeft;
        ML.GetComponent<Card>().CurrentPosition = CardPositions.MiddleLeft;

        MC.GetComponent<Being>().CurrentPosition = CardPositions.MiddleCenter;
        MC.GetComponent<Deployable>().CurrentPosition = CardPositions.MiddleCenter;
        MC.GetComponent<Card>().CurrentPosition = CardPositions.MiddleCenter;

        MR.GetComponent<Being>().CurrentPosition = CardPositions.MiddleRight;
        MR.GetComponent<Deployable>().CurrentPosition = CardPositions.MiddleRight;
        MR.GetComponent<Card>().CurrentPosition = CardPositions.MiddleRight;

        BL.GetComponent<Being>().CurrentPosition = CardPositions.BackLeft;
        BL.GetComponent<Deployable>().CurrentPosition = CardPositions.BackLeft;
        BL.GetComponent<Card>().CurrentPosition = CardPositions.BackLeft;

        BC.GetComponent<Being>().CurrentPosition = CardPositions.BackCenter;
        BC.GetComponent<Deployable>().CurrentPosition = CardPositions.BackCenter;
        BC.GetComponent<Card>().CurrentPosition = CardPositions.BackCenter;

        BR.GetComponent<Being>().CurrentPosition = CardPositions.BackRight;
        BR.GetComponent<Deployable>().CurrentPosition = CardPositions.BackRight;
        BR.GetComponent<Card>().CurrentPosition = CardPositions.BackRight;

        frontRow = new GameObject[] { FL, FC, FR };
        middleRow = new GameObject[] { ML, MC, MR };
        backRow = new GameObject[] { BL, BC, BR };
        battlefieldMatrix = new GameObject[][] { frontRow, middleRow, backRow };
        leftColumn = new GameObject[] { FL, ML, BL };
        centerColumn = new GameObject[] { FC, MC, BC };;
        rightColumn = new GameObject[] { FR, MR, BR }; ;
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TROUBLESHOOTING THE DECLARE ATTACKERS ERROR.
        //foreach(GameObject card in frontRow)
        //{
        //    print(card.GetComponent<CardZone>().BeingScript.CurrentPosition);
        //}
        //foreach (GameObject card in middleRow)
        //{
        //    print(card.GetComponent<CardZone>().BeingScript.CurrentPosition);
        //}
        //foreach (GameObject card in backRow)
        //{
        //    print(card.GetComponent<CardZone>().BeingScript.CurrentPosition);
        //}
    }

    //Used for showing the user which card zones they can deploy beings and deployables to and making them selectable.
    public void ShowDeployableZones()
    {
        foreach (GameObject[] row in battlefieldMatrix)
        {
            print(row);
            foreach (GameObject position in row)
            {
                position.SendMessage("ShowDeployableZone");
            }
        }
    }

    //Used for hiding all unoccupied zones to prevent the player from clicking them anymore.
    public void HideDeployableZones()
    {
        foreach (GameObject[] row in battlefieldMatrix)
        {
            //print(row);
            foreach (GameObject position in row)
            {
                position.SendMessage("HideDeployableZone");
            }
        }
    }
}
