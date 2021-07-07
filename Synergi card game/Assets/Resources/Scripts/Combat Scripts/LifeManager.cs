using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UnityEngine;
using UnityEngine.UI;

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
    public int Life { get; private set; }
    public int OpposingLife { get; private set; }

    private NetworkVariable<int> Player1Life = new NetworkVariable<int>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    private NetworkVariable<int> Player2Life = new NetworkVariable<int>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

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

    public override void NetworkStart()
    {
        if (IsHost)
        {
            Player1Life.Value = 20;
            Player2Life.Value = 20;
        }
    }

    void Update()
    {

        if (IsHost)
        {
            Life = Player1Life.Value;
            OpposingLife = Player2Life.Value;
            //print($"Called in host: P1: {Life}, \n P2: {OpposingLife}");
        }
        else if (IsClient)
        {
            Life = Player2Life.Value;
            OpposingLife = Player1Life.Value;
            //print($"Called in client: P1: {Life}, \n P2: {OpposingLife}");
        }

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
        
        if (IsHost)
        {
            SendDamagePlayerFromHostServerRpc(damageType, damageAmount);
        }
        //if this is any client aside from the host...
        else if (IsClient)
        {
            SendDamagePlayerFromClientOnlyServerRpc(damageType, damageAmount);
        }

        //lifeAmount.text = $"Life: {Life}";
    }

    /// <summary>
    /// Purpose: Host deals damage to Client
    /// Restrictions: Meant for 2 players
    /// </summary>
    /// <param name="damageType">type of damage dealt</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendDamagePlayerFromHostServerRpc(DamageTypes damageType, int damageAmount)
    {
        //print("message received from host");
        //For now, these 2 cases do the same thing. 
        //Later, they'll broadcast different messages to GameManager.
        //This will allow for different effects.
        switch (damageType)
        {
            case DamageTypes.Effect:
                Player2Life.Value -= damageAmount;
                break;
            case DamageTypes.Battle:
                Player2Life.Value -= damageAmount;
                //print($"P1: {Player1Life.Value}, \n P2: {Player2Life.Value}");
                break;
        }
    }


    /// <summary>
    /// Purpose: Client deals damage to Host
    /// Restrictions: Meant for 2 players
    /// </summary>
    /// <param name="damageType">type of damage dealt</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    [ServerRpc(RequireOwnership = false)]
    private void SendDamagePlayerFromClientOnlyServerRpc(DamageTypes damageType, int damageAmount)
    {
        //For now, these 2 cases do the same thing. 
        //Later, they'll broadcast different messages to GameManager.
        //This will allow for different effects.
        //print("received message from client");
        switch (damageType)
        {
            case DamageTypes.Effect:
                Player1Life.Value -= damageAmount;
                break;
            case DamageTypes.Battle:
                Player1Life.Value -= damageAmount;
                //print($"P1: {Player1Life.Value}, \n P2: {Player2Life.Value}");
                break;
        }
    }
}   
