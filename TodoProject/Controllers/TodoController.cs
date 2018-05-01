using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using TodoProject.Models;

namespace TodoProject.Controllers
{
    public class TodoController : ApiController
    {
        public todoEntities db = new todoEntities();

        //TodoList todoList = new TodoList();
        List<TodoList> todo = new List<TodoList>();

        [HttpGet]
        public IEnumerable<TodoList> GetList()
        {
            var todoLists = (from list in db.TodoTables
                             select list).ToList();

            foreach (var element in todoLists)
            {
                var title = element.Title;
                var description = element.Description;
                var dueDate = element.DueDate;

                todo.Add(new TodoList(title, description, (DateTime)dueDate));
            }

            return todo;
        }

        [HttpGet]
        public IHttpActionResult GetTodoList(int id)
        {
            try
            {
                var todoLists = (from list in db.TodoTables
                                 where list.Id == id
                                 select list).ToList()[0];

                var title = todoLists.Title;
                var description = todoLists.Description;
                var dueDate = todoLists.DueDate;

                return Json(new { Title = title, Description = description, DueDate = dueDate });
            }
            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }

        }

        [HttpPatch]
        public IHttpActionResult ChangeTodoList(int id, string title, string description, DateTime dueDate)
        {
            try
            {
                var todoLists = (from list in db.TodoTables
                                 where list.Id == id
                                 select list).ToList()[0];
                var newTitle = todoLists.Title;
                var newDescription = todoLists.Description;
                var newDueDate = todoLists.DueDate;

                if (title != "")
                {
                    newTitle = title;
                }
                if (description != "")
                {
                    newDescription = description;
                }
                if (dueDate != null)
                {
                    newDueDate = dueDate;
                }


                var UpdateSql = @"UPDATE [dbo].[TodoTable] SET Title = @Title, Description = @Description, DueDate = @DueDate WHERE Id = @Id";

                db.Database.ExecuteSqlCommand(
                    UpdateSql,
                    new SqlParameter("@Id", id),
                    new SqlParameter("@Title", newTitle),
                    new SqlParameter("@Description", newDescription),
                    new SqlParameter("@DueDate", newDueDate));

                return Json(new { text = "Patch Successful" });
            }

            catch (ArgumentOutOfRangeException)
            {
                return NotFound();
            }
        }

        [System.Web.Http.HttpPost]
        public IHttpActionResult PostList(string title, string description, DateTime dueDate)
        {
            try
            {
                var newEntry = new TodoList(title, description, dueDate);
                todo.Add(newEntry);

                var AddSql = @"INSERT INTO [dbo].[TodoTable] (Title,Description,DueDate) VALUES (@Title,@Description,@DueDate)";

                db.Database.ExecuteSqlCommand(
                    AddSql,
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Description", description),
                    new SqlParameter("@DueDate", dueDate));
                return Json(new { newEntry });

            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
