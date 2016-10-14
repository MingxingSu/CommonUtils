namespace MaxSu.Framework.Common
{
    public sealed class Singleton
    {
        //Until Singleton class visited at first time(static constructor called), _instance will be inited.
        private static readonly Singleton _instance = new Singleton(); //Readonly can gurante mutilthread safty.

        static Singleton()
        {
            Init();
        }

        private Singleton()
        {
        }

        public static Singleton Instance
        {
            get { return _instance; }
        }

        private static void Init()
        {
            //Load Settings
        }
    }
}