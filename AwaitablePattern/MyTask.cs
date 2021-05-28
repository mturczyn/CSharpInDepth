namespace FirstEncounterWithAsync.AwaitablePattern
{
    class MyTask<T>
    {
        public MyTaskCompletion<T> GetAwaiter()
        {
            return new MyTaskCompletion<T>();
        }
    }
}
