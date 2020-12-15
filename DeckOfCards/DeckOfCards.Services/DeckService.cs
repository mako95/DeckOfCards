using DeckOfCards.Core;
using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.Services
{
    public class DeckService
    {
        IRepository<Deck> DeckContext;
        Deck currentDeck;

        public DeckService(IRepository<Deck> deckContext)
        {
            DeckContext = deckContext;
        }

        public void CreateNewDeck(string deckName)
        {
            Deck deck = PopulateDeck(deckName);
            DeckContext.Insert(deck);
            DeckContext.Commit();
        }

        public Deck GetDeck(string id)
        {
            currentDeck = DeckContext.Find(id);

            return currentDeck;
        }

        public Card DrawCard()
        {
            if (currentDeck.Cards.Count > 0)
            {
                var card = currentDeck.Cards[0];
                currentDeck.Cards.RemoveAt(0);

                return card;
            }

            return null;
        }

        public void SplitDeck()
        {
            var firstHalf = new List<Card>();
            var secondHalf = new List<Card>();
            var half = currentDeck.Cards.Count / 2;

            //Split the deck in two parts
            for (int i = 0; i < half; i++)
            {
                firstHalf.Add(currentDeck.Cards[i]);
            }
            for (int i = half; i < currentDeck.Cards.Count; i++)
            {
                secondHalf.Add(currentDeck.Cards[i]);
            }

            currentDeck.Cards.Clear();

            //Invert the two parts
            foreach (var card in secondHalf)
            {
                currentDeck.Cards.Add(card);
            }

            foreach (var card in firstHalf)
            {
                currentDeck.Cards.Add(card);
            }

        }

        public void ShuffleDeck()
        {
            List<Card> shuffledCards = new List<Card>();
            var rnd = new Random();

            while (currentDeck.Cards.Count > 0)
            {
                int index = rnd.Next(0, currentDeck.Cards.Count);
                shuffledCards.Add(currentDeck.Cards[index]);
                currentDeck.Cards.RemoveAt(index);
            }

            foreach (var card in shuffledCards)
            {
                currentDeck.Cards.Add(card);
            }
        }

        private Deck PopulateDeck(string deckName)
        {
            Deck deck = new Deck(deckName);
            string[] suits = { "Spades", "Clubs", "Diamonds", "Hearts" };

            foreach (var suit in suits)
            {
                for (int i = 1; i <= 13; i++)
                {
                    string value;

                    switch (i)
                    {
                        case 1:
                            value = "Ace";
                            break;
                        case 11:
                            value = "Jack";
                            break;
                        case 12:
                            value = "Queen";
                            break;
                        case 13:
                            value = "King";
                            break;
                        default:
                            value = i.ToString();
                            break;

                    }

                    deck.Cards.Add(new Card(value, suit));
                }
            }

            return deck;
        }
    }
}
