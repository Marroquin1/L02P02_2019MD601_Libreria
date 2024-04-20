using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using L02P02_2019MD601.Models;

namespace L02P02_2019MD601.Controllers
{
    public class LibreriaController : Controller
    {
        private readonly libreriaDbContext _libreriaDbContext;
        public LibreriaController(libreriaDbContext libreriaDbContext)
        {
            _libreriaDbContext = libreriaDbContext;
        }

        public IActionResult Index()
        {
            // Obtener la lista de autores y categorías
            var listaDeAutores = _libreriaDbContext.autores.ToList();
            ViewData["listadoDeAutores"] = new SelectList(listaDeAutores, "id", "autor");

            var listaDeCategorias = _libreriaDbContext.categorias.ToList();
            ViewData["listadoDeCategorias"] = new SelectList(listaDeCategorias, "id", "categoria");

            // Obtener la lista de libros con detalles de autor y categoría
            var listaDeLibros = (from l in _libreriaDbContext.libros
                                 join a in _libreriaDbContext.autores on l.id_autor equals a.id
                                 join c in _libreriaDbContext.categorias on l.id_categoria equals c.id
                                 select new 
                                 {
                                     Nombre = l.nombre,
                                     Descripcion = l.descripcion,
                                     Autor = a.autor,
                                     Categoria = c.categoria,
                                     Precio = l.precio,
                                     Estado = l.estado
                                 }).ToList();
            ViewData["listadoDeLibros"] = listaDeLibros;
            return View();
        }

        [HttpPost]
        public IActionResult CrearLibros(libros nuevoLibro)
        {
            if (ModelState.IsValid)
            {
                _libreriaDbContext.Add(nuevoLibro);
                _libreriaDbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nuevoLibro);
        }
    }
}

