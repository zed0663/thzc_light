using System;


namespace Monster.Events
{
    public static class GameEvents
    {
        private static EventDispatcher _dispatcher = new EventDispatcher();

        public static void Subscribe<T>(Action<T> callback) where T : class
        {
            GameEvents._dispatcher.Subscribe<T>(callback);
        }


        public static void Unsubscribe<T>(Action<T> callback) where T : class
        {
            GameEvents._dispatcher.Unsubscribe<T>(callback);
        }


        public static void Invoke<T>(T evt) where T : class
        {
            GameEvents._dispatcher.Invoke<T>(evt);
        }


    }
}
