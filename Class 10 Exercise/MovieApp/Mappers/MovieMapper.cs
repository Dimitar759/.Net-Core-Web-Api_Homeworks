using Domain;
using DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mappers
{
    public static class MovieMapper
    {
        public static Movie ToMovie(this AddMovieDto addMovieDto)
        {
            return new Movie
            {
                Year = addMovieDto.Year,
                Description = addMovieDto.Description,
                Genre = addMovieDto.Genre,
                Title = addMovieDto.Title
            };
        }


        public static MovieDto ToMovieDto(this Movie movie)
        {
            return new MovieDto
            {
                Year = movie.Year,
                Description = movie.Description,
                Genre = movie.Genre.ToString(),
                Title = movie.Title
            };
        }
    }
}
