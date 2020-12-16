using DeckOfCards.Core.Contracts;
using DeckOfCards.Core.Models;
using DeckOfCards.Core.ViewModels;
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
        IRepository<Deck> deckContext;
        DeckService deckService;
        const string DeckSessionName = "DeckCookie";

        public HomeController()
        {
            deckContext = new InMemoryRepo<Deck>();
            deckService = new DeckService();
        }

        public ActionResult Index()
        {
            var decks = deckContext.Collection();

            return View(decks);
        }

        public ActionResult Create(string deckName)
        {
            var deck = deckService.CreateNewDeck(deckName);
            deckContext.Insert(deck);
            deckContext.Commit();

            return RedirectToAction("Index");
        }

        public ActionResult GetCard(string deckId)
        {
            var deck = deckContext.Find(deckId);
            var card = deckService.DrawCard(deck);

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
            var deck = deckContext.Find(deckId);
            deckService.ShuffleDeck(deck);
            deckContext.Commit();

            return RedirectToAction("Details", new { id = deckId });
        }

        public ActionResult SplitDeck(string deckId)
        {
            var deck = deckContext.Find(deckId);
            deckService.SplitDeck(deck);
            deckContext.Commit();

            return RedirectToAction("Details", new { id = deckId });
        }

        [HttpGet]
        public ActionResult Details(string id)
        {
            var deck = deckContext.Find(id);
            DeckViewModel model = new DeckViewModel(deck.Id, deck.Cards);

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(string deckId)
        {
            var deckToDelete = deckContext.Find(deckId);

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
            deckContext.Delete(deckId);
            deckContext.Commit();

            return RedirectToAction("Index");
        }
    }
}