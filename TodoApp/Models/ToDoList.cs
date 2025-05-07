public class ToDoList
{
    public int Id { get; set; }
    public string ?Title { get; set; }

    // Required foreign key relationships
    public DateTime CreatedAt { get; set; }
    public int CreatedById { get; set; }
    public User ?CreatedBy { get; set; }

    public DateTime LastModifiedAt { get; set; }
    public int LastModifiedById { get; set; }
    public User ?LastModifiedBy { get; set; }

    public ICollection<TaskItem> ?Tasks { get; set; }
}