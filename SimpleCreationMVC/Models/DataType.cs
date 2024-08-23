namespace SimpleCreation.Models
{
    public class DataType
    {
        public string SqlDataType { get; set; } = "";
        public string DotNetDataType { get; set; } = "";
        public string TypescriptDataType { get; set; } = "";
        public List<DataType> List()
        {

            return new List<DataType>
            {
                new DataType { SqlDataType = "bit", DotNetDataType = "bool",TypescriptDataType="boolean" },
                new DataType { SqlDataType = "char", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "date", DotNetDataType = "DateTime" ,TypescriptDataType="Date"},
                new DataType { SqlDataType = "datetime", DotNetDataType = "DateTime",TypescriptDataType="Date" },
                new DataType { SqlDataType = "datetime2", DotNetDataType = "DateTime",TypescriptDataType="Date" },
                new DataType { SqlDataType = "datetimeoffset", DotNetDataType = "DateTimeOffset" },
                new DataType { SqlDataType = "decimal", DotNetDataType = "decimal",TypescriptDataType="number" },
                new DataType { SqlDataType = "float", DotNetDataType = "double" ,TypescriptDataType="number"},
                new DataType { SqlDataType = "image", DotNetDataType = "byte[]" },
                new DataType { SqlDataType = "int", DotNetDataType = "int",TypescriptDataType="number" },
                new DataType { SqlDataType = "money", DotNetDataType = "decimal",TypescriptDataType="number" },
                new DataType { SqlDataType = "nchar", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "ntext", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "numeric", DotNetDataType = "decimal",TypescriptDataType="number" },
                new DataType { SqlDataType = "nvarchar", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "smalldatetime", DotNetDataType = "DateTime",TypescriptDataType="Date" },
                new DataType { SqlDataType = "smallint", DotNetDataType = "short",TypescriptDataType="number" },
                new DataType { SqlDataType = "sql_variant", DotNetDataType = "object" },
                new DataType { SqlDataType = "text", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "time", DotNetDataType = "TimeSpan",TypescriptDataType="Date" },
                new DataType { SqlDataType = "tinyint", DotNetDataType = "byte",TypescriptDataType="number" },
                new DataType { SqlDataType = "uniqueidentifier", DotNetDataType = "Guid",TypescriptDataType="string" },
                new DataType { SqlDataType = "varchar", DotNetDataType = "string",TypescriptDataType="string" },
                new DataType { SqlDataType = "xml", DotNetDataType = "string",TypescriptDataType="string" }
            };
        }
        public string ConvertToDotNet(string sqlDataType)
        {
            return List().Where(_ => _.SqlDataType == sqlDataType).FirstOrDefault().DotNetDataType;
        }
        public string ConvertToTypeScript(string sqlDataType)
        {
            return List().Where(_ => _.SqlDataType == sqlDataType).FirstOrDefault().TypescriptDataType;
        }
    }
}
