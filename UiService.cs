using System.Text;
using UnityEngine;

namespace Core
{
    public class UiService : IServiceManager
    {
        private Canvas _canvas;
        private UiPrefabFactory _factory;

        public UiService() {
            _canvas = UiUtils.GetCanvas();
            _factory = new PrefabFactory();
        }

        public void Initialize() {
            // TODO figure out how to get a factory specific to the game...
        }

        public void Shutdown() {
            throw new System.NotImplementedException();
        }

        public GameObject MakeUiElement(UiPrefabs.UiPrefabs prefab) => _factory.MakeUiElement(prefab, _canvas);
    }

    public abstract class UiPrefabFactory
    {
        public GameObject MakeUiElement(UiPrefabs.UiPrefabs prefab, Canvas canvas) => MakeUiElement(prefab, canvas.transform);
        public abstract GameObject MakeUiElement(UiPrefabs.UiPrefabs prefab, Transform holder = null);

        private static readonly StringBuilder _sb = new StringBuilder();
        protected string PrefabPathFromEnum(UiPrefabs.UiPrefabs prefabName) {
            var name = prefabName.ToString();
            var length = name.Length;

            _sb.Append("Ui/");

            _sb.Append(name[0]);
            for (int i = 1; i < length; i++) {
                char c = name[i];
                if (char.IsUpper(c)) {
                    _sb.Append(" ");
                }

                // TODO possibly include parsing rules for inserting /'s

                _sb.Append(c);
            }

            return _sb.ToStringAndClear();
        }
    }
}


