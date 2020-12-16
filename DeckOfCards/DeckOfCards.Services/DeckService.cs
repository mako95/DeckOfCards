using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;

namespace DeckOfCards.Services
{
    public class DeckService
    {

        public Deck CreateNewDeck(string deckName)
        {
            if (string.IsNullOrEmpty(deckName))
                deckName = "New Deck";

            Deck newDeck = PopulateDeck(deckName);

            return newDeck;
        }

        public Card DrawCard(Deck deck)
        {
            if (deck.Cards.Count > 0)
            {
                var card = deck.Cards[0];
                deck.Cards.RemoveAt(0);

                return card;
            }

            return null;
        }

        public void SplitDeck(Deck deck)
        {
            var firstHalf = new List<Card>();
            var secondHalf = new List<Card>();
            var half = deck.Cards.Count / 2;

            //Split the deck in two parts
            for (int i = 0; i < half; i++)
            {
                firstHalf.Add(deck.Cards[i]);
            }

            for (int i = half; i < deck.Cards.Count; i++)
            {
                secondHalf.Add(deck.Cards[i]);
            }

            deck.Cards.Clear();

            //Invert the two parts
            foreach (var card in secondHalf)
            {
                deck.Cards.Add(card);
            }

            foreach (var card in firstHalf)
            {
                deck.Cards.Add(card);
            }

        }

        public void ShuffleDeck(Deck deck)
        {
            List<Card> shuffledCards = new List<Card>();
            var rnd = new Random();

            while (deck.Cards.Count > 0)
            {
                int index = rnd.Next(0, deck.Cards.Count);
                shuffledCards.Add(deck.Cards[index]);
                deck.Cards.RemoveAt(index);
            }

            foreach (var card in shuffledCards)
            {
                deck.Cards.Add(card);
            }
        }

        private Deck PopulateDeck(string deckName)
        {
            Deck deck = new Deck(deckName);
            string[] suits = { "Spades", "Diamonds", "Clubs", "Hearts" };

            foreach (var suit in suits)
            {
                for (int i = 1; i <= 13; i++)
                {
                    string value;

                    switch (i)
                    {
                        case 1:
                            value = "A";
                            break;
                        case 11:
                            value = "J";
                            break;
                        case 12:
                            value = "Q";
                            break;
                        case 13:
                            value = "K";
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
