using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;

public class SalesDBManager
{
      public SqlConnection conn = new SqlConnection();
      List<string> data = new List<string>();
      public SqlCommand cmd;
    //  public string UserName = "";
      object obj_lock = new object();
      public SalesDBManager()
      {
          //Go4Hosting
          //conn.ConnectionString = @" SERVER=49.50.65.161;DATABASE=Dairy_ERP;UID=sa;PASSWORD=Vyshnavi@123;";
          conn.ConnectionString = @" SERVER=182.18.162.51;DATABASE=Dairy_ERP;UID=sa;PASSWORD=Vyshnavi@123;";
      }
      public SalesDBManager(int proid)
      {
          if (proid == 1)
          {
              //Go4Hosting
              //conn.ConnectionString = @" SERVER=49.50.65.160;DATABASE=purchaseandstores;UID=sa;PASSWORD=Vyshnavi@123;";
              conn.ConnectionString = @" SERVER=182.18.162.51;DATABASE=purchaseandstores;UID=sa;PASSWORD=Vyshnavi@123;";
          }
      }
      public bool insert(SqlCommand _cmd)
      {

          try
          {
              cmd = _cmd;
              lock (cmd)
              {
                  cmd.Connection = conn;
                  cmd.Connection.Open();
                  cmd.ExecuteNonQuery();
                  cmd.Connection.Close();
              }
              return true;
          }
          catch (Exception ex)
          {
              cmd.Connection.Close();
              throw new ApplicationException(ex.Message);
          }
      }

