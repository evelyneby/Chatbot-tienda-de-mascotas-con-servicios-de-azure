using Microsoft.Bot.Builder.AI.Luis;

namespace HuellitasCompleto.Infrestructure.LUIS
{
    public interface ILuisService
    {
        public LuisRecognizer _luisRecognizer { get; set; }
    }
}
