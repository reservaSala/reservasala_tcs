using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ReservaSala.App_Start;
using ReservaSala.Models;

namespace ReservaSala.Controllers
{
    public class UsuarioSalasController : Controller
    {
        private ReservaSalaContext db = new ReservaSalaContext();

        // GET: UsuarioSalas
        public ActionResult Index()
        {
            return View(db.UsuarioSalas.ToList());
        }

        // GET: UsuarioSalas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UsuarioSala usuarioSala = db.UsuarioSalas.Find(id);
            if (usuarioSala == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSala);
        }

        // GET: UsuarioSalas/Edit/5
        //[AuthorizationFilter("Administrador")]
        public ActionResult Edit(int id)
        {
            UsuarioSala usuarioSalaAcesso = db.UsuarioSalas.Find(id);
            if (usuarioSalaAcesso.idUsuSala == Convert.ToInt16(Session["UserID"].ToString()) || Session["Perfil"].ToString() == "Administrador")
            {
                //usuarioSalaAcesso.perfilUsuario = "Usuário";
                usuarioSalaAcesso.acessoUsu = true;

                List<SelectListItem> listItems = new List<SelectListItem>();
                listItems.Add(new SelectListItem
                {
                    Text = "Administrador",
                    Value = "Administrador"
                });
                listItems.Add(new SelectListItem
                {
                    Text = "Usuário",
                    Value = "Usuário"
                });

                ViewBag.Perfis = listItems;

                List<SelectListItem> listaPergunta_um = new List<SelectListItem>();
                listaPergunta_um.Add(new SelectListItem
                {
                    Text = "Nome do animal de estimação?",
                    Value = "Nome do animal de estimação"
                });
                listaPergunta_um.Add(new SelectListItem
                {
                    Text = "Cidade onde nasceu?",
                    Value = "Cidade onde nasceu"
                });
                listaPergunta_um.Add(new SelectListItem
                {
                    Text = "Heroi preferido na infância?",
                    Value = "Heroi preferido na infância"
                });

                ViewBag.PergUm = listaPergunta_um;

                List<SelectListItem> listaPergunta_dois = new List<SelectListItem>();
                listaPergunta_dois.Add(new SelectListItem
                {
                    Text = "Filme favorito?",
                    Value = "Filme favorito"
                });
                listaPergunta_dois.Add(new SelectListItem
                {
                    Text = "Cidade onde vive atualmente?",
                    Value = "Cidade onde vive atualmente"
                });
                listaPergunta_dois.Add(new SelectListItem
                {
                    Text = "Doce ou salgado preferido?",
                    Value = "Doce ou salgado preferido"
                });

                ViewBag.PergDois = listaPergunta_dois;

                if (usuarioSalaAcesso == null)
                {
                    return HttpNotFound();
                }

                if (Session["Perfil"].ToString() == "Administrador")
                {
                    usuarioSalaAcesso.emailUsuario = usuarioSalaAcesso.emailUsuario.Substring(0, usuarioSalaAcesso.emailUsuario.IndexOf('@'));
                }

                usuarioSalaAcesso.respostaSeguranca1 = usuarioSalaAcesso.respostaSeguranca1.ToString();
                usuarioSalaAcesso.respostaSeguranca2 = usuarioSalaAcesso.respostaSeguranca2.ToString();
                usuarioSalaAcesso.pwdUsuario = "";
                return View(usuarioSalaAcesso);
            }

            TempData["mensagem-title"] = "Você não tem permissão";
            TempData["mensagem-erro"] = "para editar!";
            return RedirectToAction("Index", "ReservaSala");
        }

        // POST: UsuarioSalas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[AuthorizationFilter("Administrador")]
        public ActionResult Edit(UsuarioSala usuarioSala)
        {
            if (usuarioSala.idUsuSala == Convert.ToInt16(Session["UserID"].ToString()) || Session["Perfil"].ToString() == "Administrador")
            {
                usuarioSala.emailUsuario = usuarioSala.emailUsuario.Substring(0, usuarioSala.emailUsuario.IndexOf('@'));
                usuarioSala.emailUsuario = usuarioSala.emailUsuario + "@tcs.com";

                if (ModelState.IsValid)
                {
                    usuarioSala.pwdUsuario = CriptografarSenha(usuarioSala.pwdUsuario);
                    db.Entry(usuarioSala).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["mensagem-title"] = "Usuário editado com sucesso";
                    TempData["mensagem"] = "Tudo certo";
                    return RedirectToAction("Index");
                }
                TempData["mensagem-title"] = "Usuario não pode ser editado";
                TempData["mensagem-erro"] = "Ooppss...!";
                return RedirectToAction("Edit", usuarioSala);
            }
            else
            {
                TempData["mensagem-title"] = "Você não tem permissão";
                TempData["mensagem-erro"] = "para editar!";
                return RedirectToAction("Index", "ReservaSala");

            }
        }

