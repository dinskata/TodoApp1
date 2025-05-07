public class TaskAssignment
{
    public int TaskItemId { get; set; }
    public TaskItem? TaskItem { get; set; }
    
    public int UserId { get; set; }
    public User? User { get; set; }
}