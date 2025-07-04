using MovieHub.Engine.Storage.Entities;
using MovieHub.Engine.Storage.Common;

namespace MovieHub.Engine.Storage.Tests.GetPersonStorageTests;

public class GetPersonStorageTestFixture : StorageTestFixture
{
    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        await using MovieHubDbContext dbContext = GetMovieHubDbContext();

        await dbContext.Persons.InsertManyAsync(
        [
            new PersonEntity
            {
                Id = Guid.Parse("819FA47A-8F47-4CD2-8CB3-1A45537BADE1"),
                FullName = "Leonardo DiCaprio",
                BirthDate = new DateOnly(1974, 11, 11),
                Biography = "American actor and film producer",
                PhotoUrl = "https://example.com/dicaprio.jpg",
                Professions = [ProfessionType.Actor],
                MediaIds = [Guid.Parse("C2364431-9B79-4447-93DB-A5A6BB556C20")]
            },
            new PersonEntity
            {
                Id = Guid.Parse("1D4224A8-BD23-4D0B-8FB4-D17D3E2C7364"),
                FullName = "Margot Robbie",
                BirthDate = new DateOnly(1990, 7, 2),
                Biography = "Australian actress and producer",
                PhotoUrl = "https://example.com/robbie.jpg",
                Professions = [ProfessionType.Actor, ProfessionType.Producer],
                MediaIds = [Guid.Parse("951191AB-84E8-429C-B19E-C4B368DB4DF2")]
            },
            new PersonEntity
            {
                Id = Guid.Parse("5F6DC77D-C0AB-4B89-BFC9-09AA28D1A4B4"),
                FullName = "Christopher Nolan",
                BirthDate = new DateOnly(1970, 7, 30),
                Biography = "British-American film director",
                PhotoUrl = "https://example.com/nolan.jpg",
                Professions = [ProfessionType.Director],
                MediaIds = [Guid.Parse("B5306FDD-9C45-44B5-B4FC-02215958FD8B")]
            },
            new PersonEntity
            {
                Id = Guid.Parse("FF530997-BA0C-4012-9E58-37ACCE733BB3"),
                FullName = "Quentin Tarantino",
                BirthDate = new DateOnly(1963, 3, 27),
                Biography = "American film director and screenwriter",
                PhotoUrl = "https://example.com/tarantino.jpg",
                Professions = [ProfessionType.Director],
                MediaIds = [Guid.Parse("2D090764-CDEF-4117-9E71-1B974404D3C5")]
            },
            new PersonEntity
            {
                Id = Guid.Parse("1A889BBE-E47D-4473-8663-365759E2C5C3"),
                FullName = "Scarlett Johansson",
                BirthDate = new DateOnly(1984, 11, 22),
                Biography = "American actress",
                PhotoUrl = "https://example.com/johansson.jpg",
                Professions = [ProfessionType.Actor],
                MediaIds = [Guid.Parse("6D93FABB-D520-4B49-99D4-3D2D3E26188D")]
            }
        ]);
    }
}