using Domain.Enums;
using DTOs;
using Services.Interface;

namespace Services.Implementation
{
    public class MovieService : IMovieService
    {
        public void AddMovie(AddMovieDto addMovieDto)
        {
            throw new NotImplementedException();
        }

        public void DeleteMovie(int id)
        {
            throw new NotImplementedException();
        }

        public List<MovieDto> FilterMovies(int? year, GenreEnum? genre)
        {
            throw new NotImplementedException();
        }

        public List<MovieDto> GetAllMovies()
        {
            throw new NotImplementedException();
        }

        public MovieDto GetMovieById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateMovie(UpdateMovieDto updateMovieDto)
        {
            throw new NotImplementedException();
        }
    }
}
