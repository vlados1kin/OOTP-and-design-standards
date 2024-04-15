using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using OOTPnSP.Laba2.Domain;
using OOTPnSP.Laba2.Factories;
using OOTPnSP.Laba2.Models;
using OOTPnSP.Laba2.Services;

namespace OOTPnSP.Laba2.Controllers;

[ApiController]
[Route("editor")]
public class EditorController : ControllerBase
{
    [HttpGet]
    [Route("save")]
    public IActionResult Save()
    {
        ShapeService.SaveChanges();
        return NoContent();
    }
    
    [HttpGet]
    public ActionResult<List<object>> GetAll()
    {
        List<Shape> shapes = ShapeService.GetAll();
        List<object> patterns = new List<object>(shapes.Count);
        foreach (Shape shape in shapes)
        {
            Factory factory = shape.GetFactory();
            Figure figure = factory.Create();
            string pattern = factory.Draw(figure);
            patterns.Add(new { id = shape.Id, pattern = pattern });
        }
        return patterns;
    }

    [HttpGet("{id}")]
    public ActionResult<Shape> Get(int id)
    {
        Shape? shape = ShapeService.Get(id);
        return shape == null ? NotFound() : shape;
    }

    [HttpPost]
    public IActionResult Post(Shape shape)
    {
        ShapeService.Add(shape);
        Factory factory = shape.GetFactory();
        Figure figure = factory.Create();
        string pattern = factory.Draw(figure);
        return Content(JsonSerializer.Serialize(new { id = shape.Id, pattern }), "application/json");
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Shape shape)
    {
        if (id != shape.Id)
            return BadRequest();
        Shape? shapeForUpdate = ShapeService.Get(id);
        if (shapeForUpdate == null)
            return NotFound();
        ShapeService.Update(shape);
        Factory factory = shape.GetFactory();
        Figure figure = factory.Create();
        string pattern = factory.Draw(figure);
        return Content(JsonSerializer.Serialize(new { pattern = pattern }), "application/json");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        Shape? shape = ShapeService.Get(id);
        if (shape == null)
            return NotFound();
        ShapeService.Remove(id);
        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        ShapeService.RemoveAll();
        return NoContent();
    }
}