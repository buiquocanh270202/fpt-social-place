namespace API.Hub.SubscribeSqlTableDependencies
{
    public interface ISubscribeSqlTableDependency
    {
        void SubscribeTableDependency(string connectionString);
        void SubscribeBlockUserProfileTableDependency(string connectionString);
    }
}
