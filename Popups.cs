namespace Core
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
        public void Activate() {
            RefreshContents();
        }

        public void Deactivate() {
            throw new System.NotImplementedException();
        }

        public void OnPopped() {
            RequestHide();
        }

        public void OnPushed() {
            RequestShow();
        }
    }
}
