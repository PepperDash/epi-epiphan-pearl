namespace PepperDash.Essentials.Plugins.Interfaces
{
    interface IEpiphanPearlClient
    {
        string Delete(string path);
        T Get<T>(string path) where T:class ;
        TResponse Post<TBody, TResponse>(string path, TBody body)
            where TBody : class
            where TResponse : class;

        TResponse Post<TResponse>(string path) where TResponse : class;

        /// <summary>Point at a different Pearl, keeping the scheme and credentials.</summary>
        void SetHost(string host);
    }
}
