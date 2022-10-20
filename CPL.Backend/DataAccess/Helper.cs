﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Cover.Backend.DataAccess
{
    public class Helper
    {
        #region "Properties"

        private static String _cplCS;
        public static String cplCS
        {
            get
            {
                 if (String.IsNullOrEmpty(_cplCS))
                 {
                    _cplCS = System.Configuration.ConfigurationManager.ConnectionStrings["ProyectoCPL"].ToString();
                     if (!_cplCS.Contains("Data Source"))
                        _cplCS = AB.Common.Encryption.Decrypt(_cplCS);
                 }
                 return _cplCS;
            }
            set
            {
                _cplCS = value;
            }
        }

        #endregion

        #region "DataTable_Events"

        public static DataTable ExecuteDataTable(String storeProcedure, List<SqlParameter> parameters)
        {
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(cplCS);
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                var dt = new DataTable();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
        }

        public static DataSet ExecuteDataSet(String storeProcedure, List<SqlParameter> parameters)
        {
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(cplCS);
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                var ds = new DataSet();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
        }


        public static DataTable ExecuteDataTable(SqlTransaction sqlTran, String storeProcedure, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlTran.Connection, sqlTran);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                var dt = new DataTable();
                SqlDataAdapter sqlAdp = new SqlDataAdapter(sqlCmd);
                sqlAdp.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "Object_Events"

        public static Object ExecuteScalar(String storeProcedure, List<SqlParameter> parameters)
        {
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(cplCS);
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                return sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
        }

        public static Object ExecuteScalar(SqlTransaction sqlTran, String storeProcedure, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlTran.Connection, sqlTran);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                return sqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region "Execute_Events"

        public static void ExecuteNonQuery(String storeProcedure, List<SqlParameter> parameters)
        {
            SqlConnection sqlConn = null;
            try
            {
                sqlConn = new SqlConnection(cplCS);
                sqlConn.Open();
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlConn);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (sqlConn != null)
                    sqlConn.Close();
            }
        }

        public static void ExecuteNonQuery(SqlTransaction sqlTran, String storeProcedure, List<SqlParameter> parameters)
        {
            try
            {
                SqlCommand sqlCmd = new SqlCommand(storeProcedure, sqlTran.Connection, sqlTran);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                    foreach (var p in parameters)
                        sqlCmd.Parameters.Add(p);
                sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

    }
}