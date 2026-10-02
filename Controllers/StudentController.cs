using Microsoft.AspNetCore.Mvc;
using StudentClient.Models;
using StudentClient.Services;

namespace StudentClient.Controllers;

public class StudentController : Controller
{
    private readonly IStudentApiService _studentApiService;

    public StudentController(IStudentApiService studentApiService)
    {
        _studentApiService = studentApiService;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Error = TempData["Error"];

        try
        {
            return View(await _studentApiService.GetAllAsync());
        }
        catch (Exception)
        {
            ViewBag.Error = "Не удалось загрузить список студентов. Проверьте работу Web API.";
            return View(Enumerable.Empty<Student>());
        }
    }

    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var student = await _studentApiService.GetByIdAsync(id);
            if (student is null)
            {
                ViewBag.Error = "Студент с указанным ID не найден.";
            }

            return View(student);
        }
        catch (Exception)
        {
            ViewBag.Error = "Не удалось загрузить студента. Проверьте работу Web API.";
            return View((Student?)null);
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Student());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Student student)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Проверьте введённые данные.";
            return View(student);
        }

        try
        {
            if (await _studentApiService.CreateAsync(student))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Не удалось добавить студента. Проверьте данные и работу Web API.";
        }
        catch (Exception)
        {
            ViewBag.Error = "Не удалось добавить студента. Проверьте работу Web API.";
        }

        return View(student);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var student = await _studentApiService.GetByIdAsync(id);
            if (student is null)
            {
                ViewBag.Error = "Студент с указанным ID не найден.";
            }

            return View(student);
        }
        catch (Exception)
        {
            ViewBag.Error = "Не удалось загрузить студента. Проверьте работу Web API.";
            return View((Student?)null);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Student student)
    {
        student.Id = id;

        if (!ModelState.IsValid)
        {
            ViewBag.Error = "Проверьте введённые данные.";
            return View(student);
        }

        try
        {
            if (await _studentApiService.UpdateAsync(student))
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Не удалось изменить студента. Проверьте данные и работу Web API.";
        }
        catch (Exception)
        {
            ViewBag.Error = "Не удалось изменить студента. Проверьте работу Web API.";
        }

        return View(student);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (!await _studentApiService.DeleteAsync(id))
            {
                TempData["Error"] = "Не удалось удалить студента. Проверьте работу Web API.";
            }
        }
        catch (Exception)
        {
            TempData["Error"] = "Не удалось удалить студента. Проверьте работу Web API.";
        }

        return RedirectToAction(nameof(Index));
    }
}
