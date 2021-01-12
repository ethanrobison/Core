using UnityEngine;

namespace Core
{
    public abstract class BaseDialog : IUiDialog
    {
        protected GameObject _go;
        private bool _initialized;

        public abstract bool IsShowing { get; }
        public abstract bool ShowAtStartUp { get; }
        public abstract UiReference Ui { get; }

        virtual protected void OnBeforeShow() {
            if (!_initialized) {
                Initialize();
            }
        }

        virtual protected void OnAfterHide() { }

        virtual protected void RefreshContents() { }

        protected void Initialize() {
            _go = Game.Serv.Ui.GetUi(Ui);
            _initialized = true;
        }

        public void RequestHide() {
            _go.SetActive(false);
            OnAfterHide();
        }

        public void RequestShow() {
            OnBeforeShow();
            _go.SetActive(true);

            RefreshContents();
        }

        public abstract void Close();
    }
}
