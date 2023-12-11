namespace App
{
    public class DefaultLevelManager : ILevelManager
    {
        public IConfigManager ConfigManager { get; set; }
        public IEntityManager EntityManager { get; set; }
        public IPoolManager PoolManager { get; set; }
    }
}