namespace SimpleCreation.Models
{
    public class TableSchema : Condition
    {
        public string TABLE_NAME {  get; set; }
        public List<Column> Columns { get; set; }
    }
    public class Column 
    {
        public string COLUMN_NAME  { get; set; }
        public string IS_NULLABLE { get; set; }
        public string DATA_TYPE { get; set; }
        public int? CHARACTER_MAXIMUM_LENGTH { get; set; }
    }

    
}
