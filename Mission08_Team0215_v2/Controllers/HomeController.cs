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

    /// ###################################### finish:
    // organization:
    // Index Get and Post
    // add task get and post
    // Edit get and post
    // delete get and post

    //[HttpGet]
    //public IActionResult Index()
    //{
    //    var quadrants = _repo.Quadrants //go to the context file (which is like the C# version of our database), go to the Quadrants table, then use "linq"
    //            .Include(x => x.Category) //a way of joining to the Cat table
    //            .Where(x => x.Completed == false) //only grab tasks that have not been completed
    //            .OrderBy(x => x.Quadrant).ToList(); //this List is what we send to the view


    //    return View(quadrants); //by default, returns the Index view, and then also the quadrants since we specified that
    //}

    //[HttpPost]
    //public IActionResult Index(Quadrant q)
    //{
    //    if (ModelState.IsValid)
    //    {
    //        _repo.
    //        _repo.AddQuadrant(q);
    //    }
    //    return View(new Quadrant());
    //}

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



    ////AddTask get
    //[HttpGet]
    //public IActionResult AddTask()
    //{
    //    ViewBag.Categories = _repo.Categories
    //        .OrderBy(x => x.CategoryName)
    //        .ToList();

    //    return View("AddTask", new Quadrant());
    //}

    ////AddTask post:
    //[HttpPost]
    //public IActionResult AddTask(Quadrant response)
    //{

    //    if (ModelState.IsValid)
    //    {
    //        _repo.AddQuadrant(response); ; //add record to the database and save or commit the changes 

    //        return View("Confirmation", response); //take user to confirmation page
    //    }
    //    else //invalid data was attempted by the user
    //    {
    //        ViewBag.Categories = _repo.Categories
    //        .OrderBy(x => x.CategoryName)
    //        .ToList();

    //        return View(response); //pass them back the data they have already put in
    //    }

    //}

    // AddTask (GET)
    [HttpGet]
    public IActionResult AddTask()
    {
        ViewBag.Categories = _repo.Categories
            .OrderBy(x => x.CategoryName)
            .ToList();

        return View(new Quadrant()); // No need to specify "AddTask" since it matches the method name
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



    //Edit Get
    //[HttpGet]
    //public IActionResult Edit(int id) //This int HAS to be named "id" since that is what the pattern needs
    //{
    //    var recordToEdit = _repo.Quadrants
    //        .Single(x => x.TaskId == id); //use .Single instead of .Where so that we only are returned one record only

    //    ViewBag.Categories = _repo.Categories
    //        .OrderBy(x => x.CategoryName)
    //        .ToList();

    //    return View("AddTask", recordToEdit);
    //}

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
            //_repo.Update(updatedRecord);
            //_repo.SaveChanges(); //do we change these two lines to being in the IQuadrantsRepository file? Ex:
            //public void UpdateManager(Manager manager)
            //    {
            //        _context.Update(manager);
            //        _context.SaveChanges();
            //    }



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


    ////Delete Get
    //[HttpGet]
    //public IActionResult Delete(int id)
    //{
    //    var recordToDelete = _repo.Quadrants
    //        .Single(x => x.TaskId == id);

    //    return View(recordToDelete);
    //}

    ////Delete Post
    //[HttpPost]
    //public IActionResult Delete(Quadrant app)
    //{
    //    _repo.Quadrants.Remove(app);
    //    _repo.SaveChanges();

    //    return RedirectToAction("Index");
    //}

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

    // Delete (POST) - Actually delete the record
    //[HttpPost]
    //public IActionResult DeleteConfirmed(int id) 
    //{
    //    var recordToDelete = _repo.Quadrants
    //        .SingleOrDefault(x => x.TaskId == id);

    //        _repo.DeleteQuadrant(recordToDelete); // Use repository method


    //    return RedirectToAction("Index"); // Redirect to main page
    //}

    [HttpPost]
    public IActionResult Delete(Quadrant record)
    {

        _repo.DeleteQuadrant(record); // Delete from repository

        return RedirectToAction("Index"); // Redirect after deletion
    }



}
