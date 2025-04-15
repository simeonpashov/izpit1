using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Game
{
    public int GameId { get; set; }

    public string Title { get; set; } = null!;

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Potrebitel> Users { get; set; } = new List<Potrebitel>();

    private Game()
    {
    }

    public Game(int gameId, string title)
    {
        GameId = gameId;
        Title = title;
        Genres= new List<Genre>();
        Users= new List<Potrebitel>();
    }
}
