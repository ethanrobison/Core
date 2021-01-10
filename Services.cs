using System;

namespace Core
{

    public class Services
    {
        public UiService Ui { get; private set; }

        private bool _initialized;

        public Services() {
            Ui = new UiService();
        }

        public void Initialize() {
            if (_initialized) {
                Logging.Error("Double initializing services.");
                return;
            }

            Ui.Initialize();

            // TODO TEMP
            Ui.MakeUiElement(UiPrefabs.UiPrefabs.MainMenu);

            _initialized = true;
        }

        // TODO this happens when the player, e.g., returns to the main menu
        public void EndSession() {
            throw new NotImplementedException();
        }

        // TODO we try to do this when the player is quitting the game to desktop
        public void ShutDown() {
            throw new NotImplementedException();
        }
    }

    public interface IServiceManager
    {
        void Initialize();
        void Shutdown();
    }
}
