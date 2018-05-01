using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TodoProject.Models
{
    public class TodoList
    {
        public TodoList(string title, string description, DateTime dueDate)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}