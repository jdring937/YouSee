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
    public class NetworkUtils
    {
        public static List<double> userLats = new List<double>();
        public static List<double> userLngs = new List<double>();

        //Retrieves the local IPv4 address
        //https://stackoverflow.com/questions/6803073/get-local-ip-address
        public static Dictionary<int, String> groupsDictionary = new Dictionary<int, string>();
        
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
            //App.Current.Properties.Add("savedUserID", userID);
            //Console.WriteLine(App.Current.Properties["savedUserID"]);
            AppProperties.setSavedUserId(userID);
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
                            //Executes the stored procedure and returns the groupID
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

        //Get list of groups that user is in
        public static Dictionary<int, String> getUserGroups()
        {
            //Stores names of groups user is in in a list
            Dictionary<int, String> userGroups = new Dictionary<int, string>();
            List<String> groupName = new List<String>();

            try
            {
            List<int> groupIDs = new List<int>();
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            //String query = "SELECT TOP (100) PERCENT dbo.tGroup.GroupName AS groupName FROM dbo.tUsers INNER JOIN dbo.tGroup_User ON dbo.tUsers.UserID = dbo.tGroup_User.UserID INNER JOIN dbo.tGroup ON dbo.tGroup_User.GroupID = dbo.tGroup.GroupID GROUP BY dbo.tGroup_User.GroupID, dbo.tGroup.GroupName, dbo.tUsers.UserID, dbo.tUsers.userName HAVING(dbo.tUsers.UserID = 66) ORDER BY groupName";
            String userID = App.Current.Properties["savedUserID"].ToString();
            Console.WriteLine(userID);
            String query = "EXEC spGetUserGroups " + userID;
            int numResult = 0;
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

                            groupsDictionary.Clear();

                            for(int i = 0; i < numResult; i++)
                            {
                                DataRow dr = dtReturnedGroups.Rows[i];
                                groupName.Add(dr["groupName"].ToString());
                                groupIDs.Add((int)dr["groupID"]);
                                groupsDictionary.Add(groupIDs[i], groupName[i]);
                                userGroups.Add(groupIDs[i], groupName[i]);
                            }

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
            return userGroups;
        }

        //Get list of groups that user is in
        public static List<String> getUsers()
        {
            List<String> users = new List<String>();

            try
            {
                String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
                //String query = "SELECT TOP (100) PERCENT dbo.tGroup.GroupName AS groupName FROM dbo.tUsers INNER JOIN dbo.tGroup_User ON dbo.tUsers.UserID = dbo.tGroup_User.UserID INNER JOIN dbo.tGroup ON dbo.tGroup_User.GroupID = dbo.tGroup.GroupID GROUP BY dbo.tGroup_User.GroupID, dbo.tGroup.GroupName, dbo.tUsers.UserID, dbo.tUsers.userName HAVING(dbo.tUsers.UserID = 66) ORDER BY groupName";
                int groupID = (int)App.Current.Properties["currentGroupID"];
                int myUser = (int)App.Current.Properties["savedUserID"];
                //String query = "SELECT dbo.tGroup.GroupName, dbo.tGroup.GroupID, LTRIM(RTRIM(dbo.tUsers.userName)) AS userName, dbo.tUsers.UserID FROM dbo.tGroup INNER JOIN dbo.tGroup_User ON dbo.tGroup.GroupID = dbo.tGroup_User.GroupID INNER JOIN dbo.tUsers ON dbo.tGroup_User.UserID = dbo.tUsers.UserID WHERE (dbo.tGroup.GroupID = " + groupID + ") AND (dbo.tUsers.UserID <> " + myUser + ")";
                String query = "SELECT dbo.tGroup_User.GroupID, LTRIM(RTRIM(dbo.tUsers.userName)) AS userName, dbo.tUsers.userLat, dbo.tUsers.userLng, dbo.tGroup_User.UserID FROM dbo.tUsers INNER JOIN dbo.tGroup_User ON dbo.tUsers.UserID = dbo.tGroup_User.UserID WHERE (dbo.tGroup_User.GroupID = " + groupID + ") AND (dbo.tGroup_User.UserID <> " + myUser + ")";
                int numResult = 0;
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();

                        //Read the result
                        try
                        {
                            SqlDataAdapter daSearchUsers = new SqlDataAdapter(command);
                            DataTable dtReturnedUsers = new DataTable();
                            daSearchUsers.Fill(dtReturnedUsers);
                            numResult = dtReturnedUsers.Rows.Count;
                            userLats.Clear();
                            userLngs.Clear();

                            for (int i = 0; i < numResult; i++)
                            {
                                DataRow dr = dtReturnedUsers.Rows[i];
                                users.Add(dr["userName"].ToString());
                                userLats.Add((double)dr["userLat"]);
                                userLngs.Add((double)dr["userLng"]);
                            }

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
            return users;
        }

        //Delete a user from the selected group when they click the trashcan
        //TODO: Rewrite to delte like this, so user can be in two groups with same name... ALso rewrite code that selects active group.. Select based on group id
        //delete from tGroup_User where userID = 148 and GroupID = 299
        public static void DeleteUserFromGroup(int GroupID)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            int userID = Convert.ToInt32(App.Current.Properties["savedUserID"]);
            String query = "EXEC spDeleteUserFromGroup '" + GroupID + "', " + userID;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //delete from tGroup_User where userID = 148 and GroupID = 299
        public static void updateCoords(double lat, double lng)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            int userID = Convert.ToInt32(App.Current.Properties["savedUserID"]);
            String query = "UPDATE tUsers SET userLat = " + lat + ", userLng = " + lng + " WHERE UserID = " + userID;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //Retrieve the groupID when a user enters a group code
        public static int getGroupIdFromGroupCode(String groupCode)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            String query = "EXEC spGetGroupIDFromGroupCode '" + groupCode + "'";
            int groupID = 0;
            int numResult;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();
                        try
                        {
                            //Executes the stored procedure and returns the userID
                            SqlDataAdapter daSearchGroups = new SqlDataAdapter(command);
                            DataTable dtReturnedGroups = new DataTable();
                            daSearchGroups.Fill(dtReturnedGroups);
                            numResult = dtReturnedGroups.Rows.Count;

                            for (int i = 0; i < numResult; i++)
                            {
                                DataRow dr = dtReturnedGroups.Rows[i];
                                groupID = ((int)dr["groupID"]);
                            }
                            Console.WriteLine(groupID.ToString());
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                            Console.WriteLine(query);
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
        }

        //Retrieve the groupID when a user enters a group code
        public static String getGroupNameFromGroupCode(String groupCode)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            String query = "EXEC spGetGroupNameFromCode'" + groupCode + "'";
            String groupName = null;
            int numResult;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();
                        try
                        {
                            //Executes the stored procedure and returns the userID
                            SqlDataAdapter daSearchGroups = new SqlDataAdapter(command);
                            DataTable dtReturnedGroups = new DataTable();
                            daSearchGroups.Fill(dtReturnedGroups);
                            numResult = dtReturnedGroups.Rows.Count;

                            for (int i = 0; i < numResult; i++)
                            {
                                DataRow dr = dtReturnedGroups.Rows[i];
                                groupName = dr["GroupName"].ToString();
                            }
                            Console.WriteLine(groupName);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                            Console.WriteLine(query);
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
            return groupName;
        }

        //Retrieve the groupID when a user enters a group code
        public static String getGroupCodeFromUserIdAndGroupName(int UserID, String groupName)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            String query = "EXEC spGetGroupCodeFromUserIdAndGroupName " + UserID + ", '" + groupName + "'";
            String groupCode = null;
            int numResult;

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();
                        try
                        {
                            //Executes the stored procedure and returns the userID
                            SqlDataAdapter daSearchGroups = new SqlDataAdapter(command);
                            DataTable dtReturnedGroups = new DataTable();
                            daSearchGroups.Fill(dtReturnedGroups);
                            numResult = dtReturnedGroups.Rows.Count;

                            for (int i = 0; i < numResult; i++)
                            {
                                DataRow dr = dtReturnedGroups.Rows[i];
                                groupCode = dr["GroupCode"].ToString();
                            }
                            Console.WriteLine(groupName);
                        }
                        catch (Exception exc)
                        {
                            Console.WriteLine(exc.Message);
                            Console.WriteLine(query);
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
            return groupCode;
        }

        //insert a user into a group after they enter a group code
        public static void insertIntoGroup(int groupID, int userID)
        {
            String connString = @"Server=youseedatabase.cxj5odskcws0.us-east-2.rds.amazonaws.com,1433;DataBase=yousee;User ID=youseeDatabase; Password=yousee18";
            String query = "INSERT INTO tGroup_User(GroupID, UserID) VALUES (" + groupID + ", " + userID + ")";

            try
            {
                using (SqlConnection sqlConn = new SqlConnection(connString))
                {
                    using (SqlCommand command = sqlConn.CreateCommand())
                    {
                        command.CommandText = query;

                        sqlConn.Open();

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //This works! Connects xamarin app to ms sql DB... Template Method
        public static void ConnectDB()
        {
            //Used to select 'username' column from returned results
            String myString = "userName";

            //Returns users from DB
            String query = "SELECT userName FROM tUsers";

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
