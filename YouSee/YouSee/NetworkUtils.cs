//DB methods, IP retrieval, etc. goes here

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace YouSee
{
    class NetworkUtils
    {
        //Retrieves the local IPv4 address
        //https://stackoverflow.com/questions/6803073/get-local-ip-address
        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        //Insert new users into DB
        public static int insertUser(String userIP, String userName)
        {
            int userID = 0;
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    String spName = "insertUser";
                    String passUserName = "@userName";
                    String passUserIP = "@userIP";

                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = spName;
                        command.Parameters.Add(new SqlParameter(passUserIP, userIP));
                        command.Parameters.Add(new SqlParameter(passUserName, userName));

                        sqlConn.Open();
                        try
                        {
                            //Executes the stored procedure and returns the userID
                            userID = (int)command.ExecuteScalar();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }
                        finally
                        {
                            sqlConn.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            App.Current.Properties.Add("savedUserID", userID);
            return userID;
        }//end InsertDB


        //Insert new users into DB
        public static int insertGroup(String groupName, String groupCode, int hostID)
        {
            int groupID = 0;
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    String spName = "spInsertGroup";
                    String passGroupCode = "@GroupCode";
                    String passGroupName = "@GroupName";
                    String passHostID = "@HostID";
                    String passiGroupID = "@iGroupID";

                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = spName;
                        command.Parameters.Add(new SqlParameter(passGroupName, groupName));
                        command.Parameters.Add(new SqlParameter(passGroupCode, groupCode));
                        command.Parameters.Add(new SqlParameter(passHostID, hostID));
                        command.Parameters.Add(new SqlParameter(passiGroupID, 0));

                        sqlConn.Open();
                        try
                        {
                            //Executes the stored procedure and returns the userID
                            groupID = (int)command.ExecuteScalar();
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                        }
                        finally
                        {
                            sqlConn.Close();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return groupID;
        }//end InsertDB



        //Check to see if the random group code is already in the DB
        public static int searchDBForRandom(String myRandom)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            //Could rewrite method to run a stored procedure similar to insertDB() instead of going directly at table
            String query = "SELECT groupCode FROM tGroup WHERE groupCode = '" + myRandom + "';";
            int numResult = 0;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();

                        //Read the result
                        try
                        {
                            SqlDataAdapter daSearchGroups = new SqlDataAdapter(command);
                            DataTable dtReturnedGroups = new DataTable();
                            daSearchGroups.Fill(dtReturnedGroups);
                            numResult = dtReturnedGroups.Rows.Count;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            sqlConn.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return numResult;
        }//End searchRandom



        //This works! Connects xamarin app to ms sql DB... Template Method
        public static void ConnectDB()
        {
            //Used to select 'username' column from returned results
            String myString = "userName";

            //Returns users from DB
            String query = "sELECT userName FROM tUsers";

            //String used to connect to DB
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    SqlCommand execCmd = new SqlCommand(query, sqlConn);
                    execCmd.Connection.Open();

                    //Pulled from final project.. rename vars
                    SqlDataAdapter daFilterStreet = new SqlDataAdapter(execCmd);
                    DataTable dtFilteredStreets = new DataTable();
                    daFilterStreet.Fill(dtFilteredStreets);
                    //Add filtered data to filtered streets list
                    for (int i = 0; i < dtFilteredStreets.Rows.Count; i++)
                    {
                        DataRow dr = dtFilteredStreets.Rows[i];
                        String s = dr[myString].ToString();
                        Console.WriteLine(s);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }//End connectDB
    }
}
