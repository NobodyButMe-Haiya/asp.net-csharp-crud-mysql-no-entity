using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
// so we want to connect mysql so nuget
// so the next video will upon linking with the web api ..
// now continue back to make web api
// now the api been handle ? what with web ? and mobile ? next video 
namespace crudInMacos
{
    // the next stage kinda hard is how does we get information from those app setting ?
  
    public class PersonRepository
    {
        private string _connectionString { get; set; }
        public PersonRepository()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true,reloadOnChange:true)
                .AddEnvironmentVariables()
                .Build();
            _connectionString = configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
        }
        public void Create(String name, int age)
        {
            // okay next we create skeleton for the code


            MySqlTransaction mySqlTransaction = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {

                    connection.Open();

                    mySqlTransaction = connection.BeginTransaction();

                    string SQL = "INSERT INTO person VALUES (null,@name,@age);";
                    MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);
                    // we add some parameter
                    mySqlCommand.Parameters.AddWithValue("@name", name);
                    mySqlCommand.Parameters.AddWithValue("@age", age);
                    mySqlCommand.ExecuteNonQuery();
                    // we have error here .. 
                    mySqlTransaction.Commit();
                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch(MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }


          

        }
        public List<PersonModel> Read()
        {
            List<PersonModel> personModels = new();

            // for reading we don't need transaction ya .. 


            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {

                    connection.Open();
                    string SQL = "SELECT * FROM person ";
                    MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);
                    using (var reader = mySqlCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // it all depend , for ease we just send all data as string it all depend on web api to translate to "" or number only 
                            personModels.Add(new PersonModel()
                            {
                                
                                Name = reader["name"].ToString(),
                                Age = Convert.ToInt32(reader["age"]),
                                PersonId = Convert.ToInt32(reader["personId"])
                            });
                        }
                    }


                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }


            return personModels;
        }
        public void Update(String name, int age, int personId)
        {

            MySqlTransaction mySqlTransaction = null;
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {

                    connection.Open();
                    mySqlTransaction = connection.BeginTransaction();

                    string SQL = "UPDATE person SET name = @name, age = @age WHERE personId = @personId ";
                    MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);
                    // we add some parameter
                    mySqlCommand.Parameters.AddWithValue("@name", name);
                    mySqlCommand.Parameters.AddWithValue("@age", age);
                    mySqlCommand.Parameters.AddWithValue("@personId", personId);

                    mySqlCommand.ExecuteNonQuery();

                    mySqlTransaction.Commit();
                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }
        }
        public void Delete(int personId)
        {


            MySqlTransaction mySqlTransaction = null;
            // the old using is using(xxx) { }  now it's weirder 
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {

                    connection.Open();
                    // sorry we tend to forget sometimes..  but with error like this you all can remember what's error if failure not all working  fine :P
                    mySqlTransaction = connection.BeginTransaction();

                    string SQL = "DELETE FROM person WHERE personId  = @personId;";
                    MySqlCommand mySqlCommand = new MySqlCommand(SQL, connection);
                    // we add some parameter
                    mySqlCommand.Parameters.AddWithValue("@personId", personId);
                    mySqlCommand.ExecuteNonQuery();

                    mySqlTransaction.Commit();
                    // c;ear some memory 
                    mySqlCommand.Dispose();

                }
                catch (MySqlException ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    /// you may throw new exception here or use any log debugger to text file 
                }
            }
        }
    }
  
}
