using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CardBase{
public class CardImgHandler : MonoBehaviour
{
    public Image cardImage;
    public CardData card;
    private void Start(){
        LoadCard(card);
    }
    public void LoadCard(CardData c){
        if(c== null)
        return;
        card=c;
        //cardImage.sprite=c.cardImage;
    }

}
}
