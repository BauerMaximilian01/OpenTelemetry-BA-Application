using System.Data.Common;

namespace Dal.Common;

public class AdoTemplate {
  private readonly IConnectionFactory connectionFactory;

  public AdoTemplate(IConnectionFactory connectionFactory) => this.connectionFactory = connectionFactory;

  // GET multiple rows
  public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters) {
    await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
    await using DbCommand command = connection.CreateCommand();

    command.CommandText = sql;

    AddParameters(command, parameters);

    var items = new List<T>();
    await using DbDataReader reader = await command.ExecuteReaderAsync();
    while (reader.Read()) {
      items.Add(rowMapper(reader));
    }

    return items;
  }

  public async Task<IEnumerable<T>> QueryAsync<T>(string sql, RowMapperMultiple<T> rowMapper, params QueryParameter[] parameters) {
    await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
    await using DbCommand command = connection.CreateCommand();

    command.CommandText = sql;

    AddParameters(command, parameters);

    var items = new List<T>();
    T? domain = default;
    T? lastDomain = default;
    int count = 0;
    await using DbDataReader reader = await command.ExecuteReaderAsync();
    while (reader.Read()) {
      domain = rowMapper(domain, reader);

      if (count == 0) {
        lastDomain = domain;
        count++;
      }

      if (!domain.Equals(lastDomain)) {
        items.Add(lastDomain);
      }
      
      lastDomain = domain;
    }

    if (domain != null) {
      items.Add(domain);
    }

    return items;
  }

  // GET one row
  public async Task<T?> QuerySingleAsync<T>(string sql, RowMapper<T> rowMapper, params QueryParameter[] parameters) {
    return (await QueryAsync(sql, rowMapper, parameters)).SingleOrDefault();
  }

  public async Task<T?> QuerySingleAsync<T>(string sql, RowMapperMultiple<T> rowMapper, params QueryParameter[] parameters) {
    await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
    await using DbCommand command = connection.CreateCommand();

    command.CommandText = sql;

    AddParameters(command, parameters);

    T? domain = default;
    await using DbDataReader reader = await command.ExecuteReaderAsync();
    while (reader.Read()) {
      domain = rowMapper(domain, reader);
    }

    return domain;
  }

  // UPDATE DELETE
  public async Task<int> ExecuteAsync(string sql, params QueryParameter[] parameters) {
    await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
    await using DbCommand command = connection.CreateCommand();

    command.CommandText = sql;

    AddParameters(command, parameters);

    return await command.ExecuteNonQueryAsync();
  }

  // INSERT
  public async Task<R?> ExecuteScalarAsync<R>(string sql, params QueryParameter[] parameters) {
    await using DbConnection connection = await connectionFactory.CreateConnectionAsync();
    await using DbCommand command = connection.CreateCommand();

    command.CommandText = sql;

    AddParameters(command, parameters);

    return (R?)await command.ExecuteScalarAsync();
  }


  private void AddParameters(DbCommand command, QueryParameter[] parameters) {
    foreach (var p in parameters) {
      DbParameter dbParam = command.CreateParameter();
      dbParam.ParameterName = p.Name;
      dbParam.Value = p.Value;
      command.Parameters.Add(dbParam);
    }
  }
}