using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    internal class PotrebitelContext : IDb<Potrebitel, int>
    {
        private GameDbContext dbContext;

        public PotrebitelContext(GameDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Create(Potrebitel item)
        {
            List<Game> games = new List<Game>(item.Games.Count);
            foreach (Game game in item.Games)
            {
                Game gameFromDb = dbContext.Games.Find(game.GameId);
                if (gameFromDb != null) games.Add(gameFromDb);
                else games.Add(game);
            }
            item.Games = games;

           
            List<Potrebitel> friends = new List<Potrebitel>(item.UserId2s.Count);
            foreach (Potrebitel friend in item.UserId2s)
            {
                Potrebitel friendFromDb = dbContext.Potrebiteli.Find(friend.UserId);
                if (friendFromDb != null) friends.Add(friendFromDb);
                else friends.Add(friend);
            }
            item.UserId2s = friends;

            dbContext.Potrebiteli.Add(item);
            dbContext.SaveChanges();
        }

        

        public Potrebitel Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Potrebitel> query = dbContext.Potrebiteli;

            if (useNavigationalProperties)
                query = query.Include(u => u.Games).Include(u => u.UserId2s);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            Potrebitel user = query.FirstOrDefault(u => u.UserId == key);

            if (user == null)
                throw new ArgumentException($"User with ID = {key} does not exist!");

            return user;
        }

        public List<Potrebitel> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
        {
            IQueryable<Potrebitel> query = dbContext.Potrebiteli;

            if (useNavigationalProperties)
                query = query.Include(u => u.Games).Include(u => u.UserId2s);

            if (isReadOnly)
                query = query.AsNoTrackingWithIdentityResolution();

            return query.ToList();
        }

        public void Update(Potrebitel item, bool useNavigationalProperties = false)
        {
           
            List<Game> games = new List<Game>(item.Games.Count);
            foreach (Game game in item.Games)
            {
                Game gameFromDb = dbContext.Games.Find(game.GameId);
                if (gameFromDb != null) games.Add(gameFromDb);
                else games.Add(game);
            }
            item.Games = games;

            
            List<Potrebitel> friends = new List<Potrebitel>(item.UserId2s.Count);
            foreach (Potrebitel friend in item.UserId2s)
            {
                Potrebitel friendFromDb = dbContext.Potrebiteli.Find(friend.UserId);
                if (friendFromDb != null) friends.Add(friendFromDb);
                else friends.Add(friend);
            }
            item.UserId2s = friends;

            dbContext.Potrebiteli.Update(item);
            dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Potrebitel userFromDb = Read(key);
            dbContext.Potrebiteli.Remove(userFromDb);
            dbContext.SaveChanges();
        }
    }

}

