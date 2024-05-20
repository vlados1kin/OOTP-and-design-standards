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
    [HttpPost]
    [Route("settings")]
    public IActionResult Settings(Settings settings)
    {
        FigureService.EncryptionKey = settings.Key;
        return NoContent();
    }
    
    [HttpGet]
    [Route("save")]
    public IActionResult Save()
    {
        FigureService.SaveChanges();
        if (FigureService.EncryptPlugin != null)
        {
            FigureService.EncryptPlugin.EncryptString(FigureService.FilePath, FigureService.EncryptionKey ?? "1111111111111111");
            //FigureService.EncryptPlugin.DecryptString(Path.Combine(Directory.GetCurrentDirectory(), "UploadedFigures", "shapes_encrypted.data"), FigureService.EncryptionKey ?? "1111111111111111", FigureService.Format);
        }

        if (FigureService.ZipperPlugin != null)
        {
            string src = FigureService.FilePath;
            string dest = src.Replace(FigureService.Format == SerializationFormat.Xml ? ".xml" : ".json", ".gz");
            FigureService.ZipperPlugin.CompressAsync(src, dest, FigureService.Format);
            //FigureService.ZipperPlugin.DecompressAsync(dest, FigureService.Format);
        }
        return NoContent();
    }

    [HttpPost]
    [Route("open")]
    public async Task<IActionResult> Open(IFormFile? file)
    {
        if (file != null && file.Length > 0)
        {
            string fileName = Path.GetFileName(file.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFigures", fileName);
            FigureService.FilePath = filePath;
            string newPath;
            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            string extension = Path.GetExtension(filePath);
            if (extension == ".json" || extension == ".xml")
            {
                FigureService.InitList(filePath);
                return Ok();
            }

            if (extension == ".gz")
            {
                if (FigureService.ZipperPlugin != null)
                {
                    FigureService.ZipperPlugin.DecompressAsync(filePath, FigureService.Format);
                    newPath = FigureService.ZipperPlugin.CreateDecompressPath(filePath, FigureService.Format);
                    FigureService.InitList(newPath);
                    return Ok();
                }
                else
                {
                    return NotFound("Плагин для архивации не установлен");
                }
            }

            if (extension == ".data")
            {
                if (FigureService.EncryptPlugin != null)
                {
                    await FigureService.EncryptPlugin.DecryptString(filePath, FigureService.EncryptionKey ?? "1111111111111111", FigureService.Format);
                    newPath = FigureService.EncryptPlugin.CreateDecryptedPath(filePath, FigureService.Format);
                    FigureService.InitList(newPath);
                    return Ok();
                }
                else
                {
                    return NotFound("Плагин для шифрования не установлен");
                }
            }

            return NotFound("Неверный формат плагина или файла");
        }
        else
        {
            return BadRequest("File with figures has not been uploaded yet");
        }
    }
    
    [HttpGet]
    public ActionResult<List<object>> GetAll()
    {
        List<Shape>? shapes = FigureService.GetAll();
        if (shapes != null)
        {
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

        return new List<object>();
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
            if (factory != null)
            {
                string typeOfFactory = factory.TypeOfFactory();
                string nameOfFactory = factory.NameOfFactory();
                FigureService.Dll.TryAdd(typeOfFactory, pluginAssembly);
                FigureService.Types.TryAdd(typeOfFactory, nameOfFactory);
            }

            IEncryptor? encryptor = PluginService.GetEncryptor(pluginAssembly);
            if (encryptor != null)
            {
                FigureService.EncryptPlugin = encryptor;
            }

            IZipper? zipper = PluginService.GetZipper(pluginAssembly);
            if (zipper != null)
            {
                FigureService.ZipperPlugin = zipper;
            }
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