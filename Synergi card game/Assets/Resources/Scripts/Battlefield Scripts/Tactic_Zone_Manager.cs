using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The purpose of this script is to create and delete cards in the "tactic zone" an expandable grid with no limit.
//The goal is to make the number of cards decrease and increase easily and increase infinitely. I don't want
//a limit for tactic zones. It will need to create tactic card from the tactic prefab.
public class Tactic_Zone_Manager : MonoBehaviour
{
    private List<Tactic> TacticArray;
    //This will be assigned in the editor.
    [SerializeField] private GameObject TacticGrid;
    // Start is called before the first frame update
    void Start()
    {
        TacticArray = new List<Tactic>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //TODO: Create a method to prompt the user to confirm picking the tactic card.

    // Method for whenever a tactic card is played. Adds a tactic card to the data List in this script.
    // Also creates a tactic card prefab and adds it to the tactic zone grid.
    //tacticCard = the tactic to be created.
    public void Add(Tactic tacticCard)
    {
        //These 2 lines of code ensures that script doesn't change the tag of the card in hand.
        Tactic NewTactic = Instantiate<Tactic>(tacticCard, TacticGrid.transform);
        TacticArray.Add(NewTactic);

        //Give each tactic card in the list the "Tactical Field" tag.
        foreach(Tactic card in TacticArray)
        {
            card.tag = "Tactical Field";
        }
    }

    //Method for when it is time for a tactic to leave the field.
    //TODO: send a copy of the card to the graveyard.
    //Removes the designated tactic from the array.
    public void Remove(Tactic tacticCard)
    {
        TacticArray.RemoveAt(TacticArray.IndexOf(tacticCard));
        Destroy(tacticCard.gameObject);
    }
}
