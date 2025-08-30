using Microsoft.EntityFrameworkCore;
using PatikaCodeFirstDb1.Data;
using PatikaCodeFirstDb1.Models;

Console.WriteLine("EF Core Code-First başlıyor...");

using var db = new PatikaFirstDbContext();

// Migration varsa uygula; yoksa oluşturduktan sonra Update-Database ile gelecek
db.Database.Migrate();

// (Opsiyonel) örnek veri
if (!db.Movies.Any() && !db.Games.Any())
{
    db.Movies.AddRange(
        new Movie { Title = "The Matrix", Genre = "Action", ReleaseYear = 1999 },
        new Movie { Title = "Toy Story", Genre = "Comedy", ReleaseYear = 1995 }
    );
    db.Games.AddRange(
        new Game { Name = "The Witcher 3", Platform = "PC", Rating = 9.5m },
        new Game { Name = "God of War", Platform = "PlayStation", Rating = 9.0m }
    );
    db.SaveChanges();
    Console.WriteLine("Örnek veriler eklendi.");
}

Console.WriteLine("Bitti. DB: PatikaCodeFirstDb1 | Tables: Movies, Games");