        // GET: UsuarioSalas/Delete/5
        [AuthorizationFilter("Administrador")]
        public ActionResult Delete(int id)
        {
            UsuarioSala usuarioSala = db.UsuarioSalas.Find(id);
            if (usuarioSala == null)
            {
                return HttpNotFound();
            }
            return View(usuarioSala);
        }

        // POST: UsuarioSalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter("Administrador")]
        public ActionResult DeleteConfirmed(int id)
        {
            UsuarioSala usuarioSala = db.UsuarioSalas.Find(id);
            db.UsuarioSalas.Remove(usuarioSala);
            db.SaveChanges();
            TempData["mensagem-title"] = "Usuario deletado com sucesso";
            TempData["mensagem"] = "Tudo certo!";
            return RedirectToAction("Index");
        }

        //valida se usuario com o mesmo id já existe
        public bool validaExiste(string idTcsUsuario)
        {
            return db.UsuarioSalas.Any(x => x.idTcsUsuario.Contains(idTcsUsuario));
        }

        //delete em modal
        public ActionResult _modalDelete(int id)
        {
            return RedirectToAction("Delete", "UsuarioSalas", new { id });
        }

        //Detalhes Usuario
        [HttpPost]
        public JsonResult DetalhesUsuario(int IdUser)
        {
            UsuarioSala user = new UsuarioSala();

            if (IdUser > 0)
            {
                user = db.UsuarioSalas.Find(IdUser);
            }

            return Json(user);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login()
        {
            Session["Acesso"] = "login";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UsuarioSala usuarioSalaAcesso)
        {
            if (verificaUsuarioExiste(usuarioSalaAcesso))
            {
                var usuarioDados = db.UsuarioSalas.Where(x => x.idTcsUsuario == usuarioSalaAcesso.idTcsUsuario).First();
                if (validaAcesso(usuarioSalaAcesso))
                {
                    Session["UserID"] = usuarioDados.idUsuSala.ToString();
                    Session["UserName"] = usuarioDados.nomeUsuario.ToString();
                    Session["Perfil"] = usuarioDados.perfilUsuario.ToString();
                    Session["TcsID"] = usuarioDados.idTcsUsuario.ToString();
                    if (!usuarioDados.acessoUsu)
                    {
                        Session["Acesso"] = "false";
                        return RedirectToAction("NovaSenha", "UsuarioSalas", new { id = usuarioDados.idUsuSala });
                    }
                    Session["Acesso"] = "true";
                    return RedirectToAction("UserDashBoard");
                }

                TempData["mensagem-title"] = "Senha fornecida";
                TempData["mensagem-erro"] = "esta incorreta";
                return this.View(usuarioSalaAcesso);
            }
            else
            {
                TempData["mensagem-title"] = "Usuário não existe";
                TempData["mensagem-erro"] = "esta incorreto";
                return this.View(usuarioSalaAcesso);
            }

        }

        [AllowAnonymous]
        public ActionResult UserDashBoard()
        {
            if (Session["UserID"] != null)
            {
                return RedirectToAction("Index", "ReservaSalas");
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        public ActionResult Cadastro()
        {
            UsuarioSala usuarioSalaAcesso = new UsuarioSala();
            usuarioSalaAcesso.perfilUsuario = "Usuário";
            usuarioSalaAcesso.acessoUsu = false;

            List<SelectListItem> listaPergunta_um = new List<SelectListItem>();
            listaPergunta_um.Add(new SelectListItem
            {
                Text = "Nome do animal de estimação?",
                Value = "Nome do animal de estimação"
            });
            listaPergunta_um.Add(new SelectListItem
            {
                Text = "Cidade onde nasceu?",
                Value = "Cidade onde nasceu"
            });
            listaPergunta_um.Add(new SelectListItem
            {
                Text = "Heroi preferido na infância?",
                Value = "Heroi preferido na infância"
            });

            ViewBag.PergUm = listaPergunta_um;

            List<SelectListItem> listaPergunta_dois = new List<SelectListItem>();
            listaPergunta_dois.Add(new SelectListItem
            {
                Text = "Filme favorito?",
                Value = "Filme favorito"
            });
            listaPergunta_dois.Add(new SelectListItem
            {
                Text = "Professor(a) preferido?",
                Value = "Professor(a) preferido"
            });
            listaPergunta_dois.Add(new SelectListItem
            {
                Text = "Doce ou salgado preferido?",
                Value = "Doce ou salgado preferido"
            });

            ViewBag.PergDois = listaPergunta_dois;

            Session["Acesso"] = "false";
            return View(usuarioSalaAcesso);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Cadastro(UsuarioSala usuarioSalaAcesso)
        {
            usuarioSalaAcesso.emailUsuario = usuarioSalaAcesso.emailUsuario + "@tcs.com";
            usuarioSalaAcesso.confirmUsu = false;

            if (!validarExiste(usuarioSalaAcesso))
            { 
                usuarioSalaAcesso.acessoUsu = true;

                if (ModelState.IsValid)
                {
                    usuarioSalaAcesso.pwdUsuario = CriptografarSenha(usuarioSalaAcesso.pwdUsuario);
                    db.UsuarioSalas.Add(usuarioSalaAcesso);
                    db.SaveChanges();
                    TempData["mensagem-title"] = "Usuário cadastrado com sucesso";
                    TempData["mensagem"] = "Tudo certo!";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["mensagem-title"] = "Erro ao cadastrar Usuário";
                    TempData["mensagem-erro"] = "Verifique os dados informados!";
                    return RedirectToAction("Cadastro", usuarioSalaAcesso);
                }
            }
            TempData["mensagem-title"] = "Este usuário já existe";
            TempData["mensagem-erro"] = "Oooppss..!s";
            return RedirectToAction("Cadastro", usuarioSalaAcesso);
        }

        [AllowAnonymous]
        public ActionResult RecuperaSenha(int id)
        {
            var idTcs = id.ToString();
            var resposta = db.UsuarioSalas.Any(x => x.idTcsUsuario == idTcs);

            if (resposta)
            {
                UsuarioSala usuarioSalaAcesso = db.UsuarioSalas.Where(x => x.idTcsUsuario == idTcs).First();
                usuarioSalaAcesso.respostaSeguranca1 = "";
                usuarioSalaAcesso.respostaSeguranca2 = "";
                TempData["mensagem-title"] = "Preencha uma ou mais das perguntas";
                TempData["mensagem"] = "corretamente, assim uma nova senha sera gerada";
                Session["Acesso"] = "false";
                return View(usuarioSalaAcesso);
            }
            else
            {
                TempData["mensagem-title"] = "Este usuário não existe";
                TempData["mensagem-erro"] = "Oooppss..!";
                Session["Acesso"] = "login";
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult RecuperaSenha(UsuarioSala usuarioSalaAcesso)
        {
            var idTcs = usuarioSalaAcesso.idTcsUsuario;
            var dbUsuario = db.UsuarioSalas.Where(x => x.idTcsUsuario == idTcs).First();
            if (dbUsuario.respostaSeguranca1 == usuarioSalaAcesso.respostaSeguranca1 || dbUsuario.respostaSeguranca2 == usuarioSalaAcesso.respostaSeguranca2)
            {
                if (usuarioSalaAcesso.respostaSeguranca1 == null)
                {
                    usuarioSalaAcesso.respostaSeguranca1 = dbUsuario.respostaSeguranca1;
                }
                if (usuarioSalaAcesso.respostaSeguranca2 == null)
                {
                    usuarioSalaAcesso.respostaSeguranca2 = dbUsuario.respostaSeguranca2;
                }
                usuarioSalaAcesso.acessoUsu = false;
                usuarioSalaAcesso.pwdUsuario = GetPassword();
                dbUsuario.pwdUsuario = CriptografarSenha(usuarioSalaAcesso.pwdUsuario);
                dbUsuario.acessoUsu = usuarioSalaAcesso.acessoUsu;
                db.Entry(dbUsuario).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensagem-title"] =  usuarioSalaAcesso.pwdUsuario.ToString();
                TempData["mensagem-info"] = "lembre-se esta sera sua nova senha";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["mensagem-title"] = "As perguntas foram respondidas incorretamente!";
                TempData["mensagem-erro"] = "Errou...!";
                return View(usuarioSalaAcesso);
            }
        }

        //Aqui inicia para gerar uma senha randomica caso entre no esqueceu
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }


        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        [HttpGet]
        public ActionResult NovaSenha(int id)
        {
            UsuarioSala usuarioSalaAcesso = db.UsuarioSalas.Find(id);

            TempData["mensagem-title"] = "Edite a sua nova senha de acesso";
            TempData["mensagem"] = "lembre-se esta sera sua nova senha";
            usuarioSalaAcesso.pwdUsuario = "";
            return View(usuarioSalaAcesso);

        }

        //Aqui inicia a criptografia da senha 
        [AllowAnonymous]
        public string CriptografarSenha(string senha)
        {
            MD5 md5 = MD5.Create();
            var encodedValue = Encoding.UTF8.GetBytes(senha);
            var sb = new StringBuilder();
            var encryptedPassword = md5.ComputeHash(encodedValue);

            foreach (var caracter in encryptedPassword)
            {
                sb.Append(caracter.ToString("X2"));
            }

            return sb.ToString();
        }

        public bool VerificarSenha(string senhaDigitada, string senhaCadastrada)
        {
            MD5 md5 = MD5.Create();
            var encryptedPassword = md5.ComputeHash(Encoding.UTF8.GetBytes(senhaDigitada));

            var sb = new StringBuilder();
            foreach (var caractere in encryptedPassword)
            {
                sb.Append(caractere.ToString("X2"));
            }

            return sb.ToString() == senhaCadastrada;
        }

        [HttpPost]
        public ActionResult NovaSenha(UsuarioSala usuarioSalaAcesso)
        {
            if (ModelState.IsValid)
            {
                usuarioSalaAcesso.acessoUsu = true;
                usuarioSalaAcesso.pwdUsuario = CriptografarSenha(usuarioSalaAcesso.pwdUsuario);
                db.Entry(usuarioSalaAcesso).State = EntityState.Modified;
                db.SaveChanges();
                Session["Acesso"] = "true";
                TempData["mensagem-title"] = "Senha modificada com sucesso";
                TempData["mensagem"] = "Tudo certo";
                return RedirectToAction("Index", "ReservaSalas");
            }
            TempData["mensagem-title"] = "Senha não pode ser modificada";
            TempData["mensagem-erro"] = "Ooppss...!";
            return RedirectToAction("NovaSenha", usuarioSalaAcesso);
        }


        [AllowAnonymous]
        public ActionResult LogOff()
        {
            Session.Clear();

            return this.RedirectToAction("Login", "UsuarioSalas");
        }

        public bool validarExiste(UsuarioSala usuarioSalaAcesso)
        {
            //verifca se usuaria já esta criado
            var verificaUser = db.UsuarioSalas.Any(x => (usuarioSalaAcesso.emailUsuario == x.emailUsuario || usuarioSalaAcesso.idTcsUsuario == x.idTcsUsuario));

            return verificaUser;
        }

        public bool verificaUsuarioExiste(UsuarioSala usuarioSalaAcesso)
        {
            //verifica se usuario que tenta acessar existe 
            var usuarioExiste = db.UsuarioSalas.Any(x => x.idTcsUsuario == usuarioSalaAcesso.idTcsUsuario);

            return usuarioExiste;
        }

        public bool validaAcesso(UsuarioSala usuarioSalaAcesso)
        {
            var idTcs = usuarioSalaAcesso.idTcsUsuario;
            var dbUsuario = db.UsuarioSalas.Where(x => x.idTcsUsuario == idTcs).First();
            var senhaBanco = db.UsuarioSalas.Find(dbUsuario.idUsuSala).pwdUsuario;
            var senhaDigitada = usuarioSalaAcesso.pwdUsuario;

            //verifica senha de acesso       
            var usuario = db.UsuarioSalas.Any(x => (usuarioSalaAcesso.idTcsUsuario == x.idTcsUsuario /*&& usuarioSalaAcesso.pwdUsuario == x.pwdUsuario*/));
            var senha = VerificarSenha(senhaDigitada, senhaBanco);

            if (usuario && senha)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpGet]
        public ActionResult SemAcesso()
        {
            return View();
        }

        //modal Usuario
        public ActionResult _modalUsuario(int id)
        {
            return RedirectToAction("Details", "UsuarioSalas", new { id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
