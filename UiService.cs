using System;
using System.Collections.Generic;
//using UiPrefabs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Core.Ui
{
    public enum UiScene
    {
        Invalid = -1,
        SessionUi = 0,
        Count = 1,
    }

    public class UiService : IServiceManager
    {
        private Dictionary<UiScene, Dictionary<UiPrefab, GameObject>> _uiReferences = new Dictionary<UiScene, Dictionary<UiPrefab, GameObject>>();
        private Popups _popups = new Popups();
        private List<IUiDialog> _dialogs = new List<IUiDialog>();

        public void Initialize() {
            SceneManager.sceneLoaded += OnMainSceneLoaded;
        }

        public void Shutdown() {
            throw new System.NotImplementedException();
        }

        public bool IsFinishedLoading() {
            for (UiScene scene = UiScene.SessionUi; scene < UiScene.Count; scene++) {
                if (!_uiReferences.ContainsKey(scene)) {
                    return false;
                }
            }

            return true;
        }

        private void StartLoadingScene(UiScene scene) => SceneManager.LoadSceneAsync(scene.ToString(), LoadSceneMode.Additive);

        private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode) {
            Logging.Assert(scene.name == "Game", "Called OnMainSceneLoaded on wrong scene", scene.name);
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
            SceneManager.sceneLoaded += OnUiSceneLoaded;

            StartLoadingScene(UiScene.SessionUi);
        }

        private void OnUiSceneLoaded(Scene scene, LoadSceneMode mode) {
            var success = Enum.TryParse(scene.name, out UiScene uiscene);
            if (!success) {
                Logging.Error("Invalid scene", scene.name);
                return;
            }

            Logging.Log("Loaded UI scene", uiscene);
            _uiReferences[uiscene] = new Dictionary<UiPrefab, GameObject>();

            var roots = scene.GetRootGameObjects();
            var count = roots.Length;
            if (count < 1) {
                Logging.Error("Loaded empty scene", uiscene);
                return;
            }

            var first = roots[0];
            var canvas = first.GetComponent<Canvas>();
            if (canvas == null) {
                Logging.Error("Malformed ui scene: first object is not canvas");
                return;
            }

            var childcount = first.transform.childCount;
            for (int i = 0; i < childcount; i++) {
                // TODO
                //var child = first.transform.GetChild(i);
                //var prefab = UiElements.EnumFromPrefabPath(child.name);
                //if (prefab == UiPrefab.Invalid) { continue; }
                //_uiReferences[uiscene][prefab] = child.gameObject;
            }
        }

        public GameObject GetUi(UiReference ui) {
            if (!_uiReferences.ContainsKey(ui.Scene)) {
                Logging.Error("Missing scene for ui", ui.Scene);
                return null;
            }

            var sceneGos = _uiReferences[ui.Scene];
            if (!sceneGos.ContainsKey(ui.Prefab)) {
                Logging.Error("Scene", ui.Scene, "does not contain prefab", ui.Prefab);
                return null;
            }

            var go = sceneGos[ui.Prefab];
            Logging.Assert(go.name == ui.Name, "Malformed UiReference name,", go.name, "!=", ui.Name);
            return go;
        }

        public void ShowDialog(IUiDialog dialog) {
            if (dialog.IsShowing) {
                Logging.Warn("Attempting to double show dialog", dialog);
                return;
            }

            dialog.RequestShow();
            _dialogs.Add(dialog);
        }

        public void HideDialog(IUiDialog dialog) {
            if (!dialog.IsShowing) {
                Logging.Warn("Attempting to double hide dialog", dialog);
                return;
            }

            dialog.RequestHide();
            _dialogs.Remove(dialog);
        }

        public void AddPopup<T>() where T : PopupDialog, new() => AddPopup(new T());

        /// <summary>
        /// Show designated popup.
        /// </summary>
        public void AddPopup(PopupDialog popup) {
            Logging.Assert(popup != null, "Passing invalid popup to show");
            _popups.ShowPopup(popup);
        }

        /// <summary>
        /// Hides the top popup.
        /// </summary>
        public void RemovePopup(PopupDialog popup) {
            Logging.Assert(popup != null, "Passing invalid popup to hide");
            Logging.Assert(_popups.GetTopPopup() == popup, "Hiding non-top popup");
            _popups.HidePopup(popup);
        }
    }

    public struct UiReference
    {
        public readonly string Name;
        public readonly UiScene Scene;
        public readonly UiPrefab Prefab;
        public UiReference(UiScene scene, UiPrefab prefab) {
            Scene = scene;
            Prefab = prefab;
            Name = UiElements.PrefabPathFromEnum(prefab);
        }
    }

    public interface IUiDialog
    {
        bool IsShowing { get; }
        void RequestShow();
        void RequestHide();
    }

    public interface IUiUpdatingDialog : IUiDialog
    {
        void OnFrameUpdate();
    }


    // TODO
    public class UiElements
    {
        public static string PrefabPathFromEnum(UiPrefab prefab) { return ""; }

    }
    public class UiPrefab { }
}
