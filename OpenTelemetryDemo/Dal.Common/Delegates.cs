using System.Data;

namespace Dal.Common;

public delegate T RowMapper<T>(IDataRecord row);

public delegate T RowMapperMultiple<T>(T? domain,IDataRecord row);