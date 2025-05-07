using System;
using System.Collections.Generic;

public class TaskItem
{
    public int Id { get; set; }
    public int ToDoListId { get; set; }

    public string ?Title { get; set; }
    public string ?Description { get; set; }
    public bool IsComplete { get; set; }

    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }

    public DateTime LastModifiedAt { get; set; }
    public int LastModifiedById { get; set; }

    // Връзки
    public ToDoList ?ToDoList { get; set; }
    public User ?CreatedBy { get; set; }
    public User ?LastModifiedBy { get; set; }

    public ICollection<User> ?AssignedUsers { get; set; }
}
