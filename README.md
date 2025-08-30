# Pratik — Code First Basic

Entity Framework Core ile **Code First** yaklaşımında iki bağımsız tablo (**Movies** ve **Games**) oluşturan örnek proje.

* **Context sınıfı:** `PatikaFirstDbContext`
* **Veritabanı adı:** `PatikaCodeFirstDb1`
* **Tablolar:** `Movies`, `Games`
* **Hedef framework:** .NET 8
* **Veritabanı sunucusu (varsayılan):** SQL Server **LocalDB** (VS ile gelir)

---

## İçindekiler

1. [Gereksinimler](#gereksinimler)
2. [Proje Yapısı](#proje-yapısı)
3. [Kurulum (VS ile)](#kurulum-vs-ile)
4. [Kurulum (CLI ile)](#kurulum-cli-ile)
5. [Bağımlılıklar / NuGet Paketleri](#bağımlılıklar--nuget-paketleri)
6. [Bağlantı Dizesi (Connection String)](#bağlantı-dizesi-connection-string)
7. [Migration & Veritabanı Oluşturma](#migration--veritabanı-oluşturma)
8. [Çalıştırma](#çalıştırma)
9. [Doğrulama (Tabloları Görme)](#doğrulama-tabloları-görme)
10. [Model & Şema Notları](#model--şema-notları)
11. [Örnek CRUD Kodları](#örnek-crud-kodları)
12. [Sık Karşılaşılan Sorunlar](#sık-karşılaşılan-sorunlar)
13. [Yararlı Komutlar](#yararlı-komutlar)

---

## Gereksinimler

* **Visual Studio 2022** (veya Rider/VS Code)
* **.NET SDK 8.x**
* **SQL Server LocalDB** *(VS Installer > Individual components > SQL Server Express LocalDB)* ya da alternatif olarak **SQL Server Express/Full**

---

## Proje Yapısı

```
PatikaCodeFirstDb1/
├─ PatikaCodeFirstDb1.csproj
├─ Program.cs
├─ Data/
│  └─ PatikaFirstDbContext.cs
└─ Models/
   ├─ Movie.cs
   └─ Game.cs
```

---

## Kurulum (VS ile)

1. **File > New > Project** → **Console App (.NET)**
2. **Project name:** `PatikaCodeFirstDb1` → **Create**
3. Framework: **.NET 8.0**
4. `Models` ve `Data` klasörlerini oluştur.
5. Aşağıdaki dosyaları ekle (aşağıdaki örnek kodlar bölümünde var):

   * `Models/Movie.cs`
   * `Models/Game.cs`
   * `Data/PatikaFirstDbContext.cs`
   * `Program.cs` (varsayılanı değiştir)
6. NuGet paketlerini yükle (Bkz. [Bağımlılıklar](#bağımlılıklar--nuget-paketleri)).
7. **Package Manager Console**: `Add-Migration InitialCreate` → `Update-Database`

---

## Kurulum (CLI ile)

```bash
# Yeni proje
dotnet new console -n PatikaCodeFirstDb1
cd PatikaCodeFirstDb1

# Paketler
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design

# Migration ve DB
dotnet ef migrations add InitialCreate
dotnet ef database update

# Çalıştırma
dotnet run
```

> `dotnet ef` komutu yoksa: `dotnet tool install --global dotnet-ef`

---

## Bağımlılıklar / NuGet Paketleri

* `Microsoft.EntityFrameworkCore`
* `Microsoft.EntityFrameworkCore.SqlServer`
* `Microsoft.EntityFrameworkCore.Tools`
* `Microsoft.EntityFrameworkCore.Design`

Tüm paketleri **aynı ana sürümde (8.x)** tutun.

---

## Bağlantı Dizesi (Connection String)

**Varsayılan (LocalDB):**

```csharp
Server=(localdb)\MSSQLLocalDB;Database=PatikaCodeFirstDb1;Trusted_Connection=True;TrustServerCertificate=True;
```

**SQL Express için alternatif:**

```csharp
Server=.\SQLEXPRESS;Database=PatikaCodeFirstDb1;Trusted_Connection=True;TrustServerCertificate=True;
```

Bağlantı dizesi `Data/PatikaFirstDbContext.cs` içinde `OnConfiguring` metodunda yapılandırılır.

---

## Migration & Veritabanı Oluşturma

**Visual Studio / PMC ile:**

```powershell
Add-Migration InitialCreate
Update-Database
```

**CLI ile:**

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Başarılıysa, `PatikaCodeFirstDb1` DB’si oluşturulur ve `Movies`, `Games` tabloları yaratılır.

---

## Çalıştırma

* VS’de **F5** veya **Ctrl+F5**
* CLI: `dotnet run`

`Program.cs` ilk çalıştırmada örnek veriler ekler ve konsola durum bilgisi yazar.

---

## Doğrulama (Tabloları Görme)

**Visual Studio:** `View > SQL Server Object Explorer` → `(localdb)\MSSQLLocalDB > Databases > PatikaCodeFirstDb1 > Tables`

`dbo.Movies` ve `dbo.Games` görünüyor olmalı.

---

## Model & Şema Notları

**Movie**

* `Id` (PK, Identity)
* `Title` (zorunlu, max 200)
* `Genre` (zorunlu, max 50)
* `ReleaseYear` (zorunlu)

**Game**

* `Id` (PK, Identity)
* `Name` (zorunlu, max 150)
* `Platform` (zorunlu, max 50)
* `Rating` (zorunlu, **precision (3,1)**; örn. 9.5)
* **Check constraint:** `0 <= Rating <= 10`

Tablo adları `ToTable("Movies")` ve `ToTable("Games")` ile sabitlenmiştir.

---

## Örnek CRUD Kodları

> Uygulamanın `Program.cs` dosyasında başlangıç verisi ekleme örneği vardır. Aşağıdaki basit CRUD parçacıkları ekstra örnek olarak verilmiştir.

**Ekleme**

```csharp
using var db = new PatikaFirstDbContext();

db.Movies.Add(new Movie { Title = "Inception", Genre = "Action", ReleaseYear = 2010 });
db.Games.Add(new Game { Name = "Forza Horizon 5", Platform = "Xbox", Rating = 9.2m });

db.SaveChanges();
```

**Listeleme / Sorgu**

```csharp
var actionMovies = db.Movies
    .Where(m => m.Genre == "Action" && m.ReleaseYear >= 2000)
    .OrderByDescending(m => m.ReleaseYear)
    .ToList();
```

**Güncelleme**

```csharp
var game = db.Games.FirstOrDefault(g => g.Name == "The Witcher 3");
if (game is not null)
{
    game.Rating = 9.7m;
    db.SaveChanges();
}
```

**Silme**

```csharp
var movie = db.Movies.FirstOrDefault(m => m.Title == "Toy Story");
if (movie is not null)
{
    db.Movies.Remove(movie);
    db.SaveChanges();
}
```

---

## Sık Karşılaşılan Sorunlar

* **LocalDB bulunamadı / bağlanmıyor:** VS Installer → *Individual components* → **SQL Server Express LocalDB**’yi kurun.
* **`Add-Migration` hatası:** Proje seçimi PMC’de yanlış olabilir (Default project açılır listesini kontrol edin). `Microsoft.EntityFrameworkCore.Tools` paketinin yüklü olduğundan emin olun. Projeyi **Build** edin.
* **`dotnet ef` komutu yok:** `dotnet tool install --global dotnet-ef` (sonra terminali yeniden açın).
* **SQL izin/sertifika uyarıları:** Connection string’e `TrustServerCertificate=True` eklidir; kurumsal SQL’de gerekirse güvenilir sertifika kullanın.
* **Sürüm uyumsuzluğu:** Tüm EF paketlerini **aynı ana sürümde** (8.x) tutun.

---

## Yararlı Komutlar

```powershell
# Son migration'ı geri al
Remove-Migration

# Veritabanını düşür (sil) ve yeniden kur
Drop-Database
Update-Database

# Tüm migration'ları listele
Get-Migration
```

---


