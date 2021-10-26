using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using RAP_WithWPF.Entity;

namespace RAP_WithWPF.Database
{
    public class ResearcherAdapter : ERDAdapter
    {
        //load the basic information of each researcher to optimize loading time
        public LinkedList<Researcher> fetchBasicResearcherDetails()
        {
            LinkedList<Researcher> researchers = new LinkedList<Researcher>();
            conn = GetConnection();
            MySqlDataReader reader = null;
            Researcher researcher = null;

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT id, type,given_name, family_name, title, level FROM researcher";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //check which type of the researcher it is before adding to the list
                    if (String.Equals(reader.GetString(1), "Student"))
                    {
                        researcher = new Student
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(2),
                            FamilyName = reader.GetString(3),
                            FullName = reader.GetString(2) + " " + reader.GetString(3),
                            Title = reader.GetString(4),
                            Level = EmploymentLevel.Student
                        };
                    }
                    else
                    {
                        researcher = new Staff
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(2),
                            FamilyName = reader.GetString(3),
                            FullName = reader.GetString(2) + " " + reader.GetString(3),
                            Title = reader.GetString(4),
                            Level = ParseEnum<EmploymentLevel>(reader.GetString(5)),
                        };
                    }
                    researchers.AddLast(researcher);
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }
            return researchers;
        }

        public Researcher fetchFullResearcherDetails(int id)
        {
            conn = GetConnection();
            MySqlDataReader reader = null;
            Researcher researcher = null;

            try
            {
                Console.WriteLine("Connecting to MySQL...");
                conn.Open();
                string sql = "SELECT * FROM researcher WHERE id = ?id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", id);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //check which type of the researcher it is before adding to the list
                    if (String.Equals(reader.GetString(1), "Student"))
                    {
                        researcher = new Student
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(2),
                            FamilyName = reader.GetString(3),
                            FullName = reader.GetString(2) + " " + reader.GetString(3),
                            Title = reader.GetString(4),
                            Unit = reader.GetString(5),
                            Campus = reader.GetString(6),
                            Email = reader.GetString(7),
                            Photo = new Uri(reader.GetString(8)),
                            Degree = reader.GetString(9),
                            Level = EmploymentLevel.Student,
                            institutionStartTime = reader.GetDateTime(12),
                            Tenure = Math.Round(((DateTime.Now - reader.GetDateTime(12)).TotalDays) / 365, 1, MidpointRounding.ToEven)
                        };
                    }
                    else
                    {
                        researcher = new Staff
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(2),
                            FamilyName = reader.GetString(3),
                            FullName = reader.GetString(2) + " " + reader.GetString(3),
                            Title = reader.GetString(4),
                            Unit = reader.GetString(5),
                            Campus = reader.GetString(6),
                            Email = reader.GetString(7),
                            Photo = new Uri(reader.GetString(8)),
                            Level = ParseEnum<EmploymentLevel>(reader.GetString(11)),
                            institutionStartTime = reader.GetDateTime(12),
                            Tenure = Math.Round(((DateTime.Now - reader.GetDateTime(12)).TotalDays) / 365, 1, MidpointRounding.ToEven)
                        };

                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }
            return researcher;
        }

        //include nearly all relevant data of the researcher except for publications
        public Researcher completeResearcherDetails(Researcher researcher)
        {
            conn = GetConnection();
            MySqlDataReader reader = null;
            LinkedList<Position> positions = null;
            LinkedList<Position> temppositions = new LinkedList<Position>();

            //only staff will have positions
            if (researcher is Staff)
            {
                try
                {
                    Console.WriteLine("Connecting to MySQL...");
                    conn.Open();
                    //Console.WriteLine(researcher.id);
                    string sql = "SELECT * FROM position WHERE id = ?id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("id", researcher.id);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        Position job = new Position
                        {
                            level = ParseEnum<EmploymentLevel>(reader.GetString(1)),
                            start = reader.GetDateTime(2),
                        };
                        if (!reader.IsDBNull(3))
                        {
                            job.end = reader.GetDateTime(3);
                        }
                        else
                        {
                            job.end = null;
                        }
                        temppositions.AddLast(job);
                    }

                    //sort the position list in descending order (latest first, oldest last)
                    var sorted = from Position job in temppositions
                                 orderby job.start
                                 descending
                                 select job;
                    positions = new LinkedList<Position>(sorted);
                    researcher.position = (Position)sorted.First();
                    //remove the current job from the positions list to get the prevJob list
                    positions.RemoveFirst();
                    researcher.PrevPosition = positions;
                }
                catch (MySqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (conn != null)
                        conn.Close();
                }
            }
            

            try
            {
                conn.Open();
                //set the supervisor for student
                if (researcher is Student)
                {
                    int supervisorid = 0;
                    string sql = "SELECT supervisor_id FROM researcher WHERE id = ?id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("id", researcher.id);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        supervisorid = reader.GetInt32(0);
                    }
                    reader.Close();

                    sql = "SELECT id, type,given_name, family_name, title FROM researcher WHERE id = ?id";
                    Researcher supervisor = null;
                    cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("id", supervisorid);
                    reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        supervisor = new Researcher
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(2),
                            FamilyName = reader.GetString(3),
                            FullName = reader.GetString(2) + " " + reader.GetString(3),
                            Title = reader.GetString(4)
                        };
                    }
                    ((Student)researcher).Supervisor = supervisor;
                }
                else
                {
                    ((Staff)researcher).Supervisions = new LinkedList<Researcher>();
                    string sql = "SELECT id, given_name, family_name FROM researcher WHERE supervisor_id = ?id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("id", researcher.id);
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Researcher student = new Researcher
                        {
                            id = reader.GetInt32(0),
                            GivenName = reader.GetString(1),
                            FamilyName = reader.GetString(2),
                            FullName = reader.GetString(1) + " " + reader.GetString(2)
                        };
                        ((Staff)researcher).Supervisions.AddLast(student);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                if (reader != null)
                    reader.Close();
                if (conn != null)
                    conn.Close();
            }

            return researcher;
        }
    }
}
