using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using RAP_WithWPF.Entity;

namespace RAP_WithWPF.Database
{
    public class PublicationAdapter : ERDAdapter
    {
        public LinkedList<Publication> fetchBasicPublicationDetails(Researcher r)
        {
            LinkedList<Publication> publications = new LinkedList<Publication>();
            conn = GetConnection();
            MySqlDataReader reader = null;

            try
            {
                Console.WriteLine("Connecting to MySQL....");
                conn.Open();
                string sql = "SELECT pub.doi, title, year, available FROM publication as pub," +
                    "researcher_publication as respub WHERE pub.doi = respub.doi and researcher_id = ?id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", r.id);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Publication article = new Publication
                    {
                        DOI = reader.GetString(0),
                        Title = reader.GetString(1),
                        Year = reader.GetInt32(2),
                        Available = reader.GetDateTime(3)
                    };
                    publications.AddLast(article);
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
            r.publications = publications;
            return publications;
        }

        public Publication completePublicationDetails(Publication p)
        {
            conn = GetConnection();
            MySqlDataReader reader = null;
            Publication article = null;

            try
            {
                Console.WriteLine("Connecting to MySQL....");
                conn.Open();
                string sql = "SELECT * FROM publication WHERE doi = ?doi";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("doi", p.DOI);
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    article = new Publication
                    {
                        DOI = reader.GetString(0),
                        Title = reader.GetString(1),
                        Authors = reader.GetString(2),
                        Year = reader.GetInt32(3),
                        Type = ParseEnum<OutputType>(reader.GetString(4)),
                        CitedAs = reader.GetString(5),
                        Available = reader.GetDateTime(6)
                    };
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
            return article;
        }

    }
}
