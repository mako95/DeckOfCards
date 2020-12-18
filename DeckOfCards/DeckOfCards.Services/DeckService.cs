using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeckOfCards.Services
{
    public class DeckService : IDeckService
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
            deck.Cards = deck.Cards.OrderBy(c => c.Position).ToList();

            var card = deck.Cards[0];

            if (card.InDeck)
            {
                card.InDeck = false;
                card.Position = deck.Cards.Count;

                foreach (var c in deck.Cards)
                {
                    c.Position--;
                }

                deck.CardCount = CountCardsInDeck(deck);
                return card;
            }
            
            return null;
        }

        public void SplitDeck(Deck deck)
        {
            deck.Cards = deck.Cards.OrderBy(c => c.Position).ToList();
            var firstHalf = new List<int>();
            var secondHalf = new List<int>();
            var half = CountCardsInDeck(deck) / 2;

            foreach (var card in deck.Cards)
            {
                if (card.InDeck)
                {
                    if (firstHalf.Count < half)
                    {
                        firstHalf.Add(card.Position);
                    }
                    else
                    {
                        secondHalf.Add(card.Position);
                    }
                }
            }

            foreach(var card in deck.Cards)
            {
                if (card.InDeck)
                {
                    if (secondHalf.Count > 0)
                    {
                        card.Position = secondHalf[0];
                        secondHalf.RemoveAt(0);
                    }
                    else if(firstHalf.Count > 0)
                    {
                        card.Position = firstHalf[0];
                        firstHalf.RemoveAt(0);
                    }
                }
            }

        }

        public void ShuffleDeck(Deck deck)
        {
            deck.Cards = deck.Cards.OrderBy(c => c.Position).ToList();
            var positions = new List<int>();
            var rnd = new Random();

            for (int i = 0; i < CountCardsInDeck(deck); i++)
            {
                positions.Add(i);
            }

            foreach (var card in deck.Cards)
            {
                if (card.InDeck)
                {
                    var index = rnd.Next(0, positions.Count);
                    card.Position = positions[index];
                    positions.RemoveAt(index);
                }
            }
        }

        private int CountCardsInDeck(Deck deck)
        {
            var count = 0;
            foreach(var card in deck.Cards)
            {
                if (card.InDeck)
                {
                    count++;
                }
            }
            return count;
        }

        private Deck PopulateDeck(string deckName)
        {
            Deck deck = new Deck() { DeckName = deckName };
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
                    var card = new Card() { DeckId = deck.Id, Value = value, Suit = suit };
                    deck.Cards.Add(card);
                    var index = deck.Cards.IndexOf(card);
                    deck.Cards[index].Position = index;
                }
            }

            deck.CardCount = CountCardsInDeck(deck);
            return deck;
        }
    }
}
