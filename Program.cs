using System.Collections.Generic;
using System;
using Npgsql;

namespace second_task
{
    class Program
    {
        static void Main(string[] args)
        {
            CRUD crud = new CRUD("Host=127.0.0.1;Username=max;Password=secret;Database=todolist");
            Task upTask = new Task()
            {
                title = "first",
                dueDate = new DateTime(1212, 12, 12)
            };
            Task newTask = new Task()
            {
                title = "new task",
                dueDate = new DateTime(1111,11,11)
            };

            List<Task> tasksForRead = crud.ReadTasks();
            foreach (var task in tasksForRead)
            {
                Console.WriteLine(task);
            }

            List<Task> updatedTasks = crud.UpdateTask(upTask);
            foreach (var task in updatedTasks)
            {
                Console.WriteLine(task);
            }
            crud.DeleteTask(4);
            tasksForRead = crud.CreateTask(newTask);
        }
    }
}
