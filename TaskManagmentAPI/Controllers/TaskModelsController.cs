﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using TaskManagmentAPI.Database;
using TaskManagmentAPI.Model;

namespace TaskManagmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskModelsController : ControllerBase
    {
        private readonly SqlConnectionDB _context;

        public TaskModelsController(SqlConnectionDB context)
        {
            _context = context;
        }

        // GET: api/TaskModels
        [HttpGet]
        [EnableRateLimiting("FixedWindowPolicy")]//Endpoint call Rate limiter
        public async Task<ActionResult<IEnumerable<TaskModel>>> GetTasks()
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            return await _context.Tasks.ToListAsync();
        }

        // GET: api/TaskModels/5
        [HttpGet("{id}")]
        [EnableRateLimiting("TokenBucketPolicy")] //Endpoint call Rate limiter

        public async Task<ActionResult<TaskModel>> GetTaskModel(int id)
        {
          if (_context.Tasks == null)
          {
              return NotFound();
          }
            var taskModel = await _context.Tasks.FindAsync(id);

            if (taskModel == null)
            {
                return NotFound();
            }

            return taskModel;
        }

        // PUT: api/TaskModels/5
       
        [HttpPut("{id}")]
        [EnableRateLimiting("ConcurrentWindowPolicy")] //Endpoint call Rate limiter

        public async Task<IActionResult> PutTaskModel(int id, TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return BadRequest();
            }

            _context.Entry(taskModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TaskModels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [EnableRateLimiting("ConcurrentWindowPolicy")] //Endpoint call Rate limiter

        public async Task<ActionResult<TaskModel>> PostTaskModel(TaskModel taskModel)
        {
          if (_context.Tasks == null)
          {
              return Problem("Entity set 'SqlConnectionDB.Tasks'  is null.");
          }
            _context.Tasks.Add(taskModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTaskModel", new { id = taskModel.Id }, taskModel);
        }

        // DELETE: api/TaskModels/5
        [HttpDelete("{id}")]
        [EnableRateLimiting("ConcurrentWindowPolicy")] //Endpoint call Rate limiter

        public async Task<IActionResult> DeleteTaskModel(int id)
        {
            if (_context.Tasks == null)
            {
                return NotFound();
            }
            var taskModel = await _context.Tasks.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(taskModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskModelExists(int id)
        {
            return (_context.Tasks?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
