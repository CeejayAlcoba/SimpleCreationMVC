
using System.Data;
using System.Reflection;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class DataTableUtility : IDataTableUtility
    {
        public DataTable Convert<T>(IEnumerable<T>? lists) where T : class
        {
            DataTable dt = new DataTable(typeof(T).Name);
            PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add columns (even if list is null or empty)
            foreach (PropertyInfo property in properties)
            {
                Type columnType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;
                dt.Columns.Add(property.Name, columnType);
            }

            // Only add rows if the list is not null or empty
            if (lists != null)
            {
                foreach (T item in lists)
                {
                    DataRow row = dt.NewRow();
                    foreach (PropertyInfo property in properties)
                    {
                        row[property.Name] = property.GetValue(item) ?? DBNull.Value;
                    }
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }
    }
}

