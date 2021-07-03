using UnityEngine.UI;

namespace Core.Ui
{
    public class Popups
    {
        private SmartStack<PopupDialog> _popups = new SmartStack<PopupDialog>();

        public void ShowPopup(PopupDialog popup) {
            _popups.Push(popup);
        }

        public void HidePopup(PopupDialog popup) {
            if (_popups.Peek != popup) {
                Logging.Warn("Popping non-top popup", popup);
                return;
            }

            _popups.Pop();
        }

        public PopupDialog GetTopPopup() {
            return _popups.Peek;
        }
    }

    public abstract class PopupDialog : BaseDialog, ISmartStackElement
    {
        protected const string CONTAINER = "Container/", CLOSE = CONTAINER + "Close/";
        protected Button _close;

        protected override void OnBeforeShow() {
            base.OnBeforeShow();
            _close = UiUtils.GetAndSetButton(_go, CLOSE, Close);
        }

        protected override void OnAfterHide() {
            base.OnAfterHide();
        }

        public void Activate() {
            RefreshContents();
        }

        public void Deactivate() {
            // TODO?
        }

        public void OnPopped() {
            RequestHide();
        }

        public void OnPushed() {
            RequestShow();
        }

        public void Close() => Game.Serv.Ui.RemovePopup(this);
    }
}
