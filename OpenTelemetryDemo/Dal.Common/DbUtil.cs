using System.Data.Common;

namespace Dal.Common;

public static class DbUtil {
  public static void RegisterAdoProviders() {
    // Use new Implementation of MS SQL Provider
    DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
    // DbProviderFactories.RegisterFactory("System.Data.SqlClient", System.Data.SqlClient.SqlClientFactory.Instance);
    DbProviderFactories.RegisterFactory("Postgres.Data.PostgresClient",  Npgsql.NpgsqlFactory.Instance);
  }
}
