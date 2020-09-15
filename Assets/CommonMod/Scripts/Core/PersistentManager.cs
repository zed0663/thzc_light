using System;


namespace Monster.Core
{
    public class PersistentManager : Attribute
    {

        public bool LoadAtRuntime { get; private set; }

        public static bool SceneWasLoaded;

        public PersistentManager(bool loadAtRuntime)
        {
            this.LoadAtRuntime = loadAtRuntime;
        }

        public PersistentManager() : this(false)
        {
        }

        public static bool HasAttribute(Type type)
        {
            return Attribute.GetCustomAttribute(type, typeof(PersistentManager)) != null;
        }

        public static bool IsLoadAtRuntime(Type type)
        {
            if (!PersistentManager.HasAttribute(type))
            {
                return false;
            }

            PersistentManager persistentManager =
                (PersistentManager) Attribute.GetCustomAttribute(type, typeof(PersistentManager));
            return persistentManager.LoadAtRuntime;
        }

    }
}