using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sprotolo13a.Models;

namespace sprotolo13a.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SprotoloController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13a";

        [HttpGet("NameEmailByID")]

        public object Get(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var sql = @"SELECT `name`,`email` FROM `sportolo` WHERE id = @id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            var reader = cmd.ExecuteReader();
            object? data = null;
            if (reader.Read() == true)
            {
                Sportolo sportolo = new Sportolo()
                {
                    name = reader.GetString(0),
                    email = reader.GetString(1),
                };
                if (sportolo != null)
                {
                    data = new { result = "Sikeres lekérés", sportolo.name,sportolo.email };
                };
            }
            connection.Close();
            return data;
        }

        [HttpGet("AllinfoById")]

        public object getAll(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            List<Eredmeny> eredmenyek = new List<Eredmeny>();
            connection.Open();
            var sql = @"SELECT `name`,eredmeny.competition,eredmeny.description FROM `sportolo` INNER JOIN eredmeny ON sportolo.id = eredmeny.sportoloId WHERE sportolo.id = @id;";
            var cmd = new MySqlCommand(sql,connection);
            cmd.Parameters.AddWithValue("@id",id);
            var reader = cmd.ExecuteReader();
            Sportolo name = null;
            while (reader.Read())
            {
                name = new Sportolo() { name = reader.GetString(0) };
                Eredmeny eredmeny = new Eredmeny()
                {
                    competition = reader.GetChar(1),
                    description = reader.GetString(2),
                    
                };

                eredmenyek.Add(eredmeny);
            }
            connection.Close();
            return new { name.name, eredmenyek};
        }

        [HttpGet("RecordsById")]

        public object getrecords(int id)
        {
            var connection = new MySqlConnection(ConnectionString);

            var records = 0;

            connection.Open();
            var sql = @"SELECT count(*) FROM `sportolo` INNER JOIN eredmeny ON sportolo.id = eredmeny.sportoloId WHERE sportolo.id = @id;";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            var number = cmd.ExecuteScalar();
            records = Convert.ToInt32(number);
            connection.Close();
            return new { records};
        }
    }
}