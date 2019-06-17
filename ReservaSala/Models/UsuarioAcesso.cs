using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ReservaSala.Models
{
    public class UsuarioAcesso
    {
        [Key]
        //[Display(Name = "Usuario: ")]
        public int idUsuAces { get; set; }

        [Display(Name = "Id: ")]
        [Required(ErrorMessage = "Um id tcs possui 7 numeros")]
        [StringLength(7, MinimumLength = 7, ErrorMessage = "O id tcs contem somente 7 numeros")]
        public string usuarioAces { get; set; }

        [Display(Name = "Nome: ")]
        [Required(ErrorMessage = "Para cadastrar um usuario é necessario informar o seu nome")]
        public string usuarioNome { get; set; }

        [Display(Name = "Senha: ")]
        [Required(ErrorMessage = "Sua senha esta digitada incorretamente")]
        [StringLength(11, MinimumLength = 7, ErrorMessage = "A senha deve conter entre 7 a 11 digitos")]
        public string pwdUsuarioAces { get; set; }

        [Display(Name = "Perfil:")]
        [Required(ErrorMessage = "O Perfil deve ser escolhido para que possamnos atribuir limites")]
        public string perfilUsuario { get; set; }

        [Display(Name ="Email:")]
        [Required(ErrorMessage = "Este não é um email valido")]
        public string emailUsuario { get; set; }

        public bool acessadoUsu { get; set; }
    }

}