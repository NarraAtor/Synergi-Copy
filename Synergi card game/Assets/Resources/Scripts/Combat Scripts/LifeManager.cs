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
public class LifeManager : NetworkBehaviour
{
    [SerializeField] private GameObject portrait;
    private NetworkManager networkManager;
    private Text lifeAmount;
    public int Life { get; private set; }
    private NetworkVariable<int> networkLife;

    // Start is called before the first frame update
    void Start()
    {
        networkManager = GameObject.Find("GameManager").GetComponent<NetworkManager>();
        foreach (Transform child in portrait.GetComponentInChildren<Transform>())
        {
            if (child.gameObject.name == "Health")
            {
                lifeAmount = child.gameObject.GetComponent<Text>();
            }
        }
        Life = 20;
        networkLife = new NetworkVariable<int>(Life);
        networkLife.Settings.WritePermission = NetworkVariablePermission.ServerOnly;
        networkLife.Settings.ReadPermission = NetworkVariablePermission.OwnerOnly;
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
    public void DamagePlayer(DamageTypes damageType, int damageAmount)
    {
        print($"Damage Dealt, {damageAmount}");
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
