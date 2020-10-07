using Microsoft.Data.SqlClient;
using System;
using System.ComponentModel.Design.Serialization;
using System.Data;
using System.Linq;

namespace DBminions
{
    class Program
    {
        static string connectionString = "Server=.;Database=Minions;Trusted_Connection=True";

        static void Main(string[] args)
        {
            UpdateMinions();
        }
        //Сделано!
        static void VillainsName()
        {
            string selectionCommandString = $"SELECT v.Name, COUNT(mv.MinionId) FROM MinionsVillains AS mv" +
                " LEFT OUTER JOIN Villains AS v ON mv.VillainId = v.Id" +
                " RIGHT OUTER JOIN Minions AS m ON mv.MinionId = m.Id" +
                " GROUP BY v.Name" +
                " HAVING COUNT(mv.MinionId) >=3" +
                " ORDER BY COUNT(mv.MinionId) DESC";

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectionCommandString, connection);

            connection.Open();
            using (connection)
            {
                SqlDataReader reader = command.ExecuteReader();
                using (reader)
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            Console.Write($"{reader[i]} ");
                        }
                        Console.WriteLine();
                    }
                }
            }
        }

        //Done^_^
        static void MinionsName()
        {
            int id = Convert.ToInt32(Console.ReadLine());

            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();
            using (connection)
            {
                string selectionVillainById = $"SELECT Name FROM Villains WHERE Id = @id";
                SqlCommand command = new SqlCommand(selectionVillainById, connection);
                SqlParameter parameter = new SqlParameter("@id", SqlDbType.Int, 32) { Value = id };
                command.Parameters.Add(parameter);

                object newId = command.ExecuteScalar();
                if (newId == null)
                {
                    Console.WriteLine("В базе данных не существует злодея с идентификатором " + id);
                }
                else
                {
                    SqlDataReader reader1 = command.ExecuteReader();
                    using (reader1)
                    {
                        while (reader1.Read())
                        {
                            for (int i = 0; i < reader1.FieldCount; i++)
                            {
                                Console.Write($"Villain: {reader1[i]} ");
                            }
                            Console.WriteLine();
                        }
                    }
                    string minionsOfTheVillainAreExist = $"SELECT * FROM MinionsVillains WHERE VillainId = @id";
                    SqlCommand command1 = new SqlCommand(minionsOfTheVillainAreExist, connection);
                    SqlParameter parameter1 = new SqlParameter("@id", SqlDbType.Int, 32) { Value = id };
                    command1.Parameters.Add(parameter1);

                    object minionId = command1.ExecuteScalar();
                    if (minionId == null)
                    {
                        Console.WriteLine("У злодея с идентификатором " + id + " нет миньонов");
                    }

                    string findAllMinionsOfTheVillain = $"SELECT Minions.Name as MinionName, Minions.Age as MinionAge" +
                        " FROM MinionsVillains" +
                        " JOIN Villains ON Villains.Id = VillainId" +
                        " JOIN Minions ON Minions.Id = MinionId" +
                        " WHERE VillainId = @id" +
                        " ORDER BY MinionName ASC";
                    SqlCommand command2 = new SqlCommand(findAllMinionsOfTheVillain, connection);
                    SqlParameter parameter2 = new SqlParameter("@id", SqlDbType.Int, 32) { Value = id };
                    command2.Parameters.Add(parameter2);

                    SqlDataReader reader = command2.ExecuteReader();
                    int count = 1;
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"{count++}. {reader["MinionName"]} {reader["MinionAge"]}");
                        }
                    }
                }
            }
        }

        //Done
        static void InsertMinions()
        {
            Console.WriteLine("Enter minion's name, age, town using space");
            string[] minionDataString = Console.ReadLine().Split(' ');
            Console.WriteLine("Enter villain name and evil using space");
            string[] villainDataString = Console.ReadLine().Split(' ');
            string minionName = minionDataString[1];
            int minionAge = Convert.ToInt32(minionDataString[2]);
            string minionTown = minionDataString[3];
            string villainName = villainDataString[1];


            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
               var minionTownId = -1;
               if (!IsTownExist(minionTown))
                {
                    string insertTown = $"INSERT INTO Towns (Name, CountryId) VALUES (@minionTown, 1); SELECT SCOPE_IDENTITY()";
                    SqlCommand command = new SqlCommand(insertTown, connection);
                    command.Parameters.AddWithValue("@minionTown", minionTown);
                    minionTownId = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Город {minionTown} был добавлен в бд. Его id = {minionTownId}");
                }
                else
                {   
                    string selectTownId = "SELECT Id FROM Towns WHERE Name=@name";
                    SqlCommand command = new SqlCommand(selectTownId, connection);
                    command.Parameters.AddWithValue("@name", minionTown);
                    minionTownId = Convert.ToInt32(command.ExecuteScalar());
                }
                var villainId = -1;
                if (!IsVillainExist(villainName))
                {
                    string insertVillain = $"INSERT INTO Villains (Name, EvilnessFactorId) VALUES (@villainName, 3); SELECT SCOPE_IDENTITY()";
                    SqlCommand command = new SqlCommand(insertVillain, connection);
                    command.Parameters.AddWithValue("@villainName", villainName);
                    villainId = Convert.ToInt32(command.ExecuteScalar());
                    Console.WriteLine($"Злодей {villainName} был добавлен в бд. Его id = {villainId}");
                }
                else
                {
                    string selectVillainId = "SELECT Id FROM Towns WHERE Name=@name";
                    SqlCommand command = new SqlCommand(selectVillainId, connection);
                    command.Parameters.AddWithValue("@name", villainName);
                    villainId = Convert.ToInt32(command.ExecuteScalar());
                }

                string insertMinion = $"INSERT INTO Minions (Name, Age, TownId) VALUES (@minionName, @minionAge, @minionTownId); SELECT SCOPE_IDENTITY()";
                SqlCommand command1 = new SqlCommand(insertMinion, connection);
                command1.Parameters.AddWithValue("@minionName", minionName);
                command1.Parameters.AddWithValue("@minionAge", minionAge);
                command1.Parameters.AddWithValue("@minionTownId", minionTownId);

                int minionId = Convert.ToInt32(command1.ExecuteScalar());

                string insertMinionsVillains = $"INSERT INTO MinionsVillains (MinionId, VillainId) VALUES " +
                                                "(@minionId, @villainId); SELECT SCOPE_IDENTITY()";
                SqlCommand command2 = new SqlCommand(insertMinion, connection);
                command2.Parameters.AddWithValue("@minionId", minionId);
                command2.Parameters.AddWithValue("@villainId", villainId);

                Console.WriteLine($"Успешно добавлен {minionName} чтобы быть миньоном {villainName}");
            }
        }

        static private bool IsTownExist(string name)
        {
            string selectionTown = $"SELECT COUNT(*) FROM Towns WHERE Name=@name";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectionTown, connection);
            command.Parameters.AddWithValue("@name", name);

            connection.Open();
            {
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }

        static private bool IsVillainExist(string name)
        {
            string selectinVillain = $"SELECT COUNT(*) FROM Villains WHERE Name=@name";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectinVillain, connection);
            command.Parameters.AddWithValue("@name", name);

            connection.Open();
            {
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }
        static private bool IsVillainExist(int id)
        {
            string selectinVillain = $"SELECT COUNT(*) FROM Villains WHERE Id=@id";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(selectinVillain, connection);
            command.Parameters.AddWithValue("@id", id);

            connection.Open();
            {
                var result = (int)command.ExecuteScalar();
                return result > 0;
            }
        }
        static void DeleteMinions()
        {
            int villainId = Convert.ToInt32(Console.ReadLine());

            if (!IsVillainExist(villainId))
            {
                Console.WriteLine("Такой злодей не найден");
            }
            else
            {
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                {
                    string selectVillainName = $"SELECT Name FROM Villains WHERE Id = @id";
                    SqlCommand command = new SqlCommand(selectVillainName, connection);
                    command.Parameters.AddWithValue("@id", villainId);
                    string villainName = (string)command.ExecuteScalar();

                    string deleteMinionsVillains = $"DELETE FROM MinionsVillains WHERE VillainId = @villainId";
                    SqlCommand command1 = new SqlCommand(deleteMinionsVillains, connection);
                    command1.Parameters.AddWithValue("@villainId", villainId);
                    int numberOfMinions = (int)command1.ExecuteNonQuery();

                    string deleteVillain = $"DELETE FROM Villains WHERE Id = @villainId";
                    SqlCommand command2 = new SqlCommand(deleteVillain, connection);
                    command2.Parameters.AddWithValue("@villainId", villainId);

                    Console.WriteLine($"{villainName} был удалён.\n{numberOfMinions} миньонов было освобождено.");
                }
            }

        }
        static void UpdateMinions()
        {
            var ids = Console.ReadLine().Split(' ').Select(int.Parse);

            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                string updateMinions = $"UPDATE Minions SET Age=Age+1 WHERE Id IN ({string.Join(", ", ids)})";
                SqlCommand command = new SqlCommand(updateMinions, connection);

                int updated = Convert.ToInt32(command.ExecuteNonQuery());

                Console.WriteLine($"{updated} строк обновлено.");

                string selectMinions = $"SELECT Minions.Name as MinionName, Minions.Age as MinionAge FROM Minions";
                SqlCommand command1 = new SqlCommand(selectMinions, connection);
                SqlDataReader reader = command1.ExecuteReader();
                int count = 1;
                using (reader)
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"{count++}. {reader["MinionName"]} {reader["MinionAge"]}");
                    }
                }
            }
        }
    }
}