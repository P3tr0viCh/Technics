using P3tr0viCh.Database;
using P3tr0viCh.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace Technics
{
    public static partial class Utils
    {
        public const int ListEditId = -1;

        public static BindingList<T> ToBindingList<T>(this IEnumerable<T> list) where T : BaseId
        {
            return new BindingList<T>(list.ToList());
        }

        public static DataTableFile DataTableFileCreateNew(DataTable table)
        {
            return new DataTableFile()
            {
                Table = table
            };
        }

        public static DataTable DataTableCreateNew(string name)
        {
            return new DataTable()
            {
                TableName = name
            };
        }

        public static void DataTableFileAddColumn(DataTable table, Type type, string name)
        {
            table.Columns.Add(new DataColumn()
            {
                DataType = type,
                ColumnName = name,
            });
        }
    }
}