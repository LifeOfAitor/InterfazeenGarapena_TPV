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
        //datu baseko erabiltzaile normalak kargatzen ditu lista batean
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

        //erabiltzaile berri bat sortzen du datu basean
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

        //ezarritako izena duen erabiltzailea ezabatzen du datu basetik
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

        //ezarritako izena duen erabiltzailearen pasahitza aldatzen du
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

        //biltegiko produktuak kargatzen ditu array batean
        public static List<Produktua> kargatuBiltegia()
        {
            var biltegia = new List<Produktua>();

            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return biltegia;
            }

            const string query = "SELECT izena, stock FROM produktuak;";
            try
            {
                using var conn = dataSource.OpenConnection();
                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string izena = reader.GetString(0);
                    int stock = reader.GetInt32(1); // stock como entero
                    biltegia.Add(new Produktua(izena, stock));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea biltegia kargatzerakoan: {ex.Message}");
            }

            return biltegia;
        }
        //kargatu kategoriak
        public static List<string> kargatuKategoriak()
        {
            List<string> kategoriak = new List<string>();
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return kategoriak;
            }
            const string query = "SELECT izena FROM Kategoriak;";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    kategoriak.Add(reader.GetString(0));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea kategoriak kargatzerakoan: {ex.Message}");
            }
            return kategoriak;
        }

        //ezabatu produktua
        public static void ezabatuProduktua(string izena)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return;
            }
            const string query = "DELETE FROM Produktuak WHERE izena = @izena;";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@izena", izena);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea produktua ezabatzekoan: {ex.Message}");
            }
        }

        //hautatutako produktuaren stocka aldatzeko
        public static void aldatuStock(string produktua, int stockBerria)
        {
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return;
            }
            const string query = "UPDATE Produktuak SET stock = @stockBarria WHERE izena = @produktua;";
            try
            {
                using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@produktua", produktua);
                cmd.Parameters.AddWithValue("@stockBarria", stockBerria);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea stocka aldatzerakoan: {ex.Message}");
            }
        }

        //bazkari edo afarirako erreserbak kargatzea
        public static List<Erreserba> kargatuErreserbak(string noizin, DateTime? erreserbadatain)
        {
            List<Erreserba> erreserbak = new List<Erreserba>();
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
                return erreserbak;
            }

            if (erreserbadatain == null)
            {
                MessageBox.Show("Data ez da zuzena");
                return erreserbak;
            }

            const string query = "SELECT mahaizenbakia, data, noiz FROM erreserbak WHERE data = @erreserbadata AND noiz = @noiz::erreserbanoiz";
            try
            {
                using var conn = dataSource.OpenConnection();
                using var cmd = new NpgsqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@erreserbadata", erreserbadatain.Value.Date);
                cmd.Parameters.AddWithValue("@noiz", noizin);

                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int mahaizenbakia = reader.GetInt32(0);
                    DateTime erreserbadata = reader.GetDateTime(1);
                    string noiz = reader.GetString(2);
                    erreserbak.Add(new Erreserba(mahaizenbakia, erreserbadata, noiz));
                }

                return erreserbak;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Errorea erreserbak kargatzerakoan: {ex.Message}");
                return erreserbak;
            }
        }

        public static void erreserbatuMahaiak(string aukeratutakoMahaiak, string erabiltzailea, DateTime? erreserbadata, string noiz)
        {
            List<string> mahaiak = aukeratutakoMahaiak.Split(' ').ToList();
            if (dataSource == null)
            {
                MessageBox.Show("Ez dago konexiorik sortuta");
            }
            if (erreserbadata == null)
            {
                MessageBox.Show("Data ez da zuzena");
                return;
            }
            foreach (string s in mahaiak)
            {
                const string query = "INSERT INTO erreserbak (mahaizenbakia, data, noiz, erabiltzailea) VALUES (@s, @erreserbadata, @noiz::erreserbanoiz, @erabiltzailea)";
                try
                {
                    using var conn = dataSource.OpenConnection(); // datu basera konektatzen da
                    using var cmd = new NpgsqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@s", Int32.Parse(s));
                    cmd.Parameters.AddWithValue("@erreserbadata", erreserbadata.Value.Date);
                    cmd.Parameters.AddWithValue("@noiz", noiz);
                    cmd.Parameters.AddWithValue("@erabiltzailea", erabiltzailea);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Errorea erreserba sartzerakoan: {ex.Message}");
                }
            }
            

        }

    }
}