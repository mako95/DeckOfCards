using DeckOfCards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeckOfCards.Core.ViewModels
{
    public class DeckViewModel
    {
        public string DeckId { get; set; }
        public List<Card> Cards { get; set; }

        public DeckViewModel(string deckId, List<Card> cards)
        {
            DeckId = deckId;
            Cards = cards;
        }
    }
}
