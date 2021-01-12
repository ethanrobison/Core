using System;
using Ui;

namespace Core
{

    public enum ServiceState
    {
        Invalid = -1,
        Loading,
        PostLoading,
        Initialized,
    }
    public class Services
    {
        public UiService Ui { get; private set; }

        private ServiceState _state = ServiceState.Invalid;

        public Services() {
            Ui = new UiService();
        }

        public void Initialize() {
            if (_state >= ServiceState.Loading) {
                Logging.Error("Double initializing services.");
                return;
            }

            Ui.Initialize();

            _state = ServiceState.Loading;
        }

        public void Update() {
            switch (_state) {
                case ServiceState.Loading:
                    TryFinishLoading();
                    break;
                case ServiceState.Initialized:
                    break;
            }
        }

        // TODO this happens when the player, e.g., returns to the main menu
        public void EndSession() {
            throw new NotImplementedException();
        }

        // TODO we try to do this when the player is quitting the game to desktop
        public void ShutDown() {
            throw new NotImplementedException();
        }

        private void TryFinishLoading() {
            if (Ui.IsFinishedLoading()) {
                _state = ServiceState.PostLoading;
                OnFinishedLoading();
            }
        }

        /// <summary>
        /// Called exactly once, after every service has finished loading.
        /// </summary>
        private void OnFinishedLoading() {
            Ui.ShowUiElementTemp(new MainMenuDialog());
            _state = ServiceState.Initialized;
        }
    }

    public interface IServiceManager
    {
        void Initialize();
        void Shutdown();
        bool IsFinishedLoading();
    }
}