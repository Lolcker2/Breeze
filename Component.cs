using System.Reflection;

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Breeze
{
    public class Component
    {
        public string? name;

        public SparseRef? selfRef;

        public Component() { }
        public Component(string _name)
        {
            name = _name;
        }

        public void Fetch()
        {
            foreach (FieldInfo _field in this.GetType().GetFields())
            {
                if (_field.FieldType == typeof(GameObject))
                {
                    GameObject _fieldObject = (GameObject)_field.GetValue(this);
                    _field.SetValue(this, World.Entities.GetGameObject(_fieldObject.selfRef));
                    return;
                }
                if (_field.FieldType.IsSubclassOf(typeof(Component)))
                {
                    Component _fieldObject = (Component)_field.GetValue(this);
                    _field.SetValue(this, World.Entities.GetComponent(_fieldObject.selfRef));
                    return;
                }
            }
        }

        public virtual void Start() { }

        public virtual void Update() { }

        public virtual void Render() { } //fixed update
    }
}
