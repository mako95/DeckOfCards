using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Core.Models
{
    public class Deck : BaseEntity
    {
        public List<Card> Cards { get; set; }
        public string DeckName { get; }

        public Deck(string deckName)
        {
            DeckName = deckName;
            Cards = new List<Card>();
        }
    }
}
