
namespace MDA.Dbo
{
    public class Model
    {   
        public List<Table> Tables { get; set; }

        public class Table
        {    
            public string Name { get; set; }

            public List<Column> Columns { get; set; }
        }
       
        public class Column
        {         
            public int Cid { get; set; }

            public string Name { get; set; }

            public ColumnDataType Type { get; set; }

            public string Constrain { get; set; }
}
    }
}
