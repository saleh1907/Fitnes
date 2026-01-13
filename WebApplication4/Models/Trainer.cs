namespace WebApplication4.Models;

public class Trainer
{
    public int Id { get; set; }
    public string Name { get; set; }=string.Empty;
    public string Description { get; set; }=string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public int DepartamenId { get; set; }
    public Departament Departament { get; set; } = null!;
}
