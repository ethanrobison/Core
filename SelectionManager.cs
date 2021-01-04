using UnityEngine;

namespace Core
{
    public class SelectionManager : ICtxManager, IUpdateableManager
    {
        private Selectable _selected;

        public void Initialize() {
        }

        public void Shutdown() {
            throw new System.NotImplementedException();
        }

        public void SetSelected(Selectable target) {
            if (_selected == target) { return; }
            ToggleSelected(target);
        }

        public void DeselectAll() {
            if (_selected == null) { return; }
            ToggleSelected(_selected);
        }

        public void ToggleSelected(Selectable target) {
            Logging.Log("Selecting", target);

            if (_selected != null) {
                _selected.InformOfDeselection();
            }

            if (_selected == target) {
                _selected = null;
            } else {
                _selected = target;
                _selected.InformOfSelection();
            }
        }


        public void Update(float deltaTime) => CheckForClick();

        private static readonly RaycastHit2D[] _results = new RaycastHit2D[16];
        private void CheckForClick() {
            if (!Input.GetMouseButtonDown(0)) {
                return;
            }

            var screenpos = Input.mousePosition;
            Ray ray = Game.Ctx.Camera.Cam.ScreenPointToRay(screenpos);

            var hitcount = Physics2D.GetRayIntersectionNonAlloc(ray, _results, float.PositiveInfinity);
            if (hitcount > 0) {
                Logging.Log("Found hits", hitcount);
                ProcessClick(_results[0]);
            } else {
                Logging.Log("No hits found, deselecting.");
                DeselectAll();
            }
        }

        private void ProcessClick(RaycastHit2D hit) {
            var go = hit.collider.gameObject;
            var selectable = go.GetComponent<Selectable>();
            if (selectable == null) { return; }
            // TODO maybe iterate through them all and find one with a selectable?

            SetSelected(selectable);
        }
    }
}
