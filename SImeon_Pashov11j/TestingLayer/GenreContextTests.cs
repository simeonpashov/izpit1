using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using BussinesLayer;
using Org.BouncyCastle.Asn1.Cmp;
using NUnit.Framework;

namespace TestingLayer
{
    public class GenreContextTests
    {
        static GenreContext genresContext;
        static GenreContextTests() { genresContext = new GenreContext(TestManager.dbContext); }

        [Test]
        public void CreateGenre()
        {
            
            Genre genre = new Genre(1, "shooter");
            int genresBefore = TestManager.dbContext.Genres.Count();

            // Act
            genresContext.Create(genre);

            // Assert
            int genresAfter = TestManager.dbContext.Genres.Count();
            Genre lastGenre = TestManager.dbContext.Genres.Last();
            Assert.That(genresBefore + 1 == genresAfter && lastGenre.Name == genre.Name,
                "Names are not equal or genre is not created!");
        }
        [Test]
        public void ReadGenre()
        {
            Genre newGenre = new Genre(1, "shooter");
            genresContext.Create(newGenre);

            Genre genre = genresContext.Read(1);

            Assert.That(genre.Name == "shooter", "Read() does not get Genre by id!");
        }
        [Test]
        public void ReadAllGenres()
        {
            int genresBefore = TestManager.dbContext.Genres.Count();

            int genresAfter = genresContext.ReadAll().Count;

            Assert.That(genresBefore == genresAfter, "ReadAll() does not return all of the Genres!");
        }
        [Test]
        public void UpdateGenre()
        {
            Genre newGenre = new Genre(1,"shooter");
            genresContext.Create(newGenre);

            Genre lastGenre = genresContext.ReadAll().Last(); 
            lastGenre.Name = "Updated Genre";

            genresContext.Update(lastGenre, false);

            Assert.That(genresContext.Read(lastGenre.GenreId).Name == "Updated Genre",
            "Update() does not change the Genre's name!");
        }
        [Test]
        public void DeleteGenre()
        {
            Genre newGenre = new Genre(1,"shooter");
            genresContext.Create(newGenre);

            List<Genre> genres = genresContext.ReadAll();
            int genresBefore = genres.Count;
            Genre genre = genres.Last();

            genresContext.Delete(genre.GenreId);

            int genresAfter = genresContext.ReadAll().Count;
            Assert.That(genresBefore == genresAfter + 1, "Delete() does not delete a genre!");
        }
    }
}
