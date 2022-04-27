using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Configuration;

namespace ThompsonDatabase
{
    public class User
    {
        private int id;
        private string username;
        private string password;
        private int age;
        private string membership;
        private string degree;
        public int Id { get { return id; } set { id = value; } }
        public string Username { get { return username; } set { username = value; } }
        public string Password { get { return password; } set { password = value; } }
        public int Age { get { return age; } set { age = value; } }
        public string Membership { get { return membership; } set { membership = value; } }
        public string Degree { get { return degree; } set { degree = value; } }
        public User()
        {
            Id = 0;
            Username = "";
            Password = "";
            Age = 0;
            Membership = "";
            Degree = "";
        }
        public User(int id, string username, string password, int age, string membership, string degree)
        {
            Id = id;
            Username = username;
            Password = password;
            Age = age;
            Membership = membership;
            Degree = degree;
        }
        public override string ToString()
        {
            return String.Format("{0}. Username: {1} | Password: {2} | Age: {3} | Membership: {4} | Degree: {5}", Id, Username, Password, Age, Membership, Degree);
        }
    }
    public class ViewUsers
    {
        public static void WriteUsers(List<User> users)
        {
            Console.WriteLine("");
            foreach (User user in users)
            {
                Console.WriteLine("{0}", user);
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string provider = ConfigurationManager.AppSettings["provider"];
                string connectionString = ConfigurationManager.AppSettings["connectionString"];
                DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
                int nextid = 1;
                int repeat = 1;
                while (repeat == 1)
                {
                List<User> users = new List<User>();
                    using (DbConnection conn = factory.CreateConnection())
                    {
                        if (conn == null)
                        {
                            Console.WriteLine("Could not connect to database.");
                            Console.ReadLine();
                            return;
                        }
                        conn.ConnectionString = connectionString;
                        conn.Open();
                        DbCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "select * from Users";
                        using (DbDataReader dr = cmd.ExecuteReader())
                        {
                            while (dr.Read())
                            {
                                int id = Convert.ToInt32(dr["Id"]);
                                string username = Convert.ToString(dr["Username"]).Trim();
                                string password = Convert.ToString(dr["Password"]).Trim();
                                int age = Convert.ToInt32(dr["Age"]);
                                string membership = Convert.ToString(dr["Membership"]).Trim();
                                string degree = Convert.ToString(dr["Degree"]).Trim();
                                User user = new User(id, username, password, age, membership, degree);
                                users.Add(user);
                                if (id == nextid)
                                {
                                    nextid = id + 1;
                                }
                            }
                        }
                        ViewUsers.WriteUsers(users);
                        Console.WriteLine("");
                        Console.WriteLine("These are the current items in the database. Choose what you want to do.");
                        Console.WriteLine("1. Add an item to the database.");
                        Console.WriteLine("2. Remove an item from the database.");
                        Console.WriteLine("3. Quit.");
                        int option = Convert.ToInt32(Console.ReadLine());
                        if (option == 1)
                        {
                            Console.WriteLine("Enter the username: ");
                            string username = Console.ReadLine();
                            Console.WriteLine("Enter the password: ");
                            string password = Console.ReadLine();
                            Console.WriteLine("Enter age: ");
                            int age = Convert.ToInt32(Console.ReadLine());
                            Console.WriteLine("Do they have a membership? (Yes/No): ");
                            string membership = Console.ReadLine();
                            Console.WriteLine("What is their degree? ");
                            string degree = Console.ReadLine();
                            string query = String.Format("insert into Users values ({0}, '{1}', '{2}', '{3}', '{4}', '{5}')", nextid, username, password, age, membership, degree);
                            cmd.CommandText = query;
                            cmd.ExecuteNonQuery();
                        }
                        else if (option == 2)
                        {
                            Console.WriteLine("Enter the number of the user you wish to remove: ");
                            int removeuser = Convert.ToInt32(Console.ReadLine());
                            cmd.CommandText = String.Format("delete from Users where Id = '{0}'", removeuser);
                            cmd.ExecuteNonQuery();
                        }
                        else if (option == 3)
                        {
                            repeat = 0;
                        }
                        else
                        {
                            Console.WriteLine("Invalid value! Please enter 1, 2, or 3.");
                        }
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine(e);
            }
            finally
            {
                Console.WriteLine("Thank you for using this program.");
                Console.ReadLine();
            }
        }
    }
}
