using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Dashboard1.Licence
{
    class ValidateLicence
    {
        public static string GlobalCon = "Data Source=tcp:swxpe.database.windows.net;Initial Catalog=swxpe;Integrated Security=False;Persist Security Info=False;User ID=swxpeadmin;Password=admin@#321";
        public static string validateLicnece()
        {
            var lst = getLicenceData();
            var deviceId = GetMacAddress();
            string result = "";
            if (lst.Count > 0)
            {
                foreach (var k in lst)
                {
                    result = CheckLicenceStatus(k.key, deviceId.ToString());
                }
                return result;
            }
            else
            {
                return "NA";
            }

        }

        public static List<LicenceData> getLicenceData()
        {
            List<LicenceData> listA = new List<LicenceData>();
            var filepath = "Data/ProfileData/licence.csv";
            if (File.Exists(filepath))
            {
                var count = 0;
                using (var reader = new StreamReader(filepath))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {
                        }
                        else
                        {
                            LicenceData lc = new LicenceData();
                            count++;
                            lc.key = values[0].ToString();
                            lc.days = values[1].ToString();
                            listA.Add(lc);
                        }
                    }
                    reader.Dispose();
                }
                if (count > 0)
                {
                   

                }
            }
            else
            {

            }
            return listA;
        }

        
        public static string CheckLicenceStatus(string key, string deviceId)
        {
            SqlConnection con = new SqlConnection(); con.ConnectionString = GlobalCon;
            SqlCommand cmd = new SqlCommand();
            try
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                cmd.Connection = con;
                cmd.CommandText = "pValidateLicenceKey";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@deviceId", deviceId);
                cmd.Parameters.AddWithValue("@LicenceKey", key);
                // Run the sproc  
                DataSet ds = new DataSet();

                DataTable dt1 = new DataTable();

                var reader = cmd.ExecuteScalar();
                return reader.ToString();
            }
            catch (Exception ex)
            {
                return "Error :" + ex.Message.ToString();
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
                cmd.Parameters.Clear();
                cmd.Dispose();
                con.Dispose();
            }
        }


        //public List<Users.output> GetUsersList(string id, string UserCode)
        //{
        //    List<Users.output> obj1 = new List<Users.output>();
        //    SqlConnection con = new SqlConnection(); con.ConnectionString = GlobalCon;
        //    SqlCommand cmd = new SqlCommand();
        //    try
        //    {

        //        if (con.State == ConnectionState.Closed)
        //            con.Open();
        //        cmd.Connection = con;
        //        cmd.CommandText = "pAdminGetUsersList";
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserRole", id);
        //        cmd.Parameters.AddWithValue("@UserCode", UserCode);
        //        cmd.Parameters.AddWithValue("@StartDate", DBNull.Value);
        //        cmd.Parameters.AddWithValue("@EndDate", DBNull.Value);
        //        // Run the sproc  
        //        DataSet ds = new DataSet();

        //        DataTable dt1 = new DataTable();

        //        var reader = cmd.ExecuteReader();
        //        //reader.Read();
        //        dt1.Load(reader);
        //        ds.Tables.Add(dt1);
        //        reader.Close();

        //        if (ds.Tables.Count > 0)
        //        {
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                Users.output enq1;
        //                foreach (DataRow row in ds.Tables[0].Rows)
        //                {
        //                    enq1 = new Users.output();
        //                    enq1.UserCode = row["UserCode"].ToString();
        //                    enq1.FullName = row["FullName"].ToString();
        //                    enq1.EmailID = row["EmailID"].ToString();
        //                    enq1.ContactNo1 = row["ContactNo1"].ToString();
        //                    enq1.UserRole = row["UserRole"].ToString();
        //                    enq1.EmployeeCode = row["EmployeeCode"].ToString();
        //                    enq1.EntryDate = row["EntryDate"].ToString();
        //                    enq1.IsActive = (bool)row["IsActive"];
        //                    enq1.ReturnValue = "Success";
        //                    obj1.Add(enq1);
        //                    enq1 = null;
        //                }
        //            }
        //            else
        //            {
        //                Users.output enq1;
        //                enq1 = new Users.output();
        //                enq1.ReturnValue = "No Record Found.";
        //                obj1.Add(enq1);
        //                enq1 = null;
        //            }

        //        }
        //        return obj1;
        //    }
        //    catch (Exception ex)
        //    {
        //        Users.output enq1 = new Users.output();
        //        enq1.ReturnValue = "Error :" + ex.Message.ToString();
        //        obj1.Add(enq1);

        //        return obj1;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //            con.Close();
        //        cmd.Parameters.Clear();
        //        cmd.Dispose();
        //        con.Dispose();
        //    }


        //}

        public class LicenceData
        {
            public string key { get; set; }
            public string days { get; set; }

        }

        public static string GetMacAddress()
        {
            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    // Only consider Ethernet network interfaces
            //    if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet &&
            //        nic.OperationalStatus == OperationalStatus.Up)
            //    {
            //        return nic.GetPhysicalAddress();
            //    }
            //}
            //return null;

            string mac = "";
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {

                if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
                {
                    if (nic.GetPhysicalAddress().ToString() != "")
                    {
                        mac = nic.GetPhysicalAddress().ToString();
                    }
                }
            }

            return mac;

        }

    }
}
