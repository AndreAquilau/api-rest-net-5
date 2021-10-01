using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.DTOs;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private FilmeContext _context;
        private IMapper _mapper;

        public FilmeController(FilmeContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpPost]
        public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
        {
            Filme filme = _mapper.Map<Filme>(filmeDto);

            _context.Filmes.Add(filme);

            _context.SaveChanges();

            return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme);
        }

        [HttpGet]
        public IActionResult RecuperaFilmes()
        {
            return Ok(_context.Filmes);
        }

        [HttpGet("{id}")]
        public IActionResult RecuperaFilmesPorId(int id)
        {
            Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

            if (filme != null)
            {
                ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);

                return Ok(filmeDto);
            }
            return NotFound();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
        {
            try
            {

                var filme = _context.Filmes.Where(item => item.Id == id).FirstOrDefault();
                if (filme == null)
                {
                    return NotFound();
                }
                else
                {
                    _mapper.Map(filmeDto, filme);

                    _context.SaveChanges();

                    return NoContent();

                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFilme(int id)
        {
            try
            {
                var filme = _context.Filmes.FirstOrDefault(item => item.Id == id);
                if (filme == null)
                {
                    return NotFound();
                }
                else
                {
                    _context.Filmes.Remove(filme);
                    _context.SaveChanges();

                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
