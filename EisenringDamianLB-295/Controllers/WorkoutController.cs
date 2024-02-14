using Microsoft.AspNetCore.Mvc;
using EisenringDamianLB_295.Context;
using EisenringDamianLB_295.Models;
using Microsoft.EntityFrameworkCore;

namespace EisenringDamianLB_295.Controllers
{
    [ApiController]
    [Route("api/WorkoutController")]
    public class WorkoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkoutController(ApplicationDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetWorkoutById/{id}")]
        public async Task<ActionResult<Workout>> GetWorkoutById(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }

        [HttpGet("GetExerciseById/{id}")]
        public async Task<ActionResult<Exercise>> GetExerciseById(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }

        [HttpGet("GetWorkoutExerciseById")]
        public async Task<ActionResult<WorkoutExercise>> GetWorkoutExerciseById(int id)
        {
            // Suchen Sie in der WorkoutExercise-Tabelle nach der ID, die entweder die WorkoutId oder die ExerciseId enthält
            var workoutExercise = await _context.WorkoutExercises
                                            .FirstOrDefaultAsync(we => we.WorkoutId == id || we.ExerciseId == id);

            if (workoutExercise == null)
            {
                return NotFound();
            }

            return Ok(workoutExercise);
        }


        [HttpPost("CreateWorkout")]
        public async Task<ActionResult<Workout>> CreateWorkout(Workout workout)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkoutById), new { id = workout.Id }, workout);
        }

        [HttpPost("CreateExercise")]
        public async Task<ActionResult<Exercise>> CreateExercise(Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, exercise);
        }

        [HttpPost("AddWorkoutExercise")]
        public async Task<ActionResult<WorkoutExercise>> AddWorkoutExercise(int workoutId, int exerciseId, WorkoutExercise workoutExercise)
        {
            // Überprüfen, ob die Beziehung bereits vorhanden ist
            var existingRelationship = await _context.WorkoutExercises
                .FirstOrDefaultAsync(we => we.WorkoutId == workoutId && we.ExerciseId == exerciseId);

            if (existingRelationship != null)
            {
                // Falls die Beziehung bereits vorhanden ist, geben Sie einen Konfliktstatus zurück
                return Conflict("Relationship already exists");
            }

            // Überprüfen, ob Workout und Exercise existieren
            var workout = await _context.Workouts.FindAsync(workoutId);
            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (workout == null || exercise == null)
            {
                return NotFound("Workout or Exercise not found");
            }

            // Setzen Sie Workout und Exercise für das WorkoutExercise-Objekt
            workoutExercise.Workout = workout;
            workoutExercise.Exercise = exercise;

            _context.WorkoutExercises.Add(workoutExercise);
            await _context.SaveChangesAsync();

           
            return CreatedAtAction(nameof(GetWorkoutExerciseById), new { id = workoutExercise.Id }, workoutExercise);
        }


        [HttpDelete("DeleteWorkout/{id}")]
        public async Task<ActionResult> DeleteWorkout(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();

            return Ok("Deleted Succesfully");
        }

        [HttpDelete("DeleteExercise/{id}")]
        public async Task<ActionResult> DeleteExercise(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();

            return Ok("Deleted Succesfully");
        }

        [HttpDelete("delete-workout-exercise/{id}")]
        public async Task<IActionResult> DeleteWorkoutExercise(int id)
        {
            var workoutExercise = await _context.WorkoutExercises
                                             .FirstOrDefaultAsync(we => we.WorkoutId == id || we.ExerciseId == id);

            if (workoutExercise == null)
            {
                return NotFound();
            }

            _context.WorkoutExercises.Remove(workoutExercise);
            await _context.SaveChangesAsync();

            return Ok("Deleted Succesfully");
        }

        [HttpPut("update-exercise/{id}")]
        public async Task<ActionResult<Exercise>> UpdateExercise(int id, Exercise updatedExercise)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            // Update exercise properties
            exercise.Name = updatedExercise.Name;
            exercise.Category = updatedExercise.Category;

            _context.Entry(exercise).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Updated Succesfully");
        }
        [HttpPut("update-workout/{id}")]
        public async Task<ActionResult<Workout>> UpdateWorkout(int id, Workout updatedWorkout)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            // Update workout properties
            workout.CaloriesBurned = updatedWorkout.CaloriesBurned;
            workout.NamePerson = updatedWorkout.NamePerson;
            workout.Date = updatedWorkout.Date;
            workout.Duration = updatedWorkout.Duration;

            _context.Entry(workout).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Updated Succesfully");
        }
        [HttpPut("update-workout-exercise/{id}")]
        public async Task<ActionResult<WorkoutExercise>> UpdateWorkoutExercise(int id, WorkoutExercise updatedWorkoutExercise)
        {
            var workoutExercise = await _context.WorkoutExercises
                                             .FirstOrDefaultAsync(we => we.WorkoutId == id || we.ExerciseId == id);

            if (workoutExercise == null)
            {
                return NotFound();
            }

            // Update workout exercise properties
            workoutExercise.Sets = updatedWorkoutExercise.Sets;
            workoutExercise.Reps = updatedWorkoutExercise.Reps;
            workoutExercise.Weight = updatedWorkoutExercise.Weight;

            _context.Entry(workoutExercise).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok("Updated Succesfully");
        }
    }
}
