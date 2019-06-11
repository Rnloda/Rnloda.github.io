using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using log4net;
using System.Reflection;

namespace Win.Data
{
    public class sqlDao : dbIntface
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private string _connectionString;
        private SqlTransaction _trans = null;
        private static string m_ConnXML  ;//=ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
       // private static string m_DatabaseTypeXML = System.Configuration.ConfigurationManager.AppSettings["DBType"].ToString(); // ReadDataTypeXML(out m_ConnXML);
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public sqlDao(string ServerSource,string serverDB, string userId, string pwd)
        {
            m_ConnXML = "Server=" + ServerSource + ";Database=" + serverDB + ";Persist Security Info=True;User ID=" + userId + ";Password=" + pwd + "";
            //m_ConnXML = "Server=192.168.209.1;Database=ORCL;Persist Security Info=True;User ID=admintool;Password=admintool";
            ///  增加 20090907
            /// ===================================================================================================================
            this._connectionString = m_ConnXML;
            /// ===================================================================================================================


            ///  增加 2007.7.30
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  
            //_connectionString = CDbInfo.m_strDbConnStr;
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            OpenConnection();
        }

        //=====================================================================================

        public bool OpenConnection()
        {
            try
            {
                if (_conn == null || _conn.State == ConnectionState.Closed)
                {
                    _conn = new System.Data.SqlClient.SqlConnection();
                    _conn.ConnectionString = _connectionString;
                    _conn.Open();

                    int i = 0;
                    while (_conn.State == ConnectionState.Closed)
                    {
                        if (i > 10)
                        {
                            _conn = null;//数据库连接打不开
                            //Comm.WriteLog("数据库连接打开失败！"　+ strConn );
                            return false;
                        }
                        System.Threading.Thread.Sleep(500);
                        _conn.Open();
                        i++;
                    }

                }
                _cmd = new SqlCommand();
                _cmd.Connection = _conn;
                return true;
            }
            catch (Exception ex)
            {
                string strErrMsg = ex.Message;
                if (strErrMsg.IndexOf("ORA-12541") >= 0 || strErrMsg.IndexOf("ORA-12514") >= 0)
                    strErrMsg = "数据库连接失败，请与管理员联系!";

                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return false;
            }
        }

        ///  增加 20100127
        /// ===================================================================================================================
        public sqlDao(out string strErrMsg)
        {
            this._connectionString = m_ConnXML;
            strErrMsg = this.OpenConnection2();
        }


        public string OpenConnection2()
        {
            try
            {
                if (_conn == null || _conn.State == ConnectionState.Closed)
                {
                    _conn = new System.Data.SqlClient.SqlConnection();
                    _conn.ConnectionString = _connectionString;
                    _conn.Open();
                }
                _cmd = new SqlCommand();
                _cmd.Connection = _conn;
                return null;
            }
            catch (Exception ex)
            {
                string strErrMsg = "数据库连接失败！请联系管理员。详细信息：" + ex.Message;
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return strErrMsg;
            }
        }

