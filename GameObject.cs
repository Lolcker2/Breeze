#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8604 // Possible null reference argument.

namespace Breeze
{
    public class GameObject : Component
    {
        public List<SparseRef> components; // sparseref list instead of components

        public SparseRef? parent;
        public List<SparseRef> children;

        public GameObject()
            : base()
        {
            components = new List<SparseRef>();
            children = new List<SparseRef>();
        }

        // full consturctor
        public GameObject(string _name, List<SparseRef> _comps, List<SparseRef> _childs)
            : base(_name)
        {
            components = _comps;
            children = _childs;
        }

        public GameObject(
            string _name,
            List<SparseRef> _comps,
            List<SparseRef> _childs,
            SparseRef _parent
        )
            : base(_name)
        {
            components = _comps;
            children = _childs;
            parent = _parent;
        }

        public bool IsChild()
        {
            return parent != null;
        }

        public static GameObject ToGameObject(Entity _entity)
        {
            if (_entity.parent == null)
            {
                return new GameObject(_entity.name, _entity.components, _entity.children);
            }
            return new GameObject(
                _entity.name,
                _entity.components,
                _entity.children,
                _entity.parent
            );
        }

        public Entity ToEntity()
        {
            if (IsChild())
            {
                return new Entity(name, components, children);
            }
            return new Entity(name, components, children, parent);
        }

        /*
            given a component type as the generic 'T', returns the component of said type
            of this GameObject
        */

        public T? GetComponent<T>()
        {
            foreach (Component _comp in components.Select(World.Entities.GetComponent).ToList())
            {
                if (_comp.GetType() == typeof(T))
                {
                    return (T)(object)_comp;
                }
            }
            Console.WriteLine($"GameObject {name} does not have any {typeof(T)} component.");
            return default;
        }

        public override void Start()
        {
            foreach (Component _comp in components.Select(World.Entities.GetComponent).ToList())
            {
                _comp.Start();
            }
        }

        public override void Render()
        {
            foreach (Component _comp in components.Select(World.Entities.GetComponent).ToList())
            {
                _comp.Fetch(); // for each component, update its anchored fields
                _comp.Render();
            }
        }

        public override void Update()
        {
            foreach (Component _comp in components.Select(World.Entities.GetComponent).ToList())
            {
                _comp.Update();
            }
        }
    }
}
