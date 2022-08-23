using GestionUsuarios.Data;
using GestionUsuarios.Models;
using Microsoft.AspNetCore.Mvc;
//Agrego referencias para las cookies y seguridad de acceso
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
//**********

namespace GestionUsuarios.Controllers
{
    public class AccesoController : Controller
    {
        public const string SessionKeyId = "_Id";

        private readonly GestionUsuariosContext _context;
        public AccesoController(GestionUsuariosContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();

        }

        public IActionResult AccesoDenegado()
        {
            return View();

        }

        //Las cookies son asincronicas entonces el metodo es #async Task<"">#
        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {
            var user = ValidarUsuario(_usuario.UsuarioAlias, _usuario.PasswordNavigation.PasswordText);
            if (user != null)
            {


                //Me traigo los roles del usuario
                var query = new List<UsuarioRole>();
                query = _context.UsuarioRoles.Where(s => s.UsuarioId == user.UsuarioId).ToList();
                var rolitos = new List<string>();
                foreach (UsuarioRole Uri in query)
                {
                    string rolcillo = Uri.RoleId.ToString();
                    rolitos.Add(rolcillo);
                }

                //Establezco los datos principales en cookie
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UsuarioNombre),
                    new Claim("UsuarioAlias", user.UsuarioAlias),
                };

                //Establecemos el role del usuario
                //Leemos con un for los roles que tiene el usuario y los almacena en la cookie
                foreach (string rol in rolitos)
                {
                    claims.Add(new Claim(ClaimTypes.Role, rol.ToString()));
                }
                //Guardamos todos los roles
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                //Como es un metodo asincrono debemos ponerlo a esperar
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                //Establezco variable session con Id del usuario
                HttpContext.Session.SetInt32(SessionKeyId, user.UsuarioId);

                //Direccionamos a la pantalla según el rol:
                bool isADMIN = false;
                foreach (string rol in rolitos)
                {
                    if (rol == "1")
                    {
                        isADMIN = true;
                        break;
                    }
                }
                if (isADMIN)
                {
                    return RedirectToAction("Index", "Usuarios");
                }
                else
                {
                    return RedirectToAction("MyAccount", "Usuarios");
                }


            }
            return View();

        }

        public async Task<IActionResult> Salir()
        {
            //eliminamos las cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View("~/Views/Acceso/Login.cshtml");

        }



        public List<Usuario> GetUsers()
        {
            var users = _context.Usuarios.Include(p => p.UsuarioRole).Include(p => p.PasswordNavigation).ToList();
            return users;


        }

        public Usuario ValidarUsuario(string alias, string clave)
        {
            string contrasena;
            var user = GetUsers().Where(item => item.UsuarioAlias == alias).FirstOrDefault();
            if (user != null)
            {
                contrasena = user.PasswordNavigation.PasswordText;
                if (contrasena == clave)
                {
                    return user;
                }
            }
            return null;
        }


    }
}
