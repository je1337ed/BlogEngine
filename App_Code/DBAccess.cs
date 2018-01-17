using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SiteData
{
        public class DBAccess
        {

            #region Local Variables


            private System.Data.SqlClient.SqlConnection conn;
            private System.Data.SqlClient.SqlTransaction tr;
            private String connString = null;
            private int cmdTimeout = 120;

            #endregion

            #region Constructor

            public DBAccess()
            {                
                connString = "Data Source=datasourceurl;" +
                    "database=dbname;uid=user;pwd=password;";                
            }
            
            #endregion
                        
            #region Connection Methods

            public SqlConnection Connect()
            {

                if (conn == null)
                {
                    try
                    {
                        conn = new SqlConnection(connString);
                        conn.Open();
                    }

                    catch (Exception e)
                    {
                        throw new ApplicationException("Unable to create connection to database: " + connString, e);
                    }

                }


                return conn;

            }

            public void Disconnect()
            {

                try
                {

                    if ((conn != null))
                    {
                        conn.Close();
                        //close can be called multiple times with no exception 
                    }
                }

                catch (Exception e)
                {

                    throw new ApplicationException("Unable to close connection to database: " + connString, e);

                }

            }

            #endregion


            public System.Data.DataTable OpenDataTable(System.Data.SqlClient.SqlCommand cmd)           
            {

                DataTable dt = new DataTable();
                System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connString);

                //------------------------------------------------------------------------ 
                //Open Connection 
                //------------------------------------------------------------------------ 
                try
                {
                    conn.Open();

                    //------------------------------------------------------------------------ 
                    //Set up command object 
                    //------------------------------------------------------------------------ 
                    cmd.Connection = conn;
                    cmd.CommandTimeout = cmdTimeout;

                    //------------------------------------------------------------------------ 
                    //Load and Return DataTable 
                    //------------------------------------------------------------------------ 
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(cmd);

                    try
                    {
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        DestroyObject(da);
                    }
                    // 
                    return dt;
                }
                // 
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    DestroyObject(conn);
                }

            }

            public void ExecuteSQL(System.Data.SqlClient.SqlCommand cmd)
            {                
                SqlConnection conn = new SqlConnection(connString);

                //------------------------------------------------------------------------ 
                //Open Connection 
                //------------------------------------------------------------------------ 
                try
                {
                    conn.Open();
                    //------------------------------------------------------------------------ 
                    //Execute the command 
                    //------------------------------------------------------------------------ 
                    try
                    {
                        cmd.Connection = conn;
                        cmd.CommandTimeout = cmdTimeout;
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                // 
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    DestroyObject((object)conn);
                }

            }
            

            public void DestroyObject(object oObject)            
            {
                if ((oObject != null))
                {
                    try
                    {
                        if (oObject is System.Data.SqlClient.SqlConnection && ((System.Data.SqlClient.SqlConnection)oObject).State == ConnectionState.Open)
                        {
                            ((System.Data.SqlClient.SqlConnection)oObject).Close();
                        }

                        if (oObject is IDisposable)
                        {
                            ((System.IDisposable)oObject).Dispose();
                        }
                    }

                    finally
                    {
                        oObject = null;
                    }
                }

            }
        }
}
