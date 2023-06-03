using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HuellitasCompleto.Common.Cards
{
    public class MainOptionsCard
    {

        

        public static async Task ToShow(DialogContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(activity: ShowOpciones(), cancellationToken);

        }
        private static IActivity ShowOpciones()
        {
            var optionsAttachments = new List<Attachment>();

            var cardMediosContacto = new HeroCard
            {
                Title = "Medios de comunicación y ubicación.",
                Text = "A tráves de esta opción consulte nuestro teléfono, correo electrónico, ubicación o formulario de contacto ",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/Contactanos.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver medios de comunicación", Value="Ver medios de comunicación", Type= ActionTypes.ImBack}
                }
            };

            var cardProductos = new HeroCard
            {
                Title = "Productos dentro de la tienda.",
                Text = "A tráves de esta opción consulte nuestros productos por categoria y busque lo que mas necesita.",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/Productos.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver productos de la tienda", Value="Ver los productos de la tienda", Type= ActionTypes.ImBack}
                }
            };

            var cardDejarComentario = new HeroCard
            {
                Title = "Dejar un comentario.",
                Text = "Explique sus incomodidades y deje su correo electrónico, nosotros nos contactaremos para resolver sus inquietudes o comentarios.",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/Comentarios.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Dejar un comentario", Value="Dejar un comentario, queja, sugerencia", Type= ActionTypes.ImBack}
                }
            };

            var cardCalificarBot = new HeroCard
            {
                Title = "Calificar su atención a cliente.",
                Text = "Califique como fue su experiencia de atención a través de este bot.",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/Calificar.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Calificar bot", Value="Calificar al bot", Type= ActionTypes.ImBack}
                }
            };

            var cardComoComprarProducto = new HeroCard
            {
                Title = "¿Cómo comprar un producto?.",
                Text = "Se explica de forma general como realizar la compra de un producto.",
                Images= new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/pregunta.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="Como comprar", Type= ActionTypes.ImBack}
                }
            };

            var cardDevolucionProducto = new HeroCard
            {
                Title = "¿Obtener el reembolso de un producto?.",
                Text = "Se explica de forma general como realizar el reembolso de un producto.",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/pregunta.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="Como obtener mi reembolso", Type= ActionTypes.ImBack}
                }
            };

            var cardRedesSociales = new HeroCard
            {
                Title = "Redes sociales.",
                Text = "Siguenos en las redes sociales.",
                Images = new List<CardImage> { new CardImage("https://bothuellitastrgs1.blob.core.windows.net/image/Redes.jpg") },
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Facebook", Value="https://www.facebook.com/Huellitas-Oficial-101020852364253", Type= ActionTypes.OpenUrl},
                    new CardAction(){Title="Instagram", Value="https://www.instagram.com/huellitassjr_oficial/", Type= ActionTypes.OpenUrl},
                    new CardAction(){Title="Twitter", Value="https://twitter.com/huellitassjr", Type= ActionTypes.OpenUrl},
                    new CardAction(){Title="Mix Cloud", Value="https://www.mixcloud.com/EvelynH21/", Type= ActionTypes.OpenUrl}
                }
            };



            optionsAttachments.Add(cardMediosContacto.ToAttachment());
            optionsAttachments.Add(cardProductos.ToAttachment());
            optionsAttachments.Add(cardDejarComentario.ToAttachment());
            optionsAttachments.Add(cardCalificarBot.ToAttachment());
            optionsAttachments.Add(cardComoComprarProducto.ToAttachment());
            optionsAttachments.Add(cardDevolucionProducto.ToAttachment());
            optionsAttachments.Add(cardRedesSociales.ToAttachment());


            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;


        }

    }
}
