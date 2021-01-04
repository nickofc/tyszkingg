using System;

namespace TwReplay.Database
{
    public class DatabaseEngineTypeFactory
    {
        public DatabaseEngineType Get(string databaseEngineType)
        {
            return databaseEngineType.ToLower() switch
            {
                "sqlite" => DatabaseEngineType.Sqlite,
                "postgresql" => DatabaseEngineType.PostgreSql,
                _ => throw new Exception("Database type is not supported!")
            };
        }
    }
}