﻿using ApiPedidos.Models;
using ConectarDatos;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace ApiPedidos.Controllers
{
    public class LoginController : ApiController
    {


        // POST: api/Login
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult Login(UsuarioLogin usuarioLogin)
        {
            if (usuarioLogin == null)
                return BadRequest("Usuario y Contraseña requeridos.");

            var _userInfo = AutenticarUsuario(usuarioLogin.Usuario, usuarioLogin.Pass);
            if (_userInfo != null)
            {
                return Ok(new { token = GenerarTokenJWT(_userInfo) });
            }
            else
            {
                return Unauthorized();
            }
        }


        // COMPROBAMOS SI EL USUARIO EXISTE EN LA BASE DE DATOS 
        private UsuarioInfo AutenticarUsuario(string username, string password)
        {
            PedidosBDEntities dbContext = new PedidosBDEntities();
            Usuario usuario = null;
            using (PedidosBDEntities pedidosBDEntities = new PedidosBDEntities())
            {
                usuario = pedidosBDEntities.Usuarios.FirstOrDefault(e => e.nUsuario == username && e.Contraseña == password);

            }

            if (usuario == null)
            {
                return null;
            }

            return new UsuarioInfo()
            {
                // Id del Usuario en el Sistema de Información (BD)
                idUsuario = usuario.idUsuario,
                nUsuario = usuario.nUsuario,
                Contraseña = usuario.Contraseña,
                idRol = usuario.idRol
            };


        }

        // GENERAMOS EL TOKEN CON LA INFORMACIÓN DEL USUARIO
        private string GenerarTokenJWT(UsuarioInfo usuarioInfo)
        {
            // RECUPERAMOS LAS VARIABLES DE CONFIGURACIÓN
            var _ClaveSecreta = ConfigurationManager.AppSettings["ClaveSecreta"];
            var _Issuer = ConfigurationManager.AppSettings["Issuer"];
            var _Audience = ConfigurationManager.AppSettings["Audience"];
            if (!Int32.TryParse(ConfigurationManager.AppSettings["Expires"], out int _Expires))
                _Expires = 24;


            // CREAMOS EL HEADER //
            var _symmetricSecurityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_ClaveSecreta));
            var _signingCredentials = new SigningCredentials(
                    _symmetricSecurityKey, SecurityAlgorithms.HmacSha256
                );
            var _Header = new JwtHeader(_signingCredentials);

            // CREAMOS LOS CLAIMS //
            var _Claims = new[] {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.NameId, usuarioInfo.idUsuario.ToString()),
                new Claim("Usuario", usuarioInfo.nUsuario),
                new Claim("Pass", usuarioInfo.Contraseña),
                new Claim(ClaimTypes.Role, usuarioInfo.idRol.ToString()),
            };

            // CREAMOS EL PAYLOAD //
            var _Payload = new JwtPayload(
                    issuer: _Issuer,
                    audience: _Audience,
                    claims: _Claims,
                    notBefore: DateTime.Now,
                    // Exipra a la 24 horas.
                    expires: DateTime.Now.AddHours(_Expires)
                );

            // GENERAMOS EL TOKEN //
            var _Token = new JwtSecurityToken(
                    _Header,
                    _Payload
                );

            return new JwtSecurityTokenHandler().WriteToken(_Token);
        }
    }
}