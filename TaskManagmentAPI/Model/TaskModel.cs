using System.ComponentModel.DataAnnotations;
using TaskManagmentAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TaskManagmentAPI.Model
{
    public class TaskModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is required")]

        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; } = DateTime.UtcNow;
        public bool IsCompleted { get; set; } = false;

    }
}



