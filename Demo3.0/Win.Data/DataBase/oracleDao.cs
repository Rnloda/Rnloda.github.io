using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Configuration;
using log4net;
using System.Reflection;
/// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace Win.Data
{
    public class oracleDao : dbIntface
    {
        private OracleConnection _conn = null;
        private OracleCommand _cmd;
        private string _connectionString;
        private OracleTransaction _trans = null;

        private static string m_ConnXML  ;//= ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public oracleDao(string dataSource, string userId, string pwd)
        {

            m_ConnXML = "data source="+dataSource+";user id="+userId+";password="+pwd+"";
            ///  ���� 20090907
            /// ===================================================================================================================
            this._connectionString = m_ConnXML;
            /// ===================================================================================================================

            ///  ���� 2007.7.30
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------  
            //_connectionString = CDbInfo.m_strDbConnStr;
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

            OpenConnection();
        }

        public bool OpenConnection()
        {

            ///  ���� 2007.7.27
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                if (_conn == null || _conn.State == ConnectionState.Closed)
                {
                    _conn = new OracleConnection();
                    _conn.ConnectionString = _connectionString;
                    _conn.Open();

                    int i = 0;
                    while (_conn.State == ConnectionState.Closed)
                    {
                        if (i > 10)
                        {
                            _conn = null;//���ݿ����Ӵ򲻿�
                            //Comm.WriteLog("���ݿ����Ӵ�ʧ�ܣ�" + _connectionString);
                            return false;
                        }
                        System.Threading.Thread.Sleep(500);
                        _conn.Open();
                        i++;
                    }

                }
                _cmd = new OracleCommand();
                _cmd.Connection = _conn;
                //_connectionString = _connectionString;
                return true;

                ///  ���� 2007.7.27
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                string strErrMsg = ex.Message;
                if (strErrMsg.IndexOf("ORA-12541") >= 0 || strErrMsg.IndexOf("ORA-12514") >= 0)
                    strErrMsg = "���ݿ�����ʧ�ܣ��������Ա��ϵ!";

                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return false;
            }
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        ///  ���� 20100127
        /// ===================================================================================================================
        public oracleDao(out string strErrMsg)
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
                    _conn = new OracleConnection();
                    _conn.ConnectionString = _connectionString;
                    _conn.Open();
                }
                _cmd = new OracleCommand();
                _cmd.Connection = _conn;
                return null;
            }
            catch (Exception ex)
            {
                string strErrMsg = "���ݿ�����ʧ�ܣ�����ϵ����Ա����ϸ��Ϣ��" + ex.Message;
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = _cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);

                return ds;
            }
            catch (Exception ex)
            {
                strErrMsg = "��ȡ���ݿ�����ʱ���������볢���ٴβ�������������ˢ��ҳ�档���Դ��ڴ����������Ա��ϵ����ϸ��Ϣ��" + ex.Message;

                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                return _cmd.ExecuteOracleScalar();
            }
            catch (Exception ex)
            {
                strErrMsg = "��ȡ���ݿ�����ʱ���������볢���ٴβ�������������ˢ��ҳ�档���Դ��ڴ����������Ա��ϵ����ϸ��Ϣ��" + ex.Message;
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }
        /// ===================================================================================================================

        public void CloseConnection()
        {
            if (_conn == null) return;

            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
                _conn.Dispose();
            }
            if (_conn != null)
                _conn = null;
        }

        public DataSet GetData(string strSql)
        {

            ///  ���� 2007.7.27
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================

                DataSet ds = new DataSet();
               
               // _cmd.CommandText = strSql;
                OracleDataAdapter adapter = new OracleDataAdapter(strSql, _conn);
               // adapter.SelectCommand = _cmd;
                adapter.Fill(ds);
                
                return ds;

                ///  ���� 2007.7.27
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        public DataSet GetData(string strSql, out System.Data.OracleClient.OracleDataAdapter adapter)
        {

            ///  ���� 2007.7.27
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                {
                    adapter = null;
                    return null;
                }
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================

                DataSet ds = new DataSet();

                _cmd.CommandText = strSql;
                adapter = new OracleDataAdapter();
                adapter.SelectCommand = _cmd;
                adapter.Fill(ds);
                return ds;

                ///  ���� 2007.7.27
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                adapter = null;
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        public DataSet FillDataSet(DataSet dsData, string strSql, string tabName)
        {
            try
            {
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                _cmd.CommandText = strSql;
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = _cmd;

                adapter.Fill(dsData, tabName);

                return dsData;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public DataTable GetDataTable(string strSql)
        {
            string strTable = "T_" + DateTime.Now.ToString("HHmmss");
            DataTable dt = new DataTable(strTable);
            ///  ���� 2007.7.27
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================

                //DataSet ds = new DataSet();

                _cmd.CommandText = strSql;
                OracleDataAdapter adapter = new OracleDataAdapter();
                adapter.SelectCommand = _cmd;
                adapter.Fill(dt);
                 
                //foreach
                //dt = ds.Tables[0].Copy();
                //ds.Clear();
                //ds.Dispose();
                return dt;

                ///  ���� 2007.7.27
                /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            }
            catch(Exception ex)
            {
               // LogWrite log = new LogWrite();
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return dt;
            }
            finally
            {
                this.CloseConnection();
            }
            /// -----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        }

        public int ExecuteNonQuery(string strSql)
        {
            try
            {
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                _cmd.CommandText = strSql;
                return _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        // ���� 20091018
        //=====================================================================================
        public int ExecuteNonQuery(string strSql, out string strErrMsg)
        {
            try
            {
                strErrMsg = null;
                if (this._cmd == null)
                {
                    strErrMsg = "���ݿ����Ӵ���";
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
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                _cmd.CommandText = strSql;
                int len = (buff == null ? 0 : buff.Length);
                _cmd.Parameters.Add(new OracleParameter(strParamName, OracleType.Blob, len));
                if (len > 0) _cmd.Parameters[0].Value = buff;
                else
                {
                    _cmd.Parameters[0].IsNullable = true;
                    _cmd.Parameters[0].Value = DBNull.Value;
                }

                int r = _cmd.ExecuteNonQuery();
                _cmd.Parameters.Clear();

                return r;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                _cmd.CommandText = strSql;
                int len = (parValue == null ? 0 : parValue.Length);
                _cmd.Parameters.Add(new OracleParameter(strParamName, OracleType.LongVarChar, len));
                if (len > 0) _cmd.Parameters[0].Value = parValue;
                else
                {
                    _cmd.Parameters[0].IsNullable = true;
                    _cmd.Parameters[0].Value = DBNull.Value;
                }

                int r = _cmd.ExecuteNonQuery();
                _cmd.Parameters.Clear();

                return r;
            }
            catch (Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                _cmd.CommandText = strSql;
                return _cmd.ExecuteOracleScalar();
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public void ExecuteSqlTran(System.Collections.ArrayList SQLStringList)
        {
            try
            {
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================
                OracleTransaction tx = _conn.BeginTransaction();
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
                    tx.Rollback();
                    log.ErrorFormat("oracle���ݴ���:{0}", E.Message);
                    throw new Exception(E.Message);
                }
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
        /// </summary>
        /// <param name="SQLString">SQL���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
        public int ExecuteSql(string SQLString, string content)
        {
            try
            {
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================

                using (OracleConnection connection = new OracleConnection(_connectionString))
                {

                    OracleParameter myParameter = new OracleParameter("@content", SqlDbType.NText);
                    myParameter.Value = content;
                    _cmd.Parameters.Add(myParameter);
                    try
                    {
                        int rows = _cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        _cmd.Parameters.Clear();
                    }
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public int BeginTransaction()
        {
            try
            {
                ///  ���� 20090903
                /// ===================================================================================================================
                if (this._cmd == null)
                    return -1;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();
                /// ===================================================================================================================

                _trans = _conn.BeginTransaction();
                _cmd.Transaction = _trans;
                return 0;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
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
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #region �洢����

        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>SqlDataReader</returns>
        public OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                OracleConnection connection = new OracleConnection(_connectionString);
                OracleDataReader returnReader;
                connection.Open();
                OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                command.CommandType = CommandType.StoredProcedure;
                returnReader = command.ExecuteReader();
                return returnReader;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="tableName">DataSet����еı���</param>
        /// <returns>DataSet</returns>
        public DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    DataSet dataSet = new DataSet();
                    connection.Open();
                    OracleDataAdapter sqlDA = new OracleDataAdapter();
                    sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                    sqlDA.Fill(dataSet, tableName);
                    connection.Close();
                    return dataSet;
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
                
            }
        }


        /// <summary>
        /// ���� OracleCommand ����(��������һ���������������һ������ֵ)
        /// </summary>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>OracleCommand</returns>
        private OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                OracleCommand command = new OracleCommand(storedProcName, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Clear();
                foreach (OracleParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
                return command;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// <summary>
        /// ִ�д洢���̣�����Ӱ�������		
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="rowsAffected">Ӱ�������</param>
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

                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    int result;
                    connection.Open();
                    OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    rowsAffected = command.ExecuteNonQuery();
                    result = (int)command.Parameters["ReturnValue"].Value;
                    //Connection.Close();
                    return result;
                }
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                rowsAffected = 0;
                return -1;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        /// Modify by caocg in 2013-12-19
        /// <summary>
        /// ִ�д������Ĵ洢���̣�û�з���ֵ	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns></returns>
        public void RunProcedure(string storedProcName, IDataParameter[] parameters, out string[] returnparm,out string errorMsg)
        {
            try
            {
                if (this._cmd == null)
                {
                    returnparm = null;
                    errorMsg = "";
                    return;
                }
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                returnparm = null;
                errorMsg = "";
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {
                    //begin
                    OracleCommand com = connection.CreateCommand();//BuildIntCommand(connection, storedProcName, parameters);
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = storedProcName;
                    List<OracleParameter> param_outList = new List<OracleParameter>();

                    foreach (OracleParameter param in parameters)
                    {
                        if (param.Direction == ParameterDirection.Output)
                            param_outList.Add(param);
                        com.Parameters.Add(param);
                    }

                    connection.Open();
                    com.ExecuteNonQuery();

                    returnparm = new string[param_outList.Count];
                    for (int i = 0; i < param_outList.Count; i++)
                    {
                        returnparm[i] = param_outList[i].Value.ToString();
                    }
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                returnparm = null;
                errorMsg = ex.Message;
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return;
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
        /// �滻sql���Ĳ�����ǰ׺�ַ�
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        private string ParamChar(string Str)
        {
            string oracleChar = ":";
            string sqlserverChar = "@";

            Str = Str.Replace(sqlserverChar, oracleChar);

            return Str;
        }

        /// <summary>
        /// ���� OracleCommand ����ʵ��(��������һ������ֵ)	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>OracleCommand ����ʵ��</returns>
        private OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            try
            {
                if (this._cmd == null)
                    return null;
                if (this._conn == null || this._conn.State == ConnectionState.Closed)
                    this.OpenConnection();

                OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
                //command.Parameters.Add(new OracleParameter("ReturnValue",
                //    OracleType.Int32, 4, ParameterDirection.ReturnValue,
                //    false, 0, 0, string.Empty, DataRowVersion.Default, null));
                return command;
            }
            catch(Exception ex)
            {
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return null;
            }
            finally
            {
                this.CloseConnection();
            }
        }


        /// <summary>
        /// ִ�д������Ĵ洢���̣�û�з���ֵ	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
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

                returnparm = "";
                using (OracleConnection connection = new OracleConnection(_connectionString))
                {

                    connection.Open();

                    OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                returnparm = ex.Message;
                log.ErrorFormat("oracle���ݴ���:{0}", ex.Message);
                return;
            }
            finally
            {
                this.CloseConnection();
            }
        }

        #endregion
    }
}