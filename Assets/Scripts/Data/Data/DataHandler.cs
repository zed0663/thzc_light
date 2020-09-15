using System;
using System.Reflection;

public class DataHandler<T> where T :class
{
    private static object lockObj = new object();
    private static T mySelf;//= default(T);
    public static T Instance
    {
        get
        {
            lock (lockObj)
            {
                if (mySelf == null)
                {
                    mySelf = InstanceCreater.CreateInstance<T>();
                }
            }

            return mySelf;
        }
    }
#if MOCK
        public static void InstanceClear()
        {
           mySelf=null;
        } 
#endif
}

static class InstanceCreater
{
    public static T CreateInstance<T>()
    {
        var type = typeof(T);
        try
        {
            return (T)type.Assembly.CreateInstance(type.FullName, true, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null, null);
        }
        catch (MissingMethodException ex)
        {
            throw new System.Exception(string.Format("{0}(单例模式下，构造函数必须为private)", ex.Message));
        }
    }
}