        public DataSet GetData(string strSql, out string strErrMsg)
        {
            strErrMsg = null;
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = _cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                strErrMsg = "获取数据库数据时发生错误！请尝试再次操作，或者重新刷新页面。若仍存在错误，请与管理员联系。详细信息：" + ex.Message;
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataTable GetDataTable(string strSql)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = _cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds.Tables[0];
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public object ExecuteScale(string strSql, out string strErrMsg)
        {
            strErrMsg = null;
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                _cmd.CommandText = strSql;
                return _cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                strErrMsg = "获取数据库数据时发生错误！请尝试再次操作，或者重新刷新页面。若仍存在错误，请与管理员联系。详细信息：" + ex.Message;
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }
        //=====================================================================================

        public void CloseConnection()
        {
            if (_conn == null) return;

            if (_conn.State == ConnectionState.Open)
                _conn.Close();
            _conn = null;
        }

        public DataSet GetData(string strSql)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = _cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataSet GetData(string strSql, out System.Data.OracleClient.OracleDataAdapter adapter)
        {
            
            adapter = null;
            return null;
        }

        public DataSet FillDataSet(DataSet dsData, string strSql, string tabName)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                SqlDataAdapter Adapter = new SqlDataAdapter();
                Adapter.SelectCommand = _cmd;

                Adapter.Fill(dsData, tabName);

                return dsData;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int ExecuteNonQuery(string strSql)
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                return _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        // 增加 20091018
        //=====================================================================================
        public int ExecuteNonQuery(string strSql, out string strErrMsg)
        {
            try
            {
                strErrMsg = null;
                if (this._cmd == null)
                {
                    strErrMsg = "数据库连接错误！";
                    return -1;
                }
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                return _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                strErrMsg = ex.Message;
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }

        }
        //=====================================================================================

        public int ExecuteNonQuery(string strSql, string strParamName, byte[] buff)
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                _cmd.Parameters.Add(new SqlParameter(strParamName, SqlDbType.Image, buff.Length));
                _cmd.Parameters[0].Value = buff;
                int rtn = _cmd.ExecuteNonQuery();
                _cmd.Parameters.Clear();

                return rtn;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int ExecuteNonQuery(string strSql, string strParamName, string parValue)
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                _cmd.Parameters.Add(new SqlParameter(strParamName, SqlDbType.Text, parValue.Length));
                _cmd.Parameters[0].Value = parValue;
                int rtn = _cmd.ExecuteNonQuery();
                _cmd.Parameters.Clear();

                return rtn;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public object ExecuteScale(string strSql)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _cmd.CommandText = strSql;
                return _cmd.ExecuteScalar();
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public void ExecuteSqlTran(System.Collections.ArrayList SQLStringList)
        {
            if (this._cmd == null)
                return;
            if (this._conn == null || this._conn.State == ConnectionState.Closed)
                this.OpenConnection();

            SqlTransaction tx = _conn.BeginTransaction();
            _cmd.Transaction = tx;
            try
            {
                for (int n = 0; n < SQLStringList.Count; n++)
                {
                    string strsql = SQLStringList[n].ToString();
                    if (strsql.Trim().Length > 1)
                    {
                        _cmd.CommandText = strsql;
                        _cmd.ExecuteNonQuery();
                    }
                }
                tx.Commit();
            }
            catch (System.Data.SqlClient.SqlException E)
            {
                log.ErrorFormat("SQL数据错误:{0}", E.Message);
                tx.Rollback();
                throw new Exception(E.Message);
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, string content)
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    System.Data.SqlClient.SqlParameter myParameter = new System.Data.SqlClient.SqlParameter("@content", SqlDbType.NText);
                    myParameter.Value = content;
                    _cmd.Parameters.Add(myParameter);
                    try
                    {
                        int rows = _cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.SqlClient.SqlException E)
                    {
                        log.ErrorFormat("SQL数据错误:{0}", E.Message);
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        _cmd.Parameters.Clear();
                    }
                }
            }
            catch
            {
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public void AddParamWithValue(string paramName, object value, DbType dbType)
        {
            _cmd.Parameters.AddWithValue(ParamChar(paramName), value).DbType = dbType;

        }

        /// <summary>
        /// 替换sql语句的参数的前缀字符
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private string ParamChar(string Str)
        {
            string oracleChar = ":";
            string sqlserverChar = "@";

            Str = Str.Replace(oracleChar, sqlserverChar);

            //added by hyp in 20081124 start
            //字段||字段 转成 字段+字段
            oracleChar = "||";
            sqlserverChar = "+";

            Str = Str.Replace(oracleChar, sqlserverChar);

            //把to_char转换成str
            oracleChar = "to_char";
            sqlserverChar = "str";

            Str = Str.Replace(oracleChar, sqlserverChar);

            //added by hyp in 20081124 end
            return Str;
        }

        public int BeginTransaction()
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                _trans = _conn.BeginTransaction();
                _cmd.Transaction = _trans;//added by hyp in 20081217
                return 0;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int CommitTransation()
        {
            try
            {
                if (_trans != null)
                {
                    _trans.Commit();
                    _trans = null;
                }
                return 0;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int RollBackTransation()
        {
            try
            {
                if (_trans != null)
                {
                    _trans.Rollback();
                    _trans = null;
                }
                return 0;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #region 带参数的sql
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public DataSet Query(string SQLString, params SqlParameter[] cmdParms)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection _conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand();
                    PrepareCommand(cmd, _conn, null, SQLString, cmdParms);
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();
                        try
                        {
                            da.Fill(ds, "ds");
                            cmd.Parameters.Clear();
                        }
                        catch (System.Data.SqlClient.SqlException ex)
                        {
                            throw new Exception(ex.Message);
                        }
                        return ds;
                    }
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        private void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, string cmdText, SqlParameter[] cmdParms)
        {
            try
            {
                if (this._cmd == null)
                    return;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                if (conn.State != ConnectionState.Open)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandText = cmdText;
                if (trans != null)
                    cmd.Transaction = trans;
                cmd.CommandType = CommandType.Text;//cmdType;
                if (cmdParms != null)
                {
                    foreach (SqlParameter parm in cmdParms)
                        cmd.Parameters.Add(parm);
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public int ExecuteSql(string SQLString, params SqlParameter[] cmdParms)
        {
            try
            {
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        try
                        {
                            PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                            int rows = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                            return rows;
                        }
                        catch (System.Data.SqlClient.SqlException E)
                        {
                            throw new Exception(E.Message);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #endregion


        #region procedure

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                SqlConnection connection = new SqlConnection(_connectionString);
                SqlDataReader returnReader;
                connection.Open();
                SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                returnReader = command.ExecuteReader();
                return returnReader;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    SqlDataAdapter sqlDA = new SqlDataAdapter();
                    sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDA.Fill(dataSet, tableName);
                    connection.Close();
                    return dataSet;
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// 构建 SqlCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand</returns>
        private SqlCommand BuildQueryCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                SqlCommand command = new SqlCommand(storedProcName, connection);
                command.CommandType = CommandType.StoredProcedure;
                foreach (SqlParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                return command;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            try
            {
                if (this._cmd == null)
                {
                    rowsAffected = 0;
                    return -1;
                }
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    int result;
                    connection.Open();
                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    //Connection.Close();
                    return result;
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("SQL数据错误:{0}", ex.Message);
                rowsAffected = 0;
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// added by hyp in 20081212
        /// <summary>
        /// 执行带参数的存储过程，没有返回值	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns></returns>
        public void RunProcedure(string storedProcName, IDataParameter[] parameters, out string returnparm)
        {
            try
            {
                if (this._cmd == null)
                {
                    returnparm = null;
                    return;
                }
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {

                    connection.Open();

                    SqlCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    command.ExecuteNonQuery();
                    returnparm = command.Parameters[parameters[parameters.Length - 1].ParameterName].Value.ToString();
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                returnparm = null;
                return;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// 创建 SqlCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>SqlCommand 对象实例</returns>
        private SqlCommand BuildIntCommand(SqlConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                SqlCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                command.Parameters.Add(new SqlParameter("ReturnValue",
                    SqlDbType.Int, 4, ParameterDirection.ReturnValue,
                    false, 0, 0, string.Empty, DataRowVersion.Default, null));
                return command;
            }
            catch(Exception ex)
            {
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        #endregion

        #region dbIntface 成员

        bool dbIntface.OpenConnection()
        {
            throw new NotImplementedException();
        }

        void dbIntface.CloseConnection()
        {
            throw new NotImplementedException();
        }

        DataSet dbIntface.GetData(string strSql)
        {
            throw new NotImplementedException();
        }

        DataSet dbIntface.GetData(string strSql, out string strErrMsg)
        {
            throw new NotImplementedException();
        }

        DataSet dbIntface.GetData(string strSql, out System.Data.OracleClient.OracleDataAdapter adapter)
        {
            throw new NotImplementedException();
        }

        DataSet dbIntface.FillDataSet(DataSet dsData, string strSql, string tabName)
        {
            throw new NotImplementedException();
        }

        //int dbIntface.ExecuteNonQuery(string strSql)
        //{
        //    throw new NotImplementedException();
        //}

        //int dbIntface.ExecuteNonQuery(string strSql, out string strErrMsg)
        //{
        //    throw new NotImplementedException();
        //}

        //int dbIntface.ExecuteNonQuery(string strSql, string strParamName, byte[] buff)
        //{
        //    throw new NotImplementedException();
        //}

        //object dbIntface.ExecuteScale(string strSql)
        //{
        //    throw new NotImplementedException();
        //}

        //object dbIntface.ExecuteScale(string strSql, out string strErrMsg)
        //{
        //    throw new NotImplementedException();
        //}

        void dbIntface.ExecuteSqlTran(System.Collections.ArrayList SQLStringList)
        {
            throw new NotImplementedException();
        }

        int dbIntface.ExecuteSql(string SQLString, string content)
        {
            throw new NotImplementedException();
        }

        int dbIntface.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        int dbIntface.CommitTransation()
        {
            throw new NotImplementedException();
        }

        int dbIntface.RollBackTransation()
        {
            throw new NotImplementedException();
        }

        //void dbIntface.RunProcedure(string storedProcName, IDataParameter[] parameters, out string returnparm)
        //{
        //    throw new NotImplementedException();
        //}

        #endregion


        public void RunProcedure(string storedProcName, IDataParameter[] parameters, out string[] returnparm, out string errorMsg)
        {
            throw new NotImplementedException();
        }

        void dbIntface.RunProcedure(string storedProcName, IDataParameter[] parameters, out string returnparm)
        {
            throw new NotImplementedException();
        }
    }
}