using UnityEngine;

namespace Core
{
    public class UiService : IServiceManager
    {
        private Canvas _canvas;
        private UiPrefabFactory _factory;

        public UiService() {
            _canvas = UiUtils.GetCanvas();
            _factory = new UiPrefabFactory();
        }

        public void Initialize() {
            // TODO figure out how to get a factory specific to the game...
        }

        public void Shutdown() {
            throw new System.NotImplementedException();
        }

        public GameObject MakeUiElement(UiPrefabs.UiPrefabs prefab) => _factory.MakeUiElement(prefab, _canvas);
    }
}


