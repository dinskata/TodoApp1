public class ListShare
{
    public int Id { get; set; }
    
    // Foreign key to ToDoList
    public int ToDoListId { get; set; }
    public ToDoList ?ToDoList { get; set; }
    
    // Foreign key to User
    public int UserId { get; set; }
    public User ?User { get; set; }
    
    public DateTime SharedAt { get; set; } = DateTime.Now;
}