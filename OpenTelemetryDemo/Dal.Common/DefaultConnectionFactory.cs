using System.Data.Common;
using System.Threading.Tasks;

namespace Dal.Common;

public class DefaultConnectionFactory : IConnectionFactory {
  private readonly DbProviderFactory dbProviderFactory;

  public static IConnectionFactory FromConfiguration(string connectionStringConfigName) {
    (string connectionString, string providerName) =
      ConfigurationUtil.GetConnectionParameters(connectionStringConfigName);
    return new DefaultConnectionFactory(connectionString, providerName);
  }

  public DefaultConnectionFactory(string connectionString, string providerName) {
    this.ConnectionString = connectionString;
    this.ProviderName = providerName;

    DbUtil.RegisterAdoProviders();
    this.dbProviderFactory = DbProviderFactories.GetFactory(providerName);
  }

  public string ConnectionString { get; }

  public string ProviderName { get; }

  public async Task<DbConnection> CreateConnectionAsync() {
    var connection = dbProviderFactory.CreateConnection();
    if (connection is null)
      throw new InvalidOperationException("DbProviderFactory.CreateConnection() returned null.");

    connection.ConnectionString = this.ConnectionString;
    await connection.OpenAsync();

    return connection;
  }
}
