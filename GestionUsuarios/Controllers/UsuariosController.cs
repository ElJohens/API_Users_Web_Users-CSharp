using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionUsuarios.Data;
using GestionUsuarios.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
namespace GestionUsuarios.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly GestionUsuariosContext _context;

        public UsuariosController(GestionUsuariosContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "1") ]
        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
              return View(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Roles = "1")]
        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [Authorize(Roles = "1")]
        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioId,UsuarioNombre,UsuarioApellidos,UsuarioIdentificacion,UsuarioCorreo,UsuarioTelefono,UsuarioAlias")] Usuario usuario, int roles, string pass)
        {
            if (ModelState.IsValid)
            {
                //Añadir Usuario
                _context.Add(usuario);
                await _context.SaveChangesAsync();

                //Traerse Id del usuario creado para ligarle roles y contraseña
                var query = (from a in _context.Usuarios
                             where a.UsuarioIdentificacion == usuario.UsuarioIdentificacion
                             select a).FirstOrDefault();

                //Añadir Rol
                var usuarioRole = new UsuarioRole();
                usuarioRole.UsuarioId = query.UsuarioId;
                usuarioRole.RoleId = roles;
                _context.Add(usuarioRole);

                //Añadir Contraseña
                var usuarioPass = new Password();
                usuarioPass.UserdId = query.UsuarioId;
                usuarioPass.PasswordText = pass;
                _context.Add(usuarioPass);

                //Guardar Cambios
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            var UsuarioRole =  _context.UsuarioRoles.Where(p => p.UsuarioId == usuario.UsuarioId).FirstOrDefault();
            usuario.UsuarioRole = UsuarioRole;

            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,UsuarioNombre,UsuarioApellidos,UsuarioIdentificacion,UsuarioCorreo,UsuarioTelefono,UsuarioAlias")] Usuario usuario,int roles)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //Actualizando Datos de usuario
                    _context.Update(usuario);
                    
                    //Actualizando Role del usuario
                    var query = (from a in _context.UsuarioRoles
                                 where a.UsuarioId == usuario.UsuarioId
                                 select a).FirstOrDefault();

                    query.RoleId = roles;

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Usuarios == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UsuarioId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Usuarios == null)
            {
                return Problem("Entity set 'GestionUsuariosContext.Usuarios'  is null.");
            }
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
          return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        public async Task<IActionResult> MyAccount()
        {
            var age = HttpContext.Session.GetInt32("_Id").ToString();

            if (age == null || _context.Usuarios == null)
            {
                return NotFound();
            }


            var usuario = await _context.Usuarios.FindAsync(Int32.Parse(age));
            var UsuarioRole = _context.UsuarioRoles.Where(p => p.UsuarioId == usuario.UsuarioId).FirstOrDefault();
            usuario.UsuarioRole = UsuarioRole;

            var usuarioModificado = new UsuarioModificado();
            usuarioModificado.UsuarioId = usuario.UsuarioId;
            usuarioModificado.UsuarioNombre = usuario.UsuarioNombre;
            usuarioModificado.UsuarioApellidos = usuario.UsuarioApellidos;
            usuarioModificado.UsuarioIdentificacion = usuario.UsuarioIdentificacion;
            usuarioModificado.UsuarioCorreo = usuario.UsuarioCorreo;
            usuarioModificado.UsuarioTelefono = usuario.UsuarioTelefono;
            usuarioModificado.UsuarioAlias = usuario.UsuarioAlias;
            usuarioModificado.UsuarioRole = usuario.UsuarioRole;
            usuarioModificado.PasswordNavigation = usuario.PasswordNavigation;

            //Conectandome a API para traer un dato
            //https://localhost:7064/api/Usuario
            var httpClient = new HttpClient();
            var json = await  httpClient.GetStringAsync("https://localhost:7064/api/Usuario");
            //Pasamos ese string json que nos envía el API en una lista
            //Para esto usamos un paquete, lo instalamos mediante consola: PM> Install-Package Newtonsoft.Json
            //Utilizamos la herramienta:
            var Usuarios = JsonConvert.DeserializeObject<List<Usuario>>(json);
            foreach (var item in Usuarios)
            {
                if (item != null && item.UsuarioId == Int32.Parse(age))
                {
                    usuarioModificado.UsuarioSaludo = "Hola " + item.UsuarioAlias + " ("+item.UsuarioNombre +" "+ item.UsuarioApellidos +")";
                    break;
                }
            }
            //*************************************


            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuarioModificado);
        }



        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMyAccount([Bind("UsuarioId,UsuarioNombre,UsuarioApellidos,UsuarioIdentificacion,UsuarioCorreo,UsuarioTelefono,UsuarioAlias")] Usuario usuario, int roles)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    //Actualizando Datos de usuario
                    _context.Update(usuario);

                    //Actualizando Role del usuario
                    var query = (from a in _context.UsuarioRoles
                                 where a.UsuarioId == usuario.UsuarioId
                                 select a).FirstOrDefault();

                    query.RoleId = roles;

                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        

    }
}
