using UnityEngine;

public class CardController : MonoBehaviour
{
    [Header("Card Number")]
    public int cardNumber;  // The number this card represents

    // Return the number of the card
    public int GetNumber()
    {
        return cardNumber;
    }

    // Set the number of the card (for spawning cards)
    public void SetNumber(int number)
    {
        cardNumber = number;
    }
}