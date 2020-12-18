using DeckOfCards.Core.Models;

namespace DeckOfCards.Core.Contracts
{
    public interface IDeckService
    {
        Deck CreateNewDeck(string deckName);
        Card DrawCard(Deck deck);
        void ShuffleDeck(Deck deck);
        void SplitDeck(Deck deck);
    }
}