using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mission08_Team0215_v2.Models;

namespace Mission08_Team0215_v2.Controllers;

public class HomeController : Controller
{
    private IQuadrantRepository _repo;

    public HomeController(IQuadrantRepository temp)
    {
        _repo = temp; //so we can access the repo instead of the actual context file itself
    }

    // organization:
    // Index Get and Post
    // Add task get and post
    // Edit get and post
    // Delete get and post


    [HttpGet]
    public IActionResult Index()
    {
        var quadrants = _repo.Quadrants 
            .Include(x => x.Category) // joins Category table
            .Where(x => !x.Completed) // Filter uncompleted tasks
            .OrderBy(x => x.QuadrantNum) // Order correctly
            .ToList(); // Execute query and fetch data

        return View(quadrants); // Pass the data to the view
    }

    [HttpPost]
    public IActionResult Index(Quadrant q)
    {
        if (ModelState.IsValid)
        {
            _repo.AddQuadrant(q); 
            return RedirectToAction("Index"); // Redirect to refresh the view after adding
        }

        return View(q); // Return the model with validation errors
    }



    // AddTask (GET)
    [HttpGet]
    public IActionResult AddTask()
    {
        ViewBag.Categories = _repo.Categories
            .OrderBy(x => x.CategoryName)
            .ToList();

        return View(new Quadrant()); 
    }

    // AddTask (POST)
    [HttpPost]
    public IActionResult AddTask(Quadrant response)
    {
        if (ModelState.IsValid)
        {
            _repo.AddQuadrant(response); // Add to the database
            return View("Confirmation"); // Redirect to Confirmation page
        }
        else
        {
            ViewBag.Categories = _repo.Categories
                .OrderBy(x => x.CategoryName)
                .ToList();

            return View(response); // Return the form with validation errors
        }
    }



    [HttpGet]
    public IActionResult Edit(int id)
    {
        var recordToEdit = _repo.GetQuadrantById(id); // Use the repository method

        if (recordToEdit == null)
        {
            return NotFound(); // Handle case where ID does not exist
        }

        ViewBag.Categories = _repo.GetCategories() // Use a method instead of accessing _repo.Categories directly
            .OrderBy(x => x.CategoryName)
            .ToList();

        return View("AddTask", recordToEdit);
    }

    //Edit Post
    [HttpPost]
    public IActionResult Edit(Quadrant updatedRecord)
    {

        if (ModelState.IsValid)
        {
            _repo.UpdateQuadrant(updatedRecord);
           

            //IMPORTANT: this is calling a VIEW, NOT an action, so we do this instead:
            return RedirectToAction("Index");
        }
        else //invalid data was attempted by the user
        {
            ViewBag.Categories = _repo.Categories
            .OrderBy(x => x.CategoryName)
            .ToList();

            return View("AddTask", updatedRecord); //pass them back the data they have already put in
        }
    }


    // Delete (GET) - Confirm before deleting
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var recordToDelete = _repo.Quadrants
            .SingleOrDefault(x => x.TaskId == id);

        if (recordToDelete == null)
        {
            return NotFound(); // Handle case where TaskId is invalid
        }

        return View(recordToDelete);
    }


    [HttpPost]
    public IActionResult Delete(Quadrant record)
    {

        _repo.DeleteQuadrant(record); // Delete from repository

        return RedirectToAction("Index"); // Redirect after deletion
    }



}
