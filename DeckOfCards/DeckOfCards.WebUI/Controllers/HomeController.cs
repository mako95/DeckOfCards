using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using DeckOfCards.DataAccess.InMemory;
using DeckOfCards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeckOfCards.WebUI.Controllers
{
    public class HomeController : Controller
    {
        IRepository<Deck> DeckContext;
        IDeckService DeckService;
        const string DeckSessionName = "DeckCookie";

        public HomeController(IRepository<Deck> deckContext, IDeckService deckService)
        {
            DeckContext = deckContext;
            DeckService = deckService;
        }

        public ActionResult Index()
        {
            var decks = DeckContext.Collection().ToList();

            return View(decks);
        }

        public ActionResult Create(string deckName)
        {
            var deck = DeckService.CreateNewDeck(deckName);
            DeckContext.Insert(deck);
            DeckContext.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult GetCard(string deckId)
        {
            var deck = DeckContext.Find(deckId);
            var card = DeckService.DrawCard(deck);
            DeckContext.Commit();

            if (card != null)
            {
                HttpCookie cookie = Request.Cookies[DeckSessionName];

                if (cookie == null)
                {
                    cookie = new HttpCookie(DeckSessionName);
                }

                cookie.Values[$"{deck.Id}/CardValue"] = card.Value;
                cookie.Values[$"{deck.Id}/CardSuit"] = card.Suit;
                cookie.Expires = DateTime.Now.AddDays(7);
                Response.Cookies.Add(cookie);
            }

            return RedirectToAction("Details", new { id = deckId });
        }

        public ActionResult ShuffleDeck(string deckId)
        {
            var deck = DeckContext.Find(deckId);
            DeckService.ShuffleDeck(deck);
            DeckContext.Update(deck);
            DeckContext.Commit();

            return RedirectToAction("Details", new { id = deckId });
        }

        public ActionResult SplitDeck(string deckId)
        {
            var deck = DeckContext.Find(deckId);
            DeckService.SplitDeck(deck);
            DeckContext.Update(deck);
            DeckContext.Commit();

            return RedirectToAction("Details", new { id = deckId });
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var deck = DeckContext.Find(id);
            deck.Cards = deck.Cards.OrderBy(c => c.Position).ToList();

            return View(deck);
        }

        [HttpGet]
        public ActionResult Delete(string deckId)
        {
            var deckToDelete = DeckContext.Find(deckId);

            if (deckToDelete == null)
            {
                return HttpNotFound();
            }

            return View(deckToDelete);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string deckId)
        {
            DeckContext.Delete(deckId);
            DeckContext.Commit();

            return RedirectToAction("Index");
        }
    }
}