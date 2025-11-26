using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderingSystem.Infrastructure.Extensions
{
    using Microsoft.Data.SqlClient;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;

    namespace OrderingSystem.Infrastructure.Extensions
    {
        public static class SqlDataReaderExtensions
        {
  
            public static T? GetValue<T>(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? default : (T)reader.GetValue(ordinal);
            }

            public static T? GetValue<T>(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? default : (T)reader.GetValue(ordinal);
            }
 
            public static string? GetStringSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
            }

            public static string? GetStringSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? "" : reader.GetString(ordinal);
            }

 
            public static int GetIntSafe(this SqlDataReader reader, string columnName)
            {
                var index = reader.GetOrdinal(columnName);
                return reader.IsDBNull(index) ? 0 : reader.GetInt32(index);
            }

            public static int GetIntSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
            }

            public static int? GetNullableIntSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
            }

            public static int? GetNullableIntSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? null : reader.GetInt32(ordinal);
            }
 
            public static decimal GetDecimalSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? 0m : reader.GetDecimal(ordinal);
            }

            public static decimal GetDecimalSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? 0m : reader.GetDecimal(ordinal);
            }

            public static decimal? GetNullableDecimalSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
            }

            public static decimal? GetNullableDecimalSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? null : reader.GetDecimal(ordinal);
            }
 
            public static bool GetBoolSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
            }

            public static bool GetBoolSafe(this SqlDataReader reader, int ordinal)
            {
                return !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
            }

            public static bool? GetNullableBoolSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
            }

            public static bool? GetNullableBoolSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);
            }

             public static DateTime GetDateTimeSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
            }

            public static DateTime GetDateTimeSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
            }

            public static DateTime? GetNullableDateTimeSafe(this SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
            }

            public static DateTime? GetNullableDateTimeSafe(this SqlDataReader reader, int ordinal)
            {
                return reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
            }
        }
    }

}
