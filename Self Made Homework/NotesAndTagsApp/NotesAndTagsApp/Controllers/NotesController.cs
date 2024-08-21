using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotesAndTagsApp.DTOs;
using NotesAndTagsApp.Models;

namespace NotesAndTagsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        [HttpGet] //http://localhost:[port]/api/Notes
        public ActionResult<List<NoteDto>> GetAllNotes()
        {
            try
            {
                var notesDb = StaticDb.Notes;

                var notes = notesDb.Select(x => new NoteDto
                {
                    Priority = x.Priority,
                    Text = x.Text,
                    User = $"{x.User.FirstName} {x.User.LastName}",
                    Tags = x.Tags.Select(t => t.Name).ToList()

                }).ToList();

                return Ok(notes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")] //http://localhost:[port]/api/Notes/1
        public ActionResult<NoteDto> GetNoteById(int id)
        {
            try
            {
                if (id < 0)
                {
                    return BadRequest("The index cannot be negative");
                }

                Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id); 

                if (noteDb == null)
                {
                    return NotFound($"Note with id {id} does not exist");
                }

                var noteDto = new NoteDto
                {
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        //http://localhost:[port]/api/Notes/queryString
        //http://localhost:[port]/api/Notes/queryString?id=1
        //Here id is a query param - that means it is not a part of the route, but it can be added to the route
        //the id param here is optional
        [HttpGet("findById")]
        public ActionResult<NoteDto> FindById(int? id)
        {
            try
            {
                if (id == null)
                {
                    return BadRequest("The index is required");
                }

                if (id < 0)
                {
                    return BadRequest("The index cannot be negative");
                }

                Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id); //get from db

                if (noteDb == null)
                {
                    return NotFound($"Note with id {id} does not exist");
                }


                var noteDto = new NoteDto
                {
                    Priority = noteDb.Priority,
                    Text = noteDb.Text,
                    User = $"{noteDb.User.FirstName} {noteDb.User.LastName}",
                    Tags = noteDb.Tags.Select(t => t.Name).ToList()
                };
                return Ok(noteDto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }



        [HttpPost]
        public IActionResult CreatePostNote([FromBody] Note note)
        {
            try
            {
                if(note == null)
                {
                    return BadRequest("Note cannot be null");
                }

                if (string.IsNullOrEmpty(note.Text))
                {
                    return BadRequest("Each note must contain text");
                }

                if (note.Tags == null || note.Tags.Count == 0) //if note.Tags is null or empty list
                {
                    return BadRequest("All notes must have some tags");
                }

                if ((int)note.Priority <= 0 || (int)note.Priority > 3)
                {
                    return BadRequest("Invalid value for priority");
                }

                StaticDb.Notes.Add(note);
                return StatusCode(StatusCodes.Status201Created, "Note created!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("language")]
        //in postman we should send key value pair in Headers tab where the key is language
        public IActionResult GetApplicationLanguageFromHeader([FromHeader] string language)
        {
            return Ok(language);
        }


        [HttpPost("addNote")]

        public IActionResult AddNote([FromBody] AddNoteDto addNoteDto)
        {
            try
            {
                if (addNoteDto == null)
                {
                    return BadRequest("Note cannot be null");
                }

                if (string.IsNullOrEmpty(addNoteDto.Text))
                {
                    return BadRequest("Each note must contain text!");
                }

                User userDb = StaticDb.Users.FirstOrDefault(x => x.Id == addNoteDto.UserId);

                if (userDb == null) //ex. userId = 73
                {
                    return NotFound($"User with id {addNoteDto.UserId} was not found");
                }

                List<Tag> tags = new List<Tag>();
                //1,2
                foreach (var tagId in addNoteDto.TagIds)
                {
                    Tag tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);

                    if (tagId == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found");
                    }

                    tags.Add(tagDb);
                }

                //create
                Note newNote = new Note
                {
                    Id = StaticDb.Notes.LastOrDefault().Id + 1, //we do it like this only because we have not connected to the db yet (it will be genereted there)
                    Text = addNoteDto.Text,
                    Priority = addNoteDto.Priority,
                    User = userDb,
                    UserId = userDb.Id, //addNoteDto.UserId
                    Tags = tags
                };

                StaticDb.Notes.Add(newNote); //write in db

                return StatusCode(StatusCodes.Status201Created, "Note created");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] UpdateNoteDto updateNoteDto)
        {
            try
            {
                //validations
                if (updateNoteDto == null)
                {
                    return BadRequest("Note cannot be null");
                }

                Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == updateNoteDto.Id);

                if (noteDb == null)
                {
                    return NotFound($"Note with id {updateNoteDto.Id} was not found");
                }

                if (string.IsNullOrEmpty(updateNoteDto.Text))
                {
                    return BadRequest("Each note must contain text");
                }

                User userDb = StaticDb.Users.FirstOrDefault(x => x.Id == updateNoteDto.UserId);

                if (userDb == null) //ex. userId = 73
                {
                    return NotFound($"User with id {updateNoteDto.UserId} was not found");
                }

                List<Tag> tags = new List<Tag>();
                //1,2
                foreach (var tagId in updateNoteDto.TagIds)
                {
                    Tag tagDb = StaticDb.Tags.FirstOrDefault(x => x.Id == tagId);

                    if (tagId == null)
                    {
                        return NotFound($"Tag with id {tagId} was not found");
                    }

                    tags.Add(tagDb);
                }

                //upadte
                noteDb.Text = updateNoteDto.Text;
                noteDb.Priority = updateNoteDto.Priority;
                noteDb.User = userDb;
                noteDb.UserId = userDb.Id; //updateNoteDto.UserId
                noteDb.Tags = tags;

                return StatusCode(StatusCodes.Status204NoContent, "Note updated!");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Id has invalid value!");
                }

                Note noteDb = StaticDb.Notes.FirstOrDefault(x => x.Id == id);

                if (noteDb == null)
                {
                    return NotFound($"Note with id {id} was not found");
                }

                StaticDb.Notes.Remove(noteDb);
                return Ok("Note deleted");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