      public DataSet SelectQuery(SqlCommand _cmd)
      {
          lock (obj_lock)
          {
              cmd = _cmd;

              lock (cmd)
              {
                  try
                  {
                      DataSet ds = new DataSet();
                      cmd.Connection = conn;
                      conn.Open();

                      //cmd.ExecuteNonQuery();
                      SqlDataAdapter sda = new SqlDataAdapter();
                      sda.SelectCommand = cmd;
                      sda.Fill(ds, "Table");
                      conn.Close();
                      return ds;
                  }
                  catch (Exception ex)
                  {
                      conn.Close();
                      throw new ApplicationException(ex.Message);
                  }
              }
          }
      }
      public long insertScalar(SqlCommand _cmd)
      {
          //long sno = 0;
          long newId = 0;
          try
          {
              cmd = _cmd;
              lock (cmd)
              {
                  cmd.Connection = conn;
                  cmd.Connection.Open();
                  cmd.ExecuteNonQuery();
                  //sno = 1;
                  newId = (long)cmd.ExecuteScalar();
                  //sno = (long)cmd.ExecuteScalar();
                  cmd.Connection.Close();
              }
              return newId;
          }
          catch (Exception ex)
          {
              cmd.Connection.Close();
              throw new ApplicationException(ex.Message);
          }
          //}
      }
    public  bool insertVehicleData(string TableName, string feildsseparatedbycomma)
    {
        lock (obj_lock)
        {
            try
            {
                cmd = new SqlCommand("insert into " + TableName + " values(" + feildsseparatedbycomma + ")", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch (SqlException exe)
            {
                conn.Close();
                throw new ApplicationException(exe.ErrorCode.ToString());
            }
            return true;
        }
    }

    public bool Update(string table, string Updatestring, string condition)
    {
        lock (obj_lock)
        {
            bool flag = false;
            try
            {
                cmd = new SqlCommand("update " + table + " set " + Updatestring + "where " + condition, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                flag = true;
            }
            catch (Exception ex)
            {
                flag = false;
                conn.Close();
                throw new AccessViolationException(ex.Message);
            }
            return flag;
        }
    }

    public bool Update(string table, string[] fieldNames, string[] fieldValues, string[] conFieldNames, string[] conFieldValues, string[] Operators)
    {
        lock (obj_lock)
        {
            string UpdateString = "";
            string ConditionStr = "";
            cmd = new SqlCommand();
            SqlParameter param;
            try
            {
                if (fieldNames.Count() == fieldValues.Count() && conFieldNames.Count() == conFieldValues.Count())
                {
                    for (int i = 0; i < fieldNames.Count(); i++)
                    {
                        UpdateString += fieldNames[i] + ", ";
                        param = new SqlParameter(fieldNames[i].Split('=')[1], fieldValues[i]);
                        cmd.Parameters.Add(param);
                    }
                    UpdateString = UpdateString.Substring(0, UpdateString.LastIndexOf(","));

                    for (int j = 0; j < conFieldNames.Count(); j++)
                    {
                        ConditionStr += conFieldNames[j] + Operators[j];
                        param = new SqlParameter(conFieldNames[j].Split('=')[1], conFieldValues[j]);
                        cmd.Parameters.Add(param);
                    }

                    cmd.CommandText = "update " + table + " set " + UpdateString + " where " + ConditionStr;
                    cmd.Connection = conn;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch (SqlException exe)
            {
                conn.Close();
                throw new ApplicationException(exe.ErrorCode.ToString());
            }
        }

    }

    public   bool Delete(string table, string condition)
    {
        lock (obj_lock)
        {
            bool flag = false;
            try
            {
                cmd = new SqlCommand("delete from " + table + " where " + condition, conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                flag = true;

            }
            catch (Exception ex)
            {
                flag = false;
                conn.Close();
                throw new ApplicationException(ex.Message);
            }
            return flag;
        }
    }

    public   DataSet GetReports(string table, string columnNames, string condition)
    {
        lock (obj_lock)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDataAdapter sda = new SqlDataAdapter("select " + columnNames + " from " + table + " where " + condition, conn);
                conn.Open();
                sda.Fill(ds, table);
                conn.Close();
                return ds;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }
    //public   DataSet GetReports(string table, string columnNames)
    //{
    //    lock (obj_lock)
    //    {
    //        try
    //        {
    //            DataSet ds = new DataSet();
    //            cmd = new SqlCommand("select " + columnNames + " from " + table, conn);
    //            //MySqlDataAdapter sda = new MySqlDataAdapter("select " + columnNames + " from " + table , conn);
    //            conn.Open();
    //            SqlDataAdapter dr = cmd.ExecuteReader();
    //            if (dr.HasRows)
    //                ds = DataReaderToDataSet(dr);
    //            //sda.Fill(ds, table);
    //            conn.Close();
    //            return ds;
    //        }
    //        catch (Exception ex)
    //        {
    //            conn.Close();
    //            throw new ApplicationException(ex.Message);
    //        }
    //    }
    //}

    ///    <summary>
    ///    Converts a MySqlDataReader to a DataSet
    ///    <param name='reader'>
    /// MySqlDataReader to convert.</param>
    ///    <returns>
    /// DataSet filled with the contents of the reader.</returns>
    ///    </summary>
    public   DataSet DataReaderToDataSet(SqlDataReader reader)
    {
        lock (obj_lock)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table

                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();

                if (schemaTable != null)
                {
                    // A query returning records was executed

                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }

                    dataSet.Tables.Add(dataTable);

                    // Fill the data table we just created

                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();

                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);

                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned

                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            return dataSet;
        }
    }
    public int Update(SqlCommand _cmd)
    {
        lock (obj_lock)
        {
            try
            {
                int i = 0;
                cmd = _cmd;
                lock (cmd)
                {
                    cmd.Connection = conn;
                    cmd.Connection.Open();
                    i=cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    return i;
                }
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }

    public void Delete(SqlCommand _cmd)
    {
        lock (obj_lock)
        {
            try
            {
                cmd = _cmd;
                cmd.Connection = conn;
                cmd.Connection.Open();
             int ii=   cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                throw new ApplicationException(ex.Message);
            }
        }
    }
    public static DateTime GetTime(SqlConnection conn)
    {

        DataSet ds = new DataSet();
        DateTime dt = DateTime.Now;
        SqlCommand cmd = new SqlCommand("SELECT GETDATE()");
        cmd.Connection = conn;
        if (cmd.Connection.State == ConnectionState.Open)
        {
            cmd.Connection.Close();
        }
        conn.Open();
        //cmd.ExecuteNonQuery();
        SqlDataAdapter sda = new SqlDataAdapter();
        sda.SelectCommand = cmd;
        sda.Fill(ds, "Table");
        conn.Close();
        if (ds.Tables[0].Rows.Count > 0)
        {
            dt = (DateTime)ds.Tables[0].Rows[0][0];
        }
        return dt;
    }
}