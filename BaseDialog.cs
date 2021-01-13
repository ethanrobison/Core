using UnityEngine;

namespace Core
{
    public abstract class BaseDialog : IUiDialog
    {
        protected GameObject _go;

        public bool IsShowing => _go.activeSelf;
        public abstract UiReference Ui { get; }

        protected BaseDialog() => Initialize();

        virtual protected void OnBeforeShow() { }

        virtual protected void OnAfterHide() { }

        virtual protected void RefreshContents() { }

        protected void Initialize() => _go = Game.Serv.Ui.GetUi(Ui);

        public void RequestHide() {
            _go.SetActive(false);
            OnAfterHide();
        }

        public void RequestShow() {
            OnBeforeShow();
            _go.SetActive(true);

            RefreshContents();
        }
    }
}
