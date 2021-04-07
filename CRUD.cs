using System.Globalization;
using System.Collections.Generic;
using System;
using Npgsql;


namespace second_task
{
    internal class CRUD
    {
        private NpgsqlConnection conn;

        public CRUD(string connString)
        {
            conn = new NpgsqlConnection(connString);
            conn.Open();
        }
        public List<Task> CreateTask(Task task)
        {
            using (var command = new NpgsqlCommand("insert into tasks(title,description,done,due_date) values (@title, @description, @done, @due_date)", conn))
            {
                command.Parameters.AddWithValue("title", task.title);
                command.Parameters.AddWithValue("description", task.desc ?? "");
                command.Parameters.AddWithValue("done", task.done.Equals(null) ? false : true);
                command.Parameters.AddWithValue("due_date", task.dueDate);
                command.ExecuteNonQuery();
            }
            return this.ReadTasks();
        }
        public List<Task> ReadTasks()
        {
            List<Task> allTasks = new List<Task>();
            using (var command = new NpgsqlCommand("select * from tasks", conn))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Task readTask = new Task()
                        {
                            id = reader.GetInt32(0),
                            title = reader.GetString(1),
                            desc = reader.IsDBNull(2) ? null : reader.GetString(2),
                            done = reader.GetBoolean(3),
                            dueDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                        };
                        allTasks.Add(readTask);
                    }
                }
            }
            return allTasks;
        }
        public List<Task> UpdateTask(Task upTask)
        {
            var command = new NpgsqlCommand("update tasks set description = @description, done = @done,due_date = @due_date where title = @title", conn);
            command.Parameters.AddWithValue("title", upTask.title);
            command.Parameters.AddWithValue("description", upTask.desc ?? "");
            command.Parameters.AddWithValue("done", upTask.done);
            command.Parameters.AddWithValue("due_date", upTask.dueDate);
            command.ExecuteNonQuery();
            return this.ReadTasks();
        }
        public void DeleteTask(int id)
        {
            var command = new NpgsqlCommand("delete from tasks where id =@id", conn);
            command.Parameters.AddWithValue("id", id);
            command.ExecuteNonQuery();
        }

    }
}