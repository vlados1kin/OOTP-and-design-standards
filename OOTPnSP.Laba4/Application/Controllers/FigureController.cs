using System.Reflection;
using System.Text.Json;
using Application.Data;
using Application.PluginManager;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using PluginBase;

namespace Application.Controllers;

[ApiController]
[Route("editor")]
public class FigureController : ControllerBase
{
    [HttpGet]
    [Route("save")]
    public IActionResult Save()
    {
        FigureService.SaveChanges();
        return NoContent();
    }
    
    [HttpGet]
    public ActionResult<List<object>> GetAll()
    {
        List<Shape> shapes = FigureService.GetAll();
        List<object> patterns = new List<object>(shapes.Count);
        foreach (Shape shape in shapes)
        {
            try
            {
                Factory factory = shape.GetFactory();
                Figure figure = factory.Create();
                string pattern = factory.Draw(figure);
                patterns.Add(new { id = shape.Id, pattern = pattern });
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"{e.Message}");
            }
            
        }
        return patterns;
    }

    [HttpGet]
    [Route("types")]
    public ActionResult<List<object>> GetTypes()
    {
        List<object> result = new List<object>(FigureService.Types.Count);
        foreach (var pair in FigureService.Types)
        {
            result.Add(new {typeOfFactory = pair.Key, nameOfFactory = pair.Value});
        }
        return result;
    }

    [HttpGet]
    [Route("sign/{sign}")]
    public ActionResult<string> CheckSign(string sign)
    {
        if (FigureService.Dll.ContainsKey(sign))
        {
            Assembly plugin = FigureService.Dll[sign];
            Signature? signature = PluginService.GetSignature(plugin);
            return Ok($"Plugin: {sign}\n" +
                $"--> Name: {signature.Name}\n" +
                $"--> Author: {signature.Author}\n" +
                $"--> Description: {signature.Description}\n" +
                $"--> Assembly name: {plugin.GetName()}\n");
        }
        else
        {
            return Ok("Плагин не найден");
        }
    }

    [HttpGet("{id}")]
    public ActionResult<Shape> Get(int id)
    {
        Shape? shape = FigureService.Get(id);
        return shape == null ? NotFound() : shape;
    }

    [HttpPost]
    public IActionResult Post(Shape shape)
    {
        FigureService.Add(shape);
        Factory factory = shape.GetFactory();
        Figure figure = factory.Create();
        string pattern = factory.Draw(figure);
        return Content(JsonSerializer.Serialize(new { id = shape.Id, pattern }), "application/json");
    }

    [HttpPost]
    [Route("file")]
    public async Task<IActionResult> UploadFile(IFormFile? file)
    {
        if (file != null && file.Length > 0)
        {
            var fileName = Path.GetFileName(file.FileName);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "PluginDll", fileName);
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            Assembly pluginAssembly = PluginService.LoadPlugin(filePath);
            Factory? factory = PluginService.GetFactory(pluginAssembly, new double[5]);
            string typeOfFactory = factory.TypeOfFactory();
            string nameOfFactory = factory.NameOfFactory();
            FigureService.Dll.TryAdd(typeOfFactory, pluginAssembly);
            FigureService.Types.TryAdd(typeOfFactory, nameOfFactory);
            return Ok();
        }
        else
        {
            return BadRequest("File has not been uploaded yet");
        }
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Shape shape)
    {
        if (id != shape.Id)
            return BadRequest();
        Shape? shapeForUpdate = FigureService.Get(id);
        if (shapeForUpdate == null)
            return NotFound();
        FigureService.Update(shape);
        Factory factory = shape.GetFactory();
        Figure figure = factory.Create();
        string pattern = factory.Draw(figure);
        return Content(JsonSerializer.Serialize(new { pattern = pattern }), "application/json");
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) {
        Shape? shape = FigureService.Get(id);
        if (shape == null)
            return NotFound();
        FigureService.Remove(id);
        return NoContent();
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        FigureService.RemoveAll();
        return NoContent();
    }
}