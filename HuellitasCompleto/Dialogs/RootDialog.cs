using HuellitasCompleto.Common.Cards;
using HuellitasCompleto.Dialogs.Calificar;
using HuellitasCompleto.Infrestructure.LUIS;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HuellitasCompleto.Dialogs
{
    public class RootDialog: ComponentDialog
    {
        private readonly ILuisService _luisService;

        public RootDialog(ILuisService luisService)
        {
            _luisService = luisService;
            var waterfallSteps = new WaterfallStep[] 
            {
                InitialProcess,
                FinalProcess
            
            };

            AddDialog(new CalificarDialog());
            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId= nameof(WaterfallDialog);
        }

       

        private async Task<DialogTurnResult> InitialProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var luisResult = await _luisService._luisRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
            return await ManageIntentions(stepContext, luisResult, cancellationToken);
        }

        private async Task<DialogTurnResult> ManageIntentions(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            var topIntent = luisResult.GetTopScoringIntent();
            switch(topIntent.intent)
            {
                case "Adios":
                    await IntentAdios(stepContext, luisResult, cancellationToken);
                    break;
                case "Agradecer":
                    await IntentAgradecer(stepContext, luisResult, cancellationToken);
                    break;
                case "Inform":
                    await IntentInformacion(stepContext, luisResult, cancellationToken);
                    break;
                case "Opciones":
                    await IntentOpciones(stepContext, luisResult, cancellationToken);
                    break;
                case "Saludos":
                    await IntentSaludos(stepContext, luisResult, cancellationToken);
                    break;
                case "Productos":
                    await IntentProductos(stepContext, luisResult, cancellationToken);
                    break;
                case "None":
                    await IntentNone(stepContext, luisResult, cancellationToken);
                    break;
                case "Calificar":
                    return await IntentCalificar(stepContext, luisResult, cancellationToken);
                    break;
                case "ComprarProducto":
                    await IntentComprarProducto(stepContext, luisResult, cancellationToken);
                    break;
                case "RedesSociales":
                    await IntentRedesSociales(stepContext, luisResult, cancellationToken);
                    break;
                case "Reembolso":
                    await IntentReembolso(stepContext, luisResult, cancellationToken);
                    break;
                case "BuzonQuejas":
                    await IntentBuzonQuejas(stepContext, luisResult, cancellationToken);
                    break;
                default:
                    await IntentNone(stepContext, luisResult, cancellationToken);
                    break;
            }
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task IntentBuzonQuejas(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Debes de llenar el siguiente formulario y nos pondremos en contacto contigo por correo electrónico: https://docs.google.com/forms/d/e/1FAIpQLSe6cRfI6TcZ2pzssMp_6QcE82jNVcfU7oAD1pe1aN_oDhL8iw/viewform?usp=sf_link ", cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> IntentCalificar(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            return await stepContext.BeginDialogAsync(nameof(CalificarDialog), cancellationToken: cancellationToken);
        }

        private async Task IntentReembolso(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            string informacionReembolso = $"Para realizar un reembozo de alguno de nuestros productos sigue los siguientes pasos: {Environment.NewLine}" +
                $"1. Debes de estar hacerlo desde nuestro chat bot {Environment.NewLine}" +
                $"2. Escribe opciones o reembozo {Environment.NewLine}" +
                $"3. Se te mostrara una opción de formulario de contacto, da clic en ella {Environment.NewLine}" +
                $"4. Ingresa todos los datos que se piden {Environment.NewLine}" +
                $"5. Explica los motivos de tu mensaje (Explica detalladamente porque de tu reembolso)  {Environment.NewLine}" +
                $"5. Envialo. Nosotros nos pondremos en contacto contigo por correo electronico.  {Environment.NewLine}" +
                $" ¡Listo! {Environment.NewLine}" +
                "¿En que mas puedo ayudarte?";

            await stepContext.Context.SendActivityAsync(informacionReembolso, cancellationToken: cancellationToken);
        }

        private async Task IntentRedesSociales(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(activity: ShowRedes(), cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿En que más te puedo ayudar?", cancellationToken: cancellationToken);
        }

        private IActivity ShowRedes()
        {
            var optionsAttachments = new List<Attachment>();

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

            optionsAttachments.Add(cardRedesSociales.ToAttachment());

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;
        }

        private async Task IntentComprarProducto(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            string informacionProducto = $"Para comprar o adquirir alguno de nuestros productos sigue los siguientes pasos: {Environment.NewLine}" +
                $"1. Debes de estar en nuestra página principal: https://huellitaspet.weebly.com/ {Environment.NewLine}" +
                $"2. Entra a la sección Productos que se encuentra en el menú de encabezado {Environment.NewLine}" +
                $"3. Se te mostrara las categorias de productos que ofrecemos. Entra a la categoria de tu preferencia {Environment.NewLine}" +
                $"4. Selecciona el producto que quieras comprar {Environment.NewLine}" +
                $"5. Ingresa la cantidad de productos que quieres comprar, su tamaño o color. Da clic en comprar  {Environment.NewLine}" +
                $"5. Ingresa la forma de pago que usaras, tu dirección y los demás datos que se te pidan  {Environment.NewLine}" +
                $" ¡Listo!, has realizado tu compra. Si tienes problemas con ello puedes enviar un formulario o si quieres el reembolso pidelo. {Environment.NewLine}"+
                "¿En que mas puedo ayudarte?";

            await stepContext.Context.SendActivityAsync(informacionProducto, cancellationToken: cancellationToken);
        }

        

        private async Task IntentAdios(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Un gusto hablar contigo", cancellationToken: cancellationToken);
        }

        private async Task IntentAgradecer(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Fue un placer ayudarte", cancellationToken: cancellationToken);
        }

        private async Task IntentInformacion(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            string informacionBasica = $"Nuestros número de atención son los siguientes: {Environment.NewLine}" +
                $" +52 4271240434 o también +52 427 118 7155 "+
                $"Nuestros correo electrónico de atención es el siguiente: { Environment.NewLine}" +
                $" pethuellitas21@gmail.com";

            await stepContext.Context.SendActivityAsync(informacionBasica, cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync(activity: ShowContactos(), cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync("¿En que más te puedo ayudar?", cancellationToken: cancellationToken);

        }

        private IActivity ShowContactos()
        {
            var optionsAttachments = new List<Attachment>();

            var cardUbicacion = new HeroCard
            {
                Title = "Ubicación",
                Subtitle = "Visitanos en Santiago de Quéretaro ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver por Google Maps", Value="https://goo.gl/maps/aW5Vtts1wxDAq1S39", Type= ActionTypes.OpenUrl}
                }
            };

            var cardFormulario = new HeroCard
            {
                Title = "Formulario de contacto para quejas, dudas o reembolsos",
                Subtitle = " Nosotros nos pondremos en contacto contigo si llenas el siguiente formulario ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ir al formulario", Value="https://docs.google.com/forms/d/e/1FAIpQLSe6cRfI6TcZ2pzssMp_6QcE82jNVcfU7oAD1pe1aN_oDhL8iw/viewform?usp=sf_link", Type= ActionTypes.OpenUrl}
                }
            };

            optionsAttachments.Add(cardUbicacion.ToAttachment());
            optionsAttachments.Add(cardFormulario.ToAttachment());

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;


        }



        private async Task IntentOpciones(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Aquí tengo mis opciones", cancellationToken: cancellationToken);
            await MainOptionsCard.ToShow(stepContext, cancellationToken);

        }

        private async Task IntentSaludos(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Hola, para darte un mejor servicio puedes preguntarme que opciones tengo para ayudarte.", cancellationToken: cancellationToken);
            await MainOptionsCard.ToShow(stepContext, cancellationToken);
        }

        private async Task IntentProductos(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("Ofrecemos muchos productos, puedes darles un vistazo por categoria", cancellationToken: cancellationToken);
            await Task.Delay(1000);
            await stepContext.Context.SendActivityAsync(activity: ShowCategorias(), cancellationToken);
        }

        private IActivity ShowCategorias()
        {
            var optionsAttachments = new List<Attachment>();

            var cardJuguetes = new HeroCard
            {
                Title = "Juguetes",
                Text = "Encuentra los mejores juguetes para tu perro y gato ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="https://huellitaspet.weebly.com/store/c3/Juguetes_para_perros_y_gatos.html", Type= ActionTypes.OpenUrl}
                }
            };

            var cardCollares = new HeroCard
            {
                Title = "Collares",
                Text = "Encuentra los mejores collares para tu perro y gato ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="https://huellitaspet.weebly.com/store/c4/Collares.html", Type= ActionTypes.OpenUrl}
                }

            };

            var cardComederos = new HeroCard
            {
                Title = "Comederos",
                Text = " Conoce los productos que nos han dado popularidad, desde comederos simples hasta los inteligentes ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="https://huellitaspet.weebly.com/store/c5/Comederos_inteligentes_para_mascotas.html", Type= ActionTypes.OpenUrl}
                }

            };

            var cardLimpieza = new HeroCard
            {
                Title = "Limpieza",
                Text = " Ofrecemos una amplia gama de articulos de limpieza ",
                Buttons = new List<CardAction>()
                {
                    new CardAction(){Title="Ver", Value="https://huellitaspet.weebly.com/store/c6/Art%C3%ADculos_de_Limpieza_para_t%C3%BA_mascota.html", Type= ActionTypes.OpenUrl}
                }
            };

            optionsAttachments.Add(cardJuguetes.ToAttachment());
            optionsAttachments.Add(cardCollares.ToAttachment());
            optionsAttachments.Add(cardComederos.ToAttachment());
            optionsAttachments.Add(cardLimpieza.ToAttachment());

            var reply = MessageFactory.Attachment(optionsAttachments);
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            return reply as Activity;


        }

        private async Task IntentNone(WaterfallStepContext stepContext, RecognizerResult luisResult, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync("No entiendo lo que me dices", cancellationToken: cancellationToken);
        }

        private Task<DialogTurnResult> FinalProcess(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return stepContext.EndDialogAsync(cancellationToken: cancellationToken );
        }
    }
}
