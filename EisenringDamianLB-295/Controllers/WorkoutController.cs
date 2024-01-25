using Microsoft.AspNetCore.Mvc;
using EisenringDamianLB_295.Context;
using EisenringDamianLB_295.Models;

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


        [HttpGet("GetWorkoutById")]
        public async Task<ActionResult<Workout>> GetWorkoutById(int id)
        {
            var workout = await _context.Workouts.FindAsync(id);

            if (workout == null)
            {
                return NotFound();
            }

            return Ok(workout);
        }
        [HttpGet("GetExerciseById")]
        public async Task<ActionResult<Workout>> GetExerciseById(int id)
        {
            var exercise = await _context.Exercises.FindAsync(id);

            if (exercise == null)
            {
                return NotFound();
            }

            return Ok(exercise);
        }


        [HttpGet("GetWorkoutExerciseById")]
        public async Task<ActionResult<Workout>> GetWorkoutExerciseById(int id)
        {
            var workoutExercises = await _context.WorkoutExercises.FindAsync(id);

            if (workoutExercises == null)
            {
                return NotFound();
            }

            return Ok(workoutExercises);
        }

        [HttpPost("create-workout")]
        public async Task<ActionResult<Workout>> CreateWorkout(Workout workout)
        {
            _context.Workouts.Add(workout);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkoutById), new { id = workout.Id }, workout);
        }

        [HttpPost("create-exercise")]
        public async Task<ActionResult<Exercise>> CreateExercise(Exercise exercise)
        {
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetExerciseById), new { id = exercise.Id }, exercise);
        }

        [HttpPost("add-workout-exercise")]
        public async Task<ActionResult<WorkoutExercise>> AddWorkoutExercise(int workoutId, int exerciseId, WorkoutExercise workoutExercise)
        {
            // Überprüfe, ob Workout und Exercise existieren
            var workout = await _context.Workouts.FindAsync(workoutId);
            var exercise = await _context.Exercises.FindAsync(exerciseId);

            if (workout == null || exercise == null)
            {
                return NotFound("Workout or Exercise not found");
            }

            // Setze Workout und Exercise für das WorkoutExercise-Objekt
            workoutExercise.Workout = workout;
            workoutExercise.Exercise = exercise;

            _context.WorkoutExercises.Add(workoutExercise);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWorkoutExerciseById), new { id = workoutExercise.Id }, workoutExercise);
        }




    }
}
