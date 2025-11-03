using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static async Task ConnectDatabaseAsync(string user, string password)
        {
            var connectionString = $"Host=localhost;Port=5432;Username={user};Password={password};Database=jatetxea";

            try
            {
                // Crea el DataSource
                dataSource = NpgsqlDataSource.Create(connectionString);

                // Opcional: probar una conexión rápida
                await using var conn = await dataSource.OpenConnectionAsync();
                Console.WriteLine("Conexión a PostgreSQL establecida correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
            }
        }

        // erabiltzaileak existitzen badira egiaztatzeko eta admin diren ala ez
        public static async Task<bool> checkErabiltzaileak(string erabiltzaile, string pasahitza)
        {
            if (dataSource == null)
            {
                Console.WriteLine("Ez dago konexiorik sortuta");
                return false;
            }

            const string query = "SELECT COUNT(*) FROM Erabiltzaileak WHERE user = @user AND password = @pass;";

            try
            {
                await using var conn = await dataSource.OpenConnectionAsync();
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", erabiltzaile);
                cmd.Parameters.AddWithValue("@pass", pasahitza);

                var result = (long)await cmd.ExecuteScalarAsync();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorea eabiltzailea bilatzen: {ex.Message}");
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

            const string query = "SELECT tipo FROM Erabiltzaileak WHERE user = @user;";

            try
            {
                await using var conn = await dataSource.OpenConnectionAsync();
                await using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", erabiltzaile);

                var tipo = await cmd.ExecuteScalarAsync();

                if (tipo == null)
                    return false;

                return tipo.ToString()?.Equals("admin", StringComparison.OrdinalIgnoreCase) == true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errorea egon da administratzailea konprobatzeko momentuan: {ex.Message}");
                return false;
            }
        }
    }
}