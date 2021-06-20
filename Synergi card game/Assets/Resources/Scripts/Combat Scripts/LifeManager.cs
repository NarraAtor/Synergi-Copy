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
///          Also adding in networking: I'm going to try to use RPCs so that I don't have to keep track 
///          of/make NetworkVariables.
/// </summary>
public class LifeManager : NetworkBehaviour
{
    [SerializeField] private GameObject portrait;
    [SerializeField] private GameObject opposingPortrait;
    private Text lifeAmount;
    private Text opposingLifeAmount;
    public int Life { get; private set; }
    public int OpposingLife { get; private set; }

    public NetworkVariable<int> Player1Life = new NetworkVariable<int>(new NetworkVariableSettings
    {
        WritePermission = NetworkVariablePermission.ServerOnly,
        ReadPermission = NetworkVariablePermission.Everyone
    });

    public NetworkVariable<int> Player2Life = new NetworkVariable<int>(new NetworkVariableSettings
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

    void Update()
    {
        if(IsHost)
        {

        }
        //lifeAmount.text = $"Life: {Life}";
        //opposingLifeAmount.text = $"Life: {OpposingLife}";
    }
    /// <summary>
    /// Purpose: Reduces the player's life based on the damage dealt.
    ///          This was made as a method so that players could do things like paying life without any problem later.
    /// Restrictions:
    /// </summary>
    /// <param name="damageType">what type of damage was dealt, going to be used for effects</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    public void DamagePlayer(DamageTypes damageType, int damageAmount, Turn portraitToManipulate, bool sentFromHost)
    {
        if(IsClient && !sentFromHost && !IsHost)
        {
            SendDamagePlayerServerRpc(damageType, damageAmount, portraitToManipulate);
        }
        else if(IsHost && !sentFromHost)
        {
            //For now, these 2 cases do the same thing. 
            //Later, they'll broadcast different messages to GameManager.
            //This will allow for different effects.

            if(portrait.Equals(portraitToManipulate))
            {
                switch (damageType)
                {
                    case DamageTypes.Effect:
                        Life -= damageAmount;
                        break;
                    case DamageTypes.Battle:
                        Life -= damageAmount;
                        break;
                }

            }
            else if(opposingPortrait.Equals(portraitToManipulate))
            {
                switch (damageType)
                {
                    case DamageTypes.Effect:
                        OpposingLife -= damageAmount;
                        break;
                    case DamageTypes.Battle:
                        OpposingLife -= damageAmount;
                        break;
                }
            }
        }
        else if(IsClient && sentFromHost && !IsHost)
        {
            //For now, these 2 cases do the same thing. 
            //Later, they'll broadcast different messages to GameManager.
            //This will allow for different effects.
            if (portrait.Equals(portraitToManipulate))
            {
                switch (damageType)
                {
                    case DamageTypes.Effect:
                        Life -= damageAmount;
                        break;
                    case DamageTypes.Battle:
                        Life -= damageAmount;
                        break;
                }

            }
            else if (opposingPortrait.Equals(portraitToManipulate))
            {
                switch (damageType)
                {
                    case DamageTypes.Effect:
                        OpposingLife -= damageAmount;
                        break;
                    case DamageTypes.Battle:
                        OpposingLife -= damageAmount;
                        break;
                }
            }
        }
    }

    //Updates the host
    [ServerRpc(RequireOwnership = false)]
    public void SendDamagePlayerServerRpc(DamageTypes damageType, int damageAmount, Turn portraitToManipulate)
    {
        //Make changes on the host's end.
        lifeAmount.text = "received message";
        DamagePlayer(damageType, damageAmount, portraitToManipulate, false);

        //Then send these players
        DamagePlayerClientRpc(damageType, damageAmount, portraitToManipulate);
    }


    //Updates all clients
    [ClientRpc]
    public void DamagePlayerClientRpc(DamageTypes damageType, int damageAmount, Turn portraitToManipulate)
    {
        if (IsHost) { return; }
        DamagePlayer(damageType, damageAmount, portraitToManipulate, true);
    }

}
