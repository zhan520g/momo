using System;
using momo.Infrastructure.Configuration;

namespace momo.Infrastructure.Dapper
{
    public class DBManager
    {
        #region Initialize

        [ThreadStatic]
        private static IDataAccess _sMsSqlFactory;

        [ThreadStatic]
        private static IDataAccess _sMySqlFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        private static IDataAccess CreateDataAccess(ConnectionParameter cp)
        {
            return new DataAccessProxy(DataAccessProxyFactory.Create(cp));
        }

        #endregion

        #region APIs

        /// <summary>
        /// MsSQL 数据库连接字符串
        /// </summary>
        public static IDataAccess MsSQL
        {
            get
            {
                ConnectionParameter cp;
                if (_sMsSqlFactory == null)
                {
                    cp = new ConnectionParameter
                    {
                        ConnectionString = ConfigurationManager.GetConfig("ConnectionStrings:MsSQLConnection"),
                        DataBaseType = DataBaseTypeEnum.SqlServer
                    };
                    _sMsSqlFactory = CreateDataAccess(cp);
                }
                return _sMsSqlFactory;
            }
        }

        /// <summary>
        /// MySQL 数据库连接字符串
        /// </summary>
        public static IDataAccess MySQL
        {
            get
            {
                ConnectionParameter cp;
                if (_sMySqlFactory == null)
                {
                    cp = new ConnectionParameter
                    {
                        ConnectionString = ConfigurationManager.GetConfig("ConnectionStrings:MySQLConnection"),
                        DataBaseType = DataBaseTypeEnum.MySql
                    };
                    _sMySqlFactory = CreateDataAccess(cp);
                }
                return _sMySqlFactory;
            }
        }

        #endregion
    }
}
