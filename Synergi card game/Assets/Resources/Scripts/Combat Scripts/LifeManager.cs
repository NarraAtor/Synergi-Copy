using UnityEngine;
using UnityEngine.UI;
using Mirror;
public enum DamageTypes
{
    Effect,
    Battle
}
/// <summary>
/// Purpose: Manages each player's life total.
///          Also adding in networking. 
///          Assume Host is Player1. Each player will have a different perspective depending on their POV.
/// </summary>
public class LifeManager : NetworkBehaviour
{
    [SerializeField] private GameObject portrait;
    [SerializeField] private GameObject opposingPortrait;
    private Text lifeAmount;
    private Text opposingLifeAmount;
    [SyncVar]
    private int life;
    [SyncVar]
    private int opposingLife;
    public int Life
    {
        get
        {
            return life;
        }

        private set
        {
            life = value;
        }
    }
    public int OpposingLife {
        get
        {
            return opposingLife;
        }

        private set
        {
            opposingLife = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in portrait.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name == "Health")
            {
                lifeAmount = child.gameObject.GetComponent<Text>();
            }
        }
        foreach (Transform child in opposingPortrait.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name == "Health")
            {
                opposingLifeAmount = child.gameObject.GetComponent<Text>();
            }
        }
        Life = 20;
        OpposingLife = 20;
    }

    void Update()
    {
        lifeAmount.text = $"Life: {Life} ";
        opposingLifeAmount.text = $"Life: {OpposingLife} ";
        //print($"P1: {Life}, \n P2: {OpposingLife}");

    }
    /// <summary>
    /// Purpose: Reduces the player's life based on the damage dealt.
    ///          This was made as a method so that players could do things like paying life without any problem later.
    ///          Note: This doesn't need a portrait as a parameter since I choose which portrait I'm calling
    ///          the method on when calling the funciton from elsewhere (each portrait has a LifeManager).
    /// Restrictions:
    /// </summary>
    /// <param name="damageType">what type of damage was dealt, going to be used for effects</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    public void DamagePlayer(DamageTypes damageType, int damageAmount)
    {

        //lifeAmount.text = $"Life: {Life}";
    }

}
