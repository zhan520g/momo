//-----------------------------------------------------------------------
// Modified by:
// Description: XML 中的 SQL 对应类
//-----------------------------------------------------------------------

namespace momo.Infrastructure.Dapper
{
    public class SqlCommand
    {
        /// <summary>
        /// SQL语句名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// SQL语句或存储过程内容
        /// </summary>
        public string Sql { get; set; }
    }
}
