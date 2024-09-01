using Domain.Enums;
using Domain;

namespace DataAccess.Interface
{
    public interface IMovieRepository : IRepository<Movie>
    {
        List<Movie> FilterMovies(int? year, GenreEnum? genre);
    }
}
