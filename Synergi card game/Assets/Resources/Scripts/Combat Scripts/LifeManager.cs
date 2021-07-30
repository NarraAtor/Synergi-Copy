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
        if(isServer)
        {
            lifeAmount.text = $"Life: {Life} ";
            opposingLifeAmount.text = $"Life: {OpposingLife} ";
        }
        else if(isClientOnly)
        {
            lifeAmount.text = $"Life: {OpposingLife} ";
            opposingLifeAmount.text = $"Life: {Life} ";
        }
    }

    public void DamagePlayer(DamageTypes damageType, int damageAmount, GameObject gameObject)
    {
        if(isServer)
        {
            CmdDamagePlayerServer(damageType, damageAmount, gameObject);
        }
        else if(isClientOnly)
        {
            CmdDamagePlayerClient(damageType, damageAmount, gameObject);
        }
    }
    /// <summary>
    /// Purpose: Reduces the player's life based on the damage dealt.
    ///          This was made as a method so that players could do things like paying life without any problem later.
    ///          Note: 7/29/21, added networking capabilities.
    ///          Note:7/30/21, made seperate versions depending on whether this was sent from the server or the client.
    /// Restrictions: Only works with the portrait game objects.
    /// </summary>
    /// <param name="damageType">what type of damage was dealt, going to be used for effects</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    /// <param name="gameObject"> which game player to call the method on</param>
    [Command(requiresAuthority = false)]
    private void CmdDamagePlayerServer(DamageTypes damageType, int damageAmount, GameObject gameObject)
    {
        if(gameObject.Equals(opposingPortrait))
        {
            switch (damageType)
            {
                case DamageTypes.Effect:
                    opposingLife -= damageAmount;
                    break;
                case DamageTypes.Battle:
                    opposingLife -= damageAmount;
                    break;
            }
        }
        else if(gameObject.Equals(portrait))
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
    }

    /// <summary>
    /// Purpose: Reduces the player's life based on the damage dealt.
    ///          This was made as a method so that players could do things like paying life without any problem later.
    ///          Note: 7/29/21, added networking capabilities.
    ///          Note:7/30/21, made seperate versions depending on whether this was sent from the server or the client.
    /// Restrictions: Only works with the portrait game objects.
    /// </summary>
    /// <param name="damageType">what type of damage was dealt, going to be used for effects</param>
    /// <param name="damageAmount">how much damage was dealt</param>
    /// <param name="gameObject"> which game player to call the method on</param>
    [Command(requiresAuthority = false)]
    private void CmdDamagePlayerClient(DamageTypes damageType, int damageAmount, GameObject gameObject)
    {
        if (gameObject.Equals(opposingPortrait))
        {
            switch (damageType)
            {
                case DamageTypes.Effect:
                    life -= damageAmount;
                    break;
                case DamageTypes.Battle:
                    life -= damageAmount;
                    break;
            }
        }
        else if (gameObject.Equals(portrait))
        {
            switch (damageType)
            {
                case DamageTypes.Effect:
                    opposingLife -= damageAmount;
                    break;
                case DamageTypes.Battle:
                    opposingLife -= damageAmount;
                    break;
            }
        }
    }

}
