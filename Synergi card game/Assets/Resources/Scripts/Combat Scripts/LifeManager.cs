using System.Collections;
using System.Collections.Generic;
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
    private Text lifeAmount;
    public int Life { get; private set; }

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
        Life = 20;
    }

    void Update()
    {
        lifeAmount.text = $"Life: {Life}";
    }
    /// <summary>
    /// Purpose: Reduces the player's life based on the damage dealt.
    ///          This was made as a method so that players could do things like paying life without any problem later.
    /// Restrictions:
    /// </summary>
    /// <param name="damageType">what type of damage was dealt, going to be used for effects</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    public void DamagePlayer(DamageTypes damageType, int damageAmount, bool sentFromServer)
    {
        if(IsClient && !sentFromServer && !IsServer)
        {
            SendDamagePlayerServerRpc(damageType, damageAmount);
        }
        else if(IsServer && !sentFromServer)
        {
            //For now, these 2 cases do the same thing. 
            //Later, they'll broadcast different messages to GameManager.
            //This will allow for different effects.
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
        else if(IsClient && sentFromServer && !IsServer)
        {
            //For now, these 2 cases do the same thing. 
            //Later, they'll broadcast different messages to GameManager.
            //This will allow for different effects.
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
    }

    //Updates the host
    [ServerRpc(RequireOwnership = false)]
    public void SendDamagePlayerServerRpc(DamageTypes damageType, int damageAmount)
    {
        //Make changes on the host's end.
        lifeAmount.text = "received message";
        DamagePlayer(damageType, damageAmount, false);

        //Then send these players
        DamagePlayerClientRpc(damageType, damageAmount);
    }


    //Updates all clients
    [ClientRpc]
    public void DamagePlayerClientRpc(DamageTypes damageType, int damageAmount)
    {
        if (IsServer) { return; }
        DamagePlayer(damageType, damageAmount, true);
    }

}
