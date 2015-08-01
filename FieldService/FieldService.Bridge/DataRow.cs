﻿using System.Data.Common;

namespace FieldService.Bridge
{
    public class DataRow
    {
        public readonly DbDataReader _reader;

        public DataRow(DbDataReader reader)
        {
            _reader = reader;
        }

        public int GetInt32(string name)
        {
            return _reader.GetInt32(_reader.GetOrdinal(name));
        }

        public string GetString(string name)
        {
            return _reader.GetString(_reader.GetOrdinal(name));
        }
    }
}