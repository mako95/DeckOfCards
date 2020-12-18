using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DeckOfCards.Core.Models
{
    public class Card : BaseEntity
    {
        [Required]
        public string DeckId { get; set; }
        public string Value { get; set; }
        public string Suit { get; set; }
        public int Position { get; set; }
        public bool InDeck { get; set; }

        public Card()
        {
            InDeck = true;
        }
    }
}
