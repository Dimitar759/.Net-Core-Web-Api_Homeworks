using DataAccess.Interface;
using Domain;
using Domain.Enums;
using DTOs;
using Services.Interface;

namespace Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;

        public void AddMovie(AddMovieDto addMovieDto)
        {
            // Validation checks
            if (string.IsNullOrEmpty(addMovieDto.Title))
            {
                throw new Exception("Text must not be empty");
            }
            if (addMovieDto.Year <= 0)
            {
                throw new Exception("Year must not be negative");
            }
            if (!string.IsNullOrEmpty(addMovieDto.Description) && addMovieDto.Description.Length > 250)
            {
                throw new Exception("Description cannot be longer than 250 characters");
            }

            Movie newMovie = new Movie
            {
                Title = addMovieDto.Title,
                Year = addMovieDto.Year,
                Description = addMovieDto.Description,
                Genre = addMovieDto.Genre
            };

            // Add the movie to the repository
            _movieRepository.Add(newMovie);
        }

        public void DeleteMovie(int id)
        {
            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new Exception($"Movie with id {id} not found");
            }

            _movieRepository.Delete(movieDb);
        }

        public List<MovieDto> FilterMovies(int? year, GenreEnum? genre)
        {
            if (genre.HasValue)
            {
                // Validate if the value for genre is valid
                var enumValues = Enum.GetValues(typeof(GenreEnum))
                                            .Cast<GenreEnum>()
                                            .ToList();

                if (!enumValues.Contains(genre.Value))
                {
                    throw new Exception("Invalid genre value");
                }
            }

            // Fetch and map the movies to DTOs
            return _movieRepository.FilterMovies(year, genre)
                .Select(movie => new MovieDto
                {
                    Title = movie.Title,
                    Description = movie.Description,
                    Year = movie.Year,
                    Genre = movie.Genre
                })
                .ToList();
        }

        public List<MovieDto> GetAllMovies()
        {
            return _movieRepository.GetAll().Select(movie => new MovieDto
            {
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                Genre = movie.Genre
            })
                .ToList();

        }

        public MovieDto GetMovieById(int id)
        {
            var movieDb = _movieRepository.GetById(id);
            if (movieDb == null)
            {
                throw new Exception($"Movie with id {id} not found");
            }

            MovieDto movie = new MovieDto
            {
                Title = movieDb.Title,
                Year = movieDb.Year,
                Description = movieDb.Description,
                Genre = movieDb.Genre
            };

            return movie;
        }

        public void UpdateMovie(UpdateMovieDto updateMovieDto)
        {
            var movieDb = _movieRepository.GetById(updateMovieDto.Id);

            if (movieDb == null)
            {
                throw new Exception($"Movie with id {updateMovieDto.Id} not found");
            }
            if (string.IsNullOrEmpty(updateMovieDto.Title))
            {
                throw new Exception("Text must not be empty");
            }
            if (updateMovieDto.Year <= 0)
            {
                throw new Exception("Year must not be negative");
            }
            if (!string.IsNullOrEmpty(updateMovieDto.Description) && updateMovieDto.Description.Length > 250)
            {
                throw new Exception("Description can not be longer than 250 characters");
            }

            movieDb.Year = updateMovieDto.Year;
            movieDb.Title = updateMovieDto.Title;
            movieDb.Description = updateMovieDto.Description;
            movieDb.Genre = updateMovieDto.Genre;

            _movieRepository.Update(movieDb);
        }
    }
}
