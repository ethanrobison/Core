using System;
using System.Reflection;
using UnityEngine;

namespace Utils
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ValidatedComponent : Attribute
    {
        public string TargetTransform { get; }

        public ValidatedComponent (string targetTransform = "") {
            TargetTransform = targetTransform;
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
                    if (attribute is ValidatedComponent vc) { ValidateField(vc.TargetTransform, field); }
                }
            }
        }

        private void ValidateField (string targetTransform, FieldInfo field) {
            var fieldtype = field.FieldType;
            if (!fieldtype.IsSubclassOf(typeof(Component))) {
                throw new CustomAttributeFormatException(
                    $"ValidatedComponent attr on non-Component field {field.Name}.");
            }

            var target = transform.Find(targetTransform);
            if (target == null) {
                throw new ArgumentException($"Passed name {targetTransform} is invalid.");
            }

            var comps = target.GetComponents(fieldtype);
            if (comps.Length == 0) {
                throw new MissingComponentException($"No components of type {fieldtype} found.");
            }

            if (comps.Length > 1) {
                throw new Exception($"Multiple components of type {fieldtype} found.");
            }

            var comp = comps[0];
            Logging.Assert(comp != null, "What the fork?");

            Logging.Log($"Setting {field.Name} to {comp}");
            field.SetValue(this, comp);
            Logging.Assert(field.GetValue(this) != null, "We just did this; what the heck?");
        }
    }
}