using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Infrastructure.Dapper
{
    public interface IDataRepository
    {
        /// <summary>
        /// 获取 SQL 语句
        /// </summary>
        /// <param name="commandName"></param>
        /// <returns></returns>
        string GetCommandSQL(string commandName);

        /// <summary>
        /// 批量写入 SQL 语句
        /// </summary>
        void LoadDataXmlStore();
    }
}
