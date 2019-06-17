using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ReservaSala.Models
{
    [Table("usuarioSala")]
    public class UsuarioSala
    {
        [Key]
        [Display(Name = "Reservante: ")]
        public int idUsuSala { get; set; }

        [Display(Name = "Nome: ")]
        [Required(ErrorMessage = "Para cadastrar um usuario é necessario informar o seu nome")]
        public string nomeUsuario { get; set; }

        [Display(Name = "Id Tcs: ")]
        [Required(ErrorMessage = "É necessario informar um Id")]
        //[StringLength(7, MinimumLength = 7, ErrorMessage = "O id tcs contem somente 7 numeros")]
        public string idTcsUsuario { get; set; }

        [Display(Name = "Senha: ")]
        [Required(ErrorMessage = "Sua senha esta digitada incorretamente")]
        //[StringLength(11, MinimumLength = 7, ErrorMessage = "A senha deve conter entre 7 a 11 digitos")]
        public string pwdUsuario { get; set; }

        [Display(Name = "Perfil:")]
        [Required(ErrorMessage = "O Perfil deve ser escolhido para que possamnos atribuir limites")]
        public string perfilUsuario { get; set; }

        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Este não é um email valido")]
        public string emailUsuario { get; set; }

        [Display(Name = "Pergunta de segurança - 1: ")]
        [Required(ErrorMessage = "Está não é uma pergunta valida")]
        public string perguntaSeguranca1 { get; set; }

        [Display(Name = "Resposta - 1: ")]
        [Required(ErrorMessage = "Está não é uma resposta valida")]
        public string respostaSeguranca1 { get; set; }

        [Display(Name = "Pergunta de segurança - 2: ")]
        [Required(ErrorMessage = "Está não é uma pergunta valida")]
        public string perguntaSeguranca2 { get; set; }

        [Display(Name = "Resposta - 2: ")]
        [Required(ErrorMessage = "Está não é uma resposta valida")]
        public string respostaSeguranca2 { get; set; }

        public bool acessoUsu { get; set; }

        public bool confirmUsu { get; set; }
    }
}