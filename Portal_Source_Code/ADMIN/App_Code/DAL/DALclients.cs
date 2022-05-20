using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using HFCPortal.Common;

public class DALclients
{
  

    public DALclients()
    {

    }
    public int addEditClients(string BranchID, string UserID, decimal Password, string FullName, bool Disable, string Email,string Mobile,DateTime Updatedon,string Operator ,string PostalAddress, int NewRecord)
    {
        //declare SqlConnection and initialize it to the settings in the <connectionStrings> section of the web.config
        
        SqlConnection conn = new SqlConnection(CommonHelper.GetConnectionString());
        //===============================
        //prepare the sql string
        string strSql = "sp_addEditUsers";

        //declare sql command and initalize it
        SqlCommand Command = new SqlCommand(strSql, conn);

        //set the command type
        Command.CommandType = CommandType.StoredProcedure;

        try
        {

            //define the command parameters
            Command.Parameters.Add(new SqlParameter("@OurBranchID", SqlDbType.VarChar));
            Command.Parameters["@OurBranchID"].Direction = ParameterDirection.Input;
            Command.Parameters["@OurBranchID"].Size = 4;
            Command.Parameters["@OurBranchID"].Value = BranchID;

            Command.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar));
            Command.Parameters["@UserID"].Direction = ParameterDirection.Input;
            Command.Parameters["@UserID"].Size = 40;
            Command.Parameters["@UserID"].Value = UserID;

            Command.Parameters.Add(new SqlParameter("@FullName", SqlDbType.VarChar));
            Command.Parameters["@FullName"].Direction = ParameterDirection.Input;
            Command.Parameters["@FullName"].Size = 50;
            Command.Parameters["@FullName"].Value = FullName ;

            Command.Parameters.Add(new SqlParameter("@Password", SqlDbType.Decimal));
            Command.Parameters["@Password"].Direction = ParameterDirection.Input;
            Command.Parameters["@Password"].Value = Password;

            Command.Parameters.Add(new SqlParameter("@Disable", SqlDbType.Bit));
            Command.Parameters["@Disable"].Direction = ParameterDirection.Input;
            Command.Parameters["@Disable"].Value = Disable;

            Command.Parameters.Add(new SqlParameter("@Mobile", SqlDbType.VarChar));
            Command.Parameters["@Mobile"].Direction = ParameterDirection.Input;
            Command.Parameters["@Mobile"].Size = 10;
            Command.Parameters["@Mobile"].Value = Mobile ;

            Command.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar));
            Command.Parameters["@Email"].Direction = ParameterDirection.Input;
            Command.Parameters["@Email"].Size = 50;
            Command.Parameters["@Email"].Value = Email;

            Command.Parameters.Add(new SqlParameter("@OperatorID", SqlDbType.VarChar));
            Command.Parameters["@OperatorID"].Direction = ParameterDirection.Input;
            Command.Parameters["@OperatorID"].Size = 25;
            Command.Parameters["@OperatorID"].Value = Operator;

            Command.Parameters.Add(new SqlParameter("@UpdatedOn", SqlDbType.DateTime));
            Command.Parameters["@UpdatedOn"].Direction = ParameterDirection.Input;
            Command.Parameters["@UpdatedOn"].Value = Updatedon ;

            Command.Parameters.Add(new SqlParameter("@NewRecord", SqlDbType.Int));
            Command.Parameters["@NewRecord"].Direction = ParameterDirection.Input;
            Command.Parameters["@NewRecord"].Size = 1;
            Command.Parameters["@NewRecord"].Value = NewRecord;

            Command.Parameters.Add(new SqlParameter("@PostalAddress", SqlDbType.VarChar ));
            Command.Parameters["@PostalAddress"].Direction = ParameterDirection.Input;
            Command.Parameters["@PostalAddress"].Size = 200;
            Command.Parameters["@PostalAddress"].Value = PostalAddress;
             

            //open the database connection
            conn.Open();
            //execute the command
            return Command.ExecuteNonQuery();
        }
        catch
        {
            throw;
        }
        finally
        {
            Command.Dispose();
            conn.Dispose();
        }
    }
    public DataTable getUsers()
    {
     
        SqlConnection conn = new SqlConnection(CommonHelper.GetConnectionString());
        SqlDataAdapter dAd = new SqlDataAdapter("sp_getClients", conn);
        dAd.SelectCommand.CommandType = CommandType.StoredProcedure;


        DataSet dSet = new DataSet();
        try
        {
            dAd.Fill(dSet, "Clients");
            return dSet.Tables["Clients"];
        }
        catch
        {
            throw;
        }
        finally
        {
            dSet.Dispose();
            dAd.Dispose();
            conn.Close();
            conn.Dispose();
        }
    }
}