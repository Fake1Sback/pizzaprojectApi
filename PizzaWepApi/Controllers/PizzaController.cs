using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using PizzaWepApi.Models;
using PizzaWepApi.Models.ClientModels;

namespace PizzaWepApi.Controllers
{
    [EnableCors(origins:"*",headers:"*",methods:"*")]
    public class PizzaController : ApiController
    {
        private PizzaWebApiDbEntities db = new PizzaWebApiDbEntities();
        [HttpGet]
        public HttpResponseMessage GetPizzas(string category)
        {
            List<Pizzas> dbpizzas;
            if (category == "all")
            {
                dbpizzas = db.Pizzas.ToList();
            }
            else
                dbpizzas = db.Pizzas.Where(p => p.Category == category).ToList();
                
            List<Pizza> pizzaToSend = new List<Pizza>();

            foreach(Pizzas p in dbpizzas)
            {
                Pizza pizza = new Pizza() {Id = p.PizzaId, Name = p.Name, Price = p.Price, Size = p.Size, ImageURL = p.ImageURL };
                pizzaToSend.Add(pizza);
            }

            return Request.CreateResponse(HttpStatusCode.OK, pizzaToSend);
        }

        [HttpGet]
        public HttpResponseMessage GetPizza(int id)
        {
            Pizzas pizza = db.Pizzas.Where(p => p.PizzaId == id).FirstOrDefault();

            if (pizza == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong id");

            return Request.CreateResponse(HttpStatusCode.OK, new Pizza() { Id = pizza.PizzaId, Name = pizza.Name, Price = pizza.Price, ImageURL = pizza.ImageURL, Size = pizza.Size });
        }

        [HttpGet]
        public HttpResponseMessage GetCards()
        {
           List<PizzaWepApi.Models.Card> dbCards = db.Card.ToList();
           List<Models.ClientModels.Card> cardsToSend = new List<Models.ClientModels.Card>();
            foreach (PizzaWepApi.Models.Card dbcard in dbCards)
            {
                string[] additionalImages = dbcard.CardImages.Select(crd => crd.ImageURL).ToArray();
                Models.ClientModels.Card card = new Models.ClientModels.Card() { Title = dbcard.Title, Cols = dbcard.Cols, Rows = dbcard.Rows, ImageURL = dbcard.ImageURL, Content = dbcard.Content, AdditionalImagesURL = additionalImages };
                cardsToSend.Add(card);        
            }
            return Request.CreateResponse(HttpStatusCode.OK, cardsToSend);
        }
    }

    
}
