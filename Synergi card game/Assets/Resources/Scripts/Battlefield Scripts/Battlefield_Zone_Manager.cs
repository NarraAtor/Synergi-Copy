using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Battlefield_Zone_Manager : MonoBehaviour
{
    private GameObject[][] battlefieldMatrix = new GameObject[3][];
    private GameObject[] frontRow = new GameObject[3];
    private GameObject[] middleRow = new GameObject[3];
    private GameObject[] backRow = new GameObject[3];
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
        frontRow[0] = FL;
        frontRow[1] = FC;
        frontRow[2] = FR;
        middleRow[0] = ML;
        middleRow[1] = MC;
        middleRow[2] = MR;
        backRow[0] = BL;
        backRow[1] = BC;
        backRow[2] = BR;
        battlefieldMatrix[0] = frontRow;
        battlefieldMatrix[1] = middleRow;
        battlefieldMatrix[2] = backRow;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Used forshowing the user which card zones they can deploy beings and deployables to and making them selectable.
    public void ShowDeployableZones()
    {
        foreach (GameObject[] row in battlefieldMatrix)
        {
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
            foreach (GameObject position in row)
            {
                position.SendMessage("HideDeployableZone");
            }
        }
    }
}
