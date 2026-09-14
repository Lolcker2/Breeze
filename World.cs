using System.Reflection;
using Breeze;


namespace Breeze
{

    public static class World
    {
        public static readonly EntityManager Entities = new EntityManager();
    }

    /*
        make the name resolver
        passing components would look like "World.player.Transform", where player is a name
        and World is a prefix (other prefixes include Prefub)
        name resolutions with callbacks (ignore prefubs for now)
    */
}
