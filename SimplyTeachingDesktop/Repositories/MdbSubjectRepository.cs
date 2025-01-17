﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplyTeachingDesktop.Repositories
{
    internal class MdbSubjectRepository : Repository
    {
        private readonly string connectionString = EnvironmentVars.UrlConnection;
        private MySqlConnection connection = null;
        private MySqlCommand command = null;
        private MySqlDataReader reader = null;

        string query = null;
        public bool Delete(Entity entity)
        {
            SubjectModel model = entity as SubjectModel;
            bool result = false;

            connection = new MySqlConnection(connectionString);
            query = "DELETE FROM subjects WHERE id = @id;";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", model.id);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                result = true;
            }
            catch (MySqlException e) { Console.WriteLine(e.Message); }
            catch (Exception e) { Console.WriteLine(e.Message); }
            finally
            {
                if (connection != null) { connection.Close(); }
            }
            return result;
        }

        public Entity Find(int id)
        {
            SubjectModel entity = null;

            connection = new MySqlConnection(connectionString);
            query = "SELECT * FROM subjects WHERE id = @id;";
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    entity = new SubjectModel();
                    while (reader.Read())
                    {
                        entity.id = int.Parse(reader.GetString(0));
                        entity.name = reader.GetString(1);
                        entity.hour = reader.GetString(2);
                        entity.day = int.Parse(reader.GetString(3));
                        entity.price = double.Parse(reader.GetString(4));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MdbTeacherRepository\n" + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }


            return entity;
        }

        public List<Entity> FindAll()
        {
            List<Entity> list = new List<Entity>();
            SubjectModel entity;

            connection = new MySqlConnection(connectionString);
            query = "SELECT * FROM subjects";
            command = connection.CreateCommand();
            command.CommandText = query;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        entity = new SubjectModel();
                        entity.id = int.Parse(reader.GetString(0));
                        entity.name = reader.GetString(1);
                        entity.hour = reader.GetString(2);
                        entity.day = int.Parse(reader.GetString(3));
                        entity.price = double.Parse(reader.GetString(4));
                        
                        list.Add(entity);
                        entity = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("MdbTeacherRepository\n" + ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }


            return list;
        }

        public bool Save(Entity entity)
        {
            SubjectModel model = entity as SubjectModel;
            bool result = false;
            SubjectModel findModel = Find(model.id) as SubjectModel;
            if (findModel == null) query = "INSERT INTO subjects VALUES (@id, @name, @hour, @day, @price);";
            else query = "UPDATE subjects SET name = @name, hour = @hour, day = @day, price = @price WHERE id = @id;";
            connection = new MySqlConnection(connectionString);
            command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", model.id);
            command.Parameters.AddWithValue("@name", model.name);
            command.Parameters.AddWithValue("@hour", model.hour);
            command.Parameters.AddWithValue("@day", model.day);
            command.Parameters.AddWithValue("@price", model.price);

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                result = true;
            }
            catch (MySqlException ex) { Console.WriteLine(ex.StackTrace); }
            catch (Exception e) { Console.WriteLine(e.StackTrace); }
            finally
            {
                if (connection != null)
                    connection.Close();
            }

            return result;
        }

        public bool TestConnection()
        {
            bool result = false;

            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();
            }
            catch (AggregateException ex) { Console.WriteLine(ex.StackTrace); }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
            finally {
                if (connection != null)
                    connection.Close();
            }

            return result;
        }
    }
}
