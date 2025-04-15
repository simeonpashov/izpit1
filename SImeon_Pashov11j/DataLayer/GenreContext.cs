using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class GenreContext : IDb<Genre, int>
    {
        private GameDbContext dbContext;

        public GenreContext(GameDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(Genre item)
        {
            List<Game> games = new List<Game>(item.Games.Count);
            foreach (Game game in item.Games)
            {
                Game gameFromDb = dbContext.Games.Find(game.GameId);
                if (gameFromDb != null) games.Add(gameFromDb);
                else games.Add(game); 
            }

            item.Games = games;

            dbContext.Genres.Add(item);
            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Genre genreFromDb = Read(key);
            dbContext.Genres.Remove(genreFromDb);
            dbContext.SaveChanges();
        }

        public Genre Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Genre> query = dbContext.Genres;

            if (useNavigationalProperties)
                query = query.Include(g => g.Games);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            Genre genre = query.FirstOrDefault(g => g.GenreId == key);

            if (genre == null)
                throw new ArgumentException($"Genre with ID = {key} does not exist!");

            return genre;
        }

        public List<Genre> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Genre> query = dbContext.Genres;

            if (useNavigationalProperties)
                query = query.Include(g => g.Games);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public void Update(Genre item, bool useNavigationalProperties = false)
        {
            

            List<Game> games = new List<Game>(item.Games.Count);
            foreach (Game game in item.Games)
            {
                Game gameFromDb = dbContext.Games.Find(game.GameId);
                if (gameFromDb != null) games.Add(gameFromDb);
                else games.Add(game);
            }

            item.Games = games;

            dbContext.Genres.Update(item);
            dbContext.SaveChanges();
        }
    }
}
