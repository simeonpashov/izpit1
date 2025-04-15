using System;
using System.Collections.Generic;

namespace DataLayer;

public partial class Genre
{
    public int GenreId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    private Genre()
    {
    }

    public Genre(int genreId, string name)
    {
        GenreId = genreId;
        Name = name;
        Games= new List<Game>();
    }
}
