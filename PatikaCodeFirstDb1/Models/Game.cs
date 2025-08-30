namespace PatikaCodeFirstDb1.Models;

public class Game
{
    public int Id { get; set; }                   // PK, Identity
    public string Name { get; set; } = null!;     // Oyun adı
    public string Platform { get; set; } = null!; // PC, PS, Xbox
    public decimal Rating { get; set; }           // 0–10
}
