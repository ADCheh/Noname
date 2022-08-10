using Services.Input;

namespace Infrastructure
{
    public class Game
    {
        public static IInputService InputService;

        public Game()
        {
            RegisterInputService();
        }

        private static void RegisterInputService()
        {
            //With more input service types add here
            InputService = new StandaloneInputService();
        }
    }
}