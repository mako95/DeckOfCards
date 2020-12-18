using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeckOfCards.Core.Models
{
    public class Deck : BaseEntity
    {
        public string DeckName { get; set; }
        public virtual IList<Card> Cards { get; set; }
        public int CardCount { get; set; }


        public Deck()
        {
            Cards = new List<Card>();
            CardCount = Cards.Count;
        }
    }
}
