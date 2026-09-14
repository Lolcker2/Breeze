#pragma warning disable CS0649

namespace Breeze
{
    /*
        Vec2d class represents a 2d vector
        laid upon the 2d coordinate system (x and y)
    */
    public class Vec2d
    {
        public float x { get; set; }
        public float y { get; set; }

        public Vec2d() { }

        public Vec2d(float _x, float _y)
        {
            this.x = _x;
            this.y = _y;
        }

        public override string ToString() => $"({this.x}, {this.y})";
    }

    class Transform : Component
    {
        public GameObject? self;

        public float rotation;
        public Vec2d position;
        public Vec2d scale;

        public Transform()
        {
            position = new Vec2d(0f, 0f);
            scale = new Vec2d(1f, 1f);
            rotation = 1f;
        }

        public void ApplyTransform()
        {
            // need to do parent.apply()
            return;
        }

        public override void Start()
        {
            Console.WriteLine("Transform component has started");
        }
    }
}
