namespace WebApplication4.Models;

public class Departament
{
    public int Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public ICollection<Trainer> Trainers { get; set; } = [];
}
