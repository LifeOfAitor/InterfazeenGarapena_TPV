using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace erronkaTPVsistema
{
    static class erabiltzaileenKlasea
    {
        private static NpgsqlDataSource? dataSource;
        /*
         * Datu basera konektatzeko behar da NpSql instalatzea, horretarako NuGet Package Manager erabili daiteke.
         * Komando hau erabiliz: NuGet\Install-Package Npgsql -Version 9.0.4
         * Web orria: https://www.nuget.org/packages/Npgsql/
         */
        public static async Task ConnectDatabaseAsync(string database)
        {
            string user = "admin";
            string password = "admin";
            var connectionString = $"Host=localhost;Port=5432;Username={user};Password={password};Database={database}";

            try
            {
                //sortzen du datu basera iristeko beharrezkoa den dataSource-a (ate bat bezala)
                dataSource = NpgsqlDataSource.Create(connectionString);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar con la base de datos: {ex.Message}");
            }
        }

        // erabiltzaileak existitzen badira egiaztatzeko eta admin diren ala ez
        public static async Task<bool> checkErabiltzaileak(string erabiltzaile, string pasahitza)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return false;
            }

            const string query = "SELECT COUNT(*) FROM Erabiltzaileak WHERE izena = @erabiltzaile AND pasahitza = @pasahitza;";

            try
            {
                await using var conn = await dataSource.OpenConnectionAsync(); // datu basera konektatzen da
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@erabiltzaile", erabiltzaile);
                cmd.Parameters.AddWithValue("@pasahitza", pasahitza);

                var result = (long)await cmd.ExecuteScalarAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea eabiltzailea bilatzen: {ex.Message}");
                return false;
            }
        }

        //administratzailea den ala ez konprobatzen du
        public static async Task<bool> checkAdmin(string erabiltzaile)
        {
            if (dataSource == null)
            {
                Console.WriteLine("Ez dago konexiorik sortuta");
                return false;
            }

            const string query = "SELECT mota FROM Erabiltzaileak WHERE izena = @erabiltzaile;";

            try
            {
                await using var conn = await dataSource.OpenConnectionAsync();// datu basera konektatzen da
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@erabiltzaile", erabiltzaile);

                var tipo = await cmd.ExecuteScalarAsync();

                return tipo.ToString().Equals("admin");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorea egon da administratzailea konprobatzeko momentuan: {ex.Message}");
                return false;
            }
        }
        public static List<string> kargatuErabiltzaileak()
        {
            List<string> erabiltzaileak = new List<string>();
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return erabiltzaileak;
            }
            const string query = "SELECT izena FROM Erabiltzaileak WHERE mota = 'user';";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    erabiltzaileak.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erabiltzaileak kargatzerakoan: {ex.Message}");
            }
            return erabiltzaileak;
        }
        public static void sortuErabiltzailea(string izena, string pasahitza)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return;
            }
            const string query = "INSERT INTO Erabiltzaileak (izena, pasahitza, mota) VALUES (@izena, @pasahitza, 'user');";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@izena", izena);
                cmd.Parameters.AddWithValue("@pasahitza", pasahitza);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erabiltzailea sortzerakoan: {ex.Message}");
            }
        }
        public static void ezabatuErabiltzailea(string izena)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return;
            }
            const string query = "DELETE FROM Erabiltzaileak WHERE izena = @izena;";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@izena", izena);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erabiltzailea ezabatzekoan: {ex.Message}");
            }
        }
        public static void aldatuErabiltzailea(string izena, string pasahitza)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return;
            }
            const string query = "UPDATE Erabiltzaileak SET pasahitza = @pasahitza WHERE izena = @izena;";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@izena", izena);
                cmd.Parameters.AddWithValue("@pasahitza", pasahitza);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erabiltzailea aldatzerakoan: {ex.Message}");
            }
        }
    }
}