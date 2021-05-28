using System;
using System.Runtime.CompilerServices;

namespace FirstEncounterWithAsync.AwaitablePattern
{
    /// <summary>
    /// Awaiter type. 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    class MyTaskCompletion<T> : INotifyCompletion
    {
        public bool IsCompleted { get; set; }
        public void OnCompleted(Action continuation)
        {
            IsCompleted = true;
            continuation?.Invoke();
        }
        public T GetResult()
        {
            return default(T);
        }
    }
}
