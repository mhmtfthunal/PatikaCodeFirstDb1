namespace PatikaCodeFirstDb1.Models;

public class Movie
{
    public int Id { get; set; }                 // PK, Identity
    public string Title { get; set; } = null!;  // Başlık
    public string Genre { get; set; } = null!;  // Tür
    public int ReleaseYear { get; set; }        // Çıkış yılı
}
