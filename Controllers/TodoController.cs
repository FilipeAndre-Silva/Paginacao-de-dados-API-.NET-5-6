using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paginacao.Data;
using Paginacao.Models;

namespace Paginacao.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [HttpGet("load")]
        public async Task<IActionResult> LoadAsync([FromServices]AppDbContext context)
        {
            for(var i = 0; i < 1348; i++)
            {
                var todo = new Todo()
                {
                    Id = i + 1,
                    Done = false,
                    CreatedAt = DateTime.Now,
                    Title = $"Tarefa {i}"
                };

                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
            }

            return Ok();
        }
    
        [HttpGet]
        public async Task<IActionResult> GetAsync([FromServices]AppDbContext context,
                                                  [FromQuery]int skip = 0,
                                                  [FromQuery]int take = 25)
        {
            var todos = await context.Todos
                                     .AsNoTracking()
                                     .Skip(skip)
                                     .Take(take)
                                     .ToListAsync();
            return Ok(todos);
        }

        [HttpGet("Route/{skip:int}/{take:int}")]
        public async Task<IActionResult> GetFromRouteAsync([FromServices]AppDbContext context,
                                                  [FromRoute]int skip = 0,
                                                  [FromRoute]int take = 25)
        {
            var total = await context.Todos.CountAsync();
            var todos = await context.Todos
                                     .AsNoTracking()
                                     .Skip(skip)
                                     .Take(take)
                                     .ToListAsync();
            return Ok(new 
            {
                total,
                skip,
                take,
                data = todos
            });
        }
    }
}