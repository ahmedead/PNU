using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SharedSQL
{
    public class Parameters
    {
        string pName;
        object pValue;
        DbType pType;

        #region Properites

        public DbType PType
        {
            get { return pType; }
            set { pType = value; }
        }

        public object PValue
        {
            get { return pValue; }
            set { pValue = value; }
        }

        public string PName
        {
            get { return pName; }
            set { pName = value; }
        }
        #endregion

        public Parameters(string _pName, object _pValue, DbType _pType)
        {
            PName = _pName;
            PValue = _pValue;
            PType = _pType;
        }

        public Parameters()
        {

        }


    }

    public class PrimaryKeys
    {
        public string COLUMN_NAME { get; set; }

        public string TABLE_NAME { get; set; }
    }
}
