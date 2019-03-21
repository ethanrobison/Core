using System;
using System.Reflection;
using UnityEngine;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidatedComponent : Attribute
    {
        public string Name { get; }

        public ValidatedComponent (string name = "") {
            Name = name;
        }
    }

    public abstract class ValidatedMonoBehaviour : MonoBehaviour
    {
        protected virtual void Awake () {
            Validate();
        }

        private void Validate () {
            var type = GetType();
            var fields = type.GetRuntimeFields();

            foreach (var field in fields) {
                var attributes = field.GetCustomAttributes(true);

                foreach (var attribute in attributes) {
                    if (attribute is ValidatedComponent vc) { ValidateField(vc.Name, field); }
                }
            }
        }

        private void ValidateField (string targetTransform, FieldInfo field) {
            var fieldtype = field.FieldType;
            Logging.Log($"Validated compoent: {field.Name}");
            if (!fieldtype.IsSubclassOf(typeof(Component))) {
                Logging.Error($"Validated component put on something other than a component: {field.Name}");
                return;
            }

            var target = targetTransform == "" ? transform : transform.Find(targetTransform);
            if (target == null) {
                throw new ArgumentException($"Passed name {targetTransform} is invalid.");
            }

            var comps = target.GetComponents(fieldtype);
            if (comps.Length == 0) {
                throw new Exception($"No components of type {fieldtype} to be found.");
            }

            if (comps.Length > 1) {
                throw new Exception($"More than one component of type {fieldtype} found.");
            }

            var comp = comps[0];
            Logging.Assert(comp != null, "What the fork?");

            Logging.Log($"Setting {field.Name} to {comp}");
            field.SetValue(this, comp);
            Logging.Assert(field.GetValue(this) != null, "We just did this; what the heck?");
        }
    }
}