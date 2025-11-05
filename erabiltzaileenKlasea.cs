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
                // Crea el DataSource
                dataSource = NpgsqlDataSource.Create(connectionString);

                // Opcional: probar una conexión rápida
                await using var conn = await dataSource.OpenConnectionAsync();
                //MessageBox.Show("Conexión a PostgreSQL establecida correctamente.");
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
                await using var conn = await dataSource.OpenConnectionAsync();
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
                await using var conn = await dataSource.OpenConnectionAsync();
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
    }
}