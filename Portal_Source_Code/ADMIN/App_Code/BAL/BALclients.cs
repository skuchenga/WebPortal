using System;
using System.Collections.Generic;
using System.Web;
using System.Data;

/// <summary>
/// Summary description for BALusers
/// </summary>
public class BALclients
{
    public BALclients()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public int addEditClients(string BranchID, string UserID, decimal Password, string FullName, bool Disable, string Email, string Mobile, DateTime Updatedon, string Operator, string PostalAddress, int NewRecord)
    {
        DALclients DAL = new DALclients();
        try
        {
            return DAL.addEditClients(BranchID, UserID, Password, FullName, Disable, Email, Mobile, Updatedon, Operator, PostalAddress, NewRecord);
        }
        catch
        {
            throw;
        }
        finally
        {
            DAL = null;
        }
    }
    public DataTable getUsers()
    {
        DALclients DAL = new DALclients();
        try
        {
            return DAL.getUsers();
        }
        catch
        {
            throw;
        }
        finally
        {
            DAL = null;
        }
    }
}