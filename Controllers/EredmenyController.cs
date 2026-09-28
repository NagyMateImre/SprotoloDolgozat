using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using sprotolo13a.Models;
using sprotolo13a.Models.DTOS;

namespace sprotolo13a.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        public string ConnectionString = "server=localhost;uid=root;password=;database=sportolo13a";

        [HttpGet]
        public object Get()
        {
            List<Eredmeny> eredmenyek = new List<Eredmeny>();
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            var sql = @"SELECT * FROM eredmeny";

            var cmd = new MySqlCommand(sql, connection);
            var Reader = cmd.ExecuteReader();
            while (Reader.Read())
            {
                Eredmeny eredmeny = new Eredmeny()
                {
                    id = Reader.GetInt32(0),
                    competition = Reader.GetChar(1),
                    description = Reader.GetString(2),
                    resultTime = Reader.GetDateTime(3),
                    updateTime = Reader.GetDateTime(4),
                    sportoloId = Reader.GetInt32(5),
                };
                eredmenyek.Add(eredmeny);
            }
            connection.Close();
            return eredmenyek;
        }

        [HttpGet("ById")]
        public object GetById(int Id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            var sql = @"SELECT * FROM eredmeny WHERE Id = @id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", Id);
            var Reader = cmd.ExecuteReader();
            object? data = null;
            if (Reader.Read() == true)
            {
                Eredmeny eredmeny = new Eredmeny()
                {
                    id = Reader.GetInt32(0),
                    competition = Reader.GetChar(1),
                    description = Reader.GetString(2),
                    resultTime = Reader.GetDateTime(3),
                    updateTime = Reader.GetDateTime(4),
                    sportoloId = Reader.GetInt32(5),
                };
                if(eredmeny != null)
                {
                    data = new { result = "Sikeres Lekérés Id alapján", Record = eredmeny};
                }
            }
            connection.Close();
            return data;
        }

        [HttpPost]

        public object Post([FromQuery] AddNewEredmenyDTOS addNewEredmenyDTOS)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var sql = @"INSERT INTO `eredmeny`(`competition`, `description`, `resultTime`, `updateTime`, `sportoloId`) VALUES (@competition,@description,@resultTime,@updateTime,@sportoloId)";

            var cmd = new MySqlCommand(sql, connection);

            cmd.Parameters.AddWithValue("@competition", addNewEredmenyDTOS.competition);
            cmd.Parameters.AddWithValue("@description", addNewEredmenyDTOS.description);
            cmd.Parameters.AddWithValue("@resultTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloId", addNewEredmenyDTOS.sportoloId);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { Result = "Sikeres Felvétel"};
        }

        [HttpDelete]

        public object Delete(int id)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();
            var sql = @"DELETE FROM `eredmeny` WHERE Id = @id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);

            cmd.ExecuteNonQuery();

            connection.Close();
            return new { Result = "Sikeres Törlés"};
        }

        [HttpPut]

        public object Put(int id, UpdateEredmenyDTOS updateEredmenyDTOS)
        {
            var connection = new MySqlConnection(ConnectionString);
            connection.Open();

            var sql = @"UPDATE `eredmeny` SET `competition`=@competition,`description`=@description,`updateTime`=@updateTime,`sportoloId`=@sportoloId WHERE Id = @id";

            var cmd = new MySqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@competition", updateEredmenyDTOS.competition);
            cmd.Parameters.AddWithValue("@description", updateEredmenyDTOS.description);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@sportoloId", updateEredmenyDTOS.sportoloId);

            cmd.ExecuteScalar();
            connection.Close();
            return new { result = "Sikeres módosítás"};
        }

        [HttpGet("AllRecord")]

        public object GetRecords()
        {
            int allRecords = 0;

            var connection = new MySqlConnection(ConnectionString);

            connection.Open();
            var sql = @"SELECT COUNT(*) FROM `eredmeny`;";

            var cmd = new MySqlCommand(sql, connection);
            var number = cmd.ExecuteScalar();

            allRecords = Convert.ToInt32(number);

            connection.Close();
            return new { Result = allRecords };
        }
    }
}
