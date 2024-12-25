namespace Discus.SDK.Repository.SqlSugar.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfigurationSection mysqlSection)
        {
            if (services.HasRegistered(nameof(AddSqlSugar)))
                return services;

            services.Configure<MysqlConfig>(mysqlSection).AddSqlSugar(mysqlSection);
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            var mysqlConfig = mysqlSection.Get<MysqlConfig>();
            services.AddSingleton<ISqlSugarClient>(provider =>
            {

                var db = new SqlSugarClient(new ConnectionConfig
                {
                    ConnectionString = mysqlConfig.MasterConnectionString,
                    DbType = DbType.MySql, // 根据数据库类型选择对应的DbType
                    IsAutoCloseConnection = true, // 自动关闭数据库连接
                    InitKeyType = InitKeyType.Attribute, // 使用属性方式进行表和列的映射
                    // 配置主从数据库连接
                    SlaveConnectionConfigs = mysqlConfig.SlaveConnectionStrings?.Select(s => new SlaveConnectionConfig
                    {
                        ConnectionString = s,
                        HitRate = 10 // 可以根据需要调整命中率
                    }).ToList(),
                });

#if DEBUG
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    Console.WriteLine($"SQL: {sql}");
                    Console.WriteLine($"Connection: {db.Ado.Connection.ConnectionString}");
                };
#endif
                return db;
            });
            return services;
        }
    }
}
