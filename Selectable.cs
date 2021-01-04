using UnityEngine;

namespace Core
{
    public abstract class Selectable : MonoBehaviour
    {
        public abstract void InformOfSelection();
        public abstract void InformOfDeselection();

        private void OnMouseDown() {
            Game.Ctx.Selection.ToggleSelected(this);
        }
    }
}
