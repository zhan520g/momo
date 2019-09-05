﻿//-----------------------------------------------------------------------
// Modified by:
// Description: 数据层接口实现
//-----------------------------------------------------------------------
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace momo.Infrastructure.Dapper
{
    public class DataAccess : IDataAccess
    {
        #region Initialize

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// 数据库类型枚举
        /// </summary>
        private readonly DataBaseTypeEnum _dataBaseType;

        /// <summary>
        /// 日志记录
        /// </summary>
        private ILogger<DataAccess> _logger { get; set; }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="dataBaseType">数据库类型</param>
        public DataAccess(string connectionString, DataBaseTypeEnum dataBaseType)
        {
            //属性注入日志记录
            _logger = NullLogger<DataAccess>.Instance;

            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("创建数据库连接失败，错误信息：数据库连接字符串为空");
                throw new ArgumentNullException("Connection String is null or empty");
            }
            _connectionString = connectionString;
            _dataBaseType = dataBaseType;
        }

        #endregion

        #region APIs

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <returns></returns>
        public bool CloseConnection(IDbConnection connection)
        {
            bool result = false;
            try
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                _logger.LogError($"数据库连接关闭失败，错误信息：{ex.Message}");
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection DbConnection()
        {
            IDbConnection connection = null;
            switch (_dataBaseType)
            {
                case DataBaseTypeEnum.SqlServer:
                    connection = new SqlConnection(_connectionString);
                    break;
                case DataBaseTypeEnum.MySql:
                    connection = new MySqlConnection(_connectionString);
                    break;
            };
            return connection;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T Execute<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            T obj = default(T);
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (hasTransaction)
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                obj = connection.QueryFirstOrDefault<T>(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                obj = connection.QueryFirstOrDefault<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 Execute 方法出错，错误信息：{ex.Message}");
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text)
                        {
                            obj = connection.QueryFirstOrDefault<T>(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            obj = connection.QueryFirstOrDefault<T>(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 Execute 方法出错，错误信息：{ex.Message}");
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行SQL语句返回Object对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T Execute<T>(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            T obj = default(T);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    obj = connection.QueryFirstOrDefault<T>(sql, param, transaction, null, CommandType.Text);
                }
                else
                {
                    obj = connection.QueryFirstOrDefault<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 Execute 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return obj;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            T obj = default(T);
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (hasTransaction)
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 ExecuteAsync 方法出错，错误信息：{ex.Message}");
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text) 
                        {
                            //直接用connection查询
                            obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 ExecuteAsync 方法出错，错误信息：{ex.Message}");
                    }
                }
            }
            return obj;
        }

       
        /// <summary>
        /// 执行SQL语句返回对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> ExecuteAsync<T>(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            T obj = default(T);
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, CommandType.Text);
                }
                else
                {
                    obj = await connection.QueryFirstOrDefaultAsync<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 ExecuteAsync 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return obj;
        }

        /// <summary>
        /// 执行SQL语句或存储过程，返回IList<T>对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public IList<T> ExecuteIList<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            IList<T> list = null;
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                if (hasTransaction)
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                list = connection.Query<T>(sql, param, transaction, true, null, CommandType.Text).ToList();
                            }
                            else
                            {
                                list = connection.Query<T>(sql, param, transaction, true, null, CommandType.StoredProcedure).ToList();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 ExecuteIList<T> 方法出错，错误信息：{ex.Message}");
                            throw ex;
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text)
                        {
                            list = connection.Query<T>(sql, param, null, true, null, CommandType.Text).ToList();
                        }
                        else
                        {
                            list = connection.Query<T>(sql, param, null, true, null, CommandType.StoredProcedure).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 ExecuteIList<T> 方法出错，错误信息：{ex.Message}");
                        throw ex;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句或存储过程，返回IList<T>对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public IList<T> ExecuteIList<T>(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            IList<T> list = null;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    list = connection.Query<T>(sql, param, transaction, true, null, CommandType.Text).ToList();
                }
                else
                {
                    list = connection.Query<T>(sql, param, transaction, true, null, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 ExecuteIList<T> 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句或存储过程，返回IList<T>对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<IList<T>> ExecuteIListAsync<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            IList<T> list = null;
            using (var connection = DbConnection()) //连接
            {
                if (connection.State == ConnectionState.Closed) //判断是否连接
                {
                    connection.Open();
                }

                if (hasTransaction) //是否通过事务执行
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                var data = await connection.QueryAsync<T>(sql, param, transaction, null, CommandType.Text);
                                list = data.ToList();
                            }
                            else
                            {
                                var data = await connection.QueryAsync<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                                list = data.ToList();
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 ExecuteIListAsync<T> 方法出错，错误信息：{ex.Message}");
                            throw ex;
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text) //命令为文本
                        {
                            var data = await connection.QueryAsync<T>(sql, param, null, null, CommandType.Text);
                            list = data.ToList();
                        }
                        else
                        {
                            var data = await connection.QueryAsync<T>(sql, param, null, null, CommandType.StoredProcedure);
                            list = data.ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 ExecuteIListAsync<T> 方法出错，错误信息：{ex.Message}");
                        throw ex;
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句或存储过程，返回IList<T>对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<IList<T>> ExecuteIListAsync<T>(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            IList<T> list = null;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    var data = await connection.QueryAsync<T>(sql, param, transaction, null, CommandType.Text);
                    list = data.ToList();
                }
                else
                {
                    var data = await connection.QueryAsync<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                    list = data.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 ExecuteIListAsync<T> 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return list;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回受影响行数
        /// </summary>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (hasTransaction)
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                result = connection.Execute(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                result = connection.Execute(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 ExecuteNonQuery 方法出错，错误信息：{ex.Message}");
                            throw ex;
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text)
                        {
                            result = connection.Execute(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            result = connection.Execute(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 ExecuteNonQuery 方法出错，错误信息：{ex.Message}");
                        throw ex;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回受影响行数
        /// </summary>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    result = connection.Execute(sql, param, transaction, null, CommandType.Text);
                }
                else
                {
                    result = connection.Execute(sql, param, transaction, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 ExecuteNonQuery 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回受影响行数
        /// </summary>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (hasTransaction)
                {
                    using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        try
                        {
                            if (commandType == CommandType.Text)
                            {
                                result = await connection.ExecuteAsync(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                result = await connection.ExecuteAsync(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            _logger.LogError($"SQL语句：{sql}，使用系统事务执行 ExecuteNonQueryAsync 方法出错，错误信息：{ex.Message}");
                            throw ex;
                        }
                    }
                }
                else
                {
                    try
                    {
                        if (commandType == CommandType.Text)
                        {
                            result = await connection.ExecuteAsync(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            result = await connection.ExecuteAsync(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"SQL语句：{sql}，不使用系统事务执行 ExecuteNonQueryAsync 方法出错，错误信息：{ex.Message}");
                        throw ex;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 执行SQL语句或存储过程返回受影响行数
        /// </summary>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">外部事务</param>
        /// <param name="connection">数据库连接</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<int> ExecuteNonQueryAsync(string sql, object param, IDbTransaction transaction, IDbConnection connection, CommandType commandType = CommandType.Text)
        {
            int result = 0;
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            try
            {
                if (commandType == CommandType.Text)
                {
                    result = await connection.ExecuteAsync(sql, param, transaction, null, CommandType.Text);
                }
                else
                {
                    result = await connection.ExecuteAsync(sql, param, transaction, null, CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"SQL语句：{sql}，使用外部事务执行 ExecuteNonQueryAsync 方法出错，错误信息：{ex.Message}");
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 执行语句返回单个值对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public T ExecuteScalar<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            T result;
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                try
                {
                    if (hasTransaction)
                    {
                        using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            if (commandType == CommandType.Text)
                            {
                                result = connection.ExecuteScalar<T>(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                result = connection.ExecuteScalar<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                        }
                    }
                    else
                    {
                        if (commandType == CommandType.Text)
                        {
                            result = connection.ExecuteScalar<T>(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            result = connection.ExecuteScalar<T>(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SQL语句：{sql}，执行 ExecuteScalar<T> 方法出错，错误信息：{ex.Message}");
                    throw ex;
                }
            }
            return result;
        }

        /// <summary>
        /// 执行语句返回单个值对象
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="sql">SQL语句 or 存储过程名</param>
        /// <param name="param">参数</param>
        /// <param name="hasTransaction">是否使用事务</param>
        /// <param name="commandType">命令类型</param>
        /// <returns></returns>
        public async Task<T> ExecuteScalarAsync<T>(string sql, object param, bool hasTransaction = false, CommandType commandType = CommandType.Text)
        {
            T result;
            using (var connection = DbConnection())
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                try
                {
                    if (hasTransaction)
                    {
                        using (IDbTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            if (commandType == CommandType.Text)
                            {
                                result = await connection.ExecuteScalarAsync<T>(sql, param, transaction, null, CommandType.Text);
                            }
                            else
                            {
                                result = await connection.ExecuteScalarAsync<T>(sql, param, transaction, null, CommandType.StoredProcedure);
                            }
                        }
                    }
                    else
                    {
                        if (commandType == CommandType.Text)
                        {
                            result = await connection.ExecuteScalarAsync<T>(sql, param, null, null, CommandType.Text);
                        }
                        else
                        {
                            result = await connection.ExecuteScalarAsync<T>(sql, param, null, null, CommandType.StoredProcedure);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"SQL语句：{sql}，执行 ExecuteScalarAsync<T> 方法出错，错误信息：{ex.Message}");
                    throw ex;
                }
            }
            return result;
        }

        #endregion
    }
}
