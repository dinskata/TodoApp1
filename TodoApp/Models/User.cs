public class User
{
    public int Id { get; set; }
    public string ?Username { get; set; }
    public string ?Password { get; set; }
    public string ?FirstName { get; set; }
    public string ?LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }
    public DateTime LastModified { get; set; }
    public int LastModifiedBy { get; set; }
}
