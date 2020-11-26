using UnityEngine.Events;

namespace Reader
{
    public static class SttEventManager 
    {
        //Event Delegates
        public static UnityEvent RecorderStart = new UnityEvent();
        public static UnityEvent RecorderStop = new UnityEvent();
    
    }
}
