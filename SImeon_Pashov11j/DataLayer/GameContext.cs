using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using BussinesLayer;

namespace DataLayer
{
    public class GameContext : IDb<Game, int>
    {
        private GameDbContext dbContext;

        public void Create(Game item)
        {
            
            List<Genre> genres = new List<Genre>(item.Genres.Count);
            foreach (Genre genre in item.Genres)
            {
                Genre genreFromDb = dbContext.Genres.Find(genre.GenreId);
                if (genreFromDb != null) genres.Add(genreFromDb);
                else genres.Add(genre);
            }

            item.Genres = genres;

          
            List<Potrebitel> users = new List<Potrebitel>(item.Users.Count);
            foreach (Potrebitel user in item.Users)
            {
                Potrebitel userFromDb = dbContext.Potrebiteli.Find(user.UserId);
                if (userFromDb != null) users.Add(userFromDb);
                else users.Add(user);
            }

            item.Users = users;

            dbContext.Games.Add(item);
            dbContext.SaveChanges();
        }
        public Game Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Game> query = dbContext.Games;

            if (useNavigationalProperties)
                query = query.Include(g => g.Users).Include(g => g.Genres);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            Game game = query.FirstOrDefault(g => g.GameId == key);

            if (game == null)
                throw new ArgumentException($"Game with ID = {key} does not exist!");

            return game;
        }

        public List<Game> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Game> query = dbContext.Games;

            if (useNavigationalProperties)
                query = query.Include(g => g.Users).Include(g => g.Genres);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }


        public void Delete(int key)
        {
            Game gameFromDb = Read(key);
            dbContext.Games.Remove(gameFromDb);
            dbContext.SaveChanges();
        }


        public void Update(Game item, bool useNavigationalProperties = false)
        {
            

            List<Genre> genres = new List<Genre>(item.Genres.Count);
            foreach (Genre genre in item.Genres)
            {
                Genre genreFromDb = dbContext.Genres.Find(genre.GenreId);
                if (genreFromDb != null) genres.Add(genreFromDb);
                else genres.Add(genre);
            }
            item.Genres = genres;

            List<Potrebitel> users = new List<Potrebitel>(item.Users.Count);
            foreach (Potrebitel user in item.Users)
            {
                Potrebitel userFromDb = dbContext.Potrebiteli.Find(user.UserId);
                if (userFromDb != null) users.Add(userFromDb);
                else users.Add(user);
            }
            item.Users = users;

            dbContext.Games.Update(item);
            dbContext.SaveChanges();
        }
    }
}
