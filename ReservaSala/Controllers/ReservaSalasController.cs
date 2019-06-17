using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReservaSala.App_Start;
using ReservaSala.Models;

namespace ReservaSala.Controllers
{
    public class ReservaSalasController : Controller
    {
        private Models.ReservaSalaContext db = new Models.ReservaSalaContext();

        public ActionResult Index()
        {           
            ViewBag.Salas = new SelectList(db.Salas.Where(x => x.salaAtiva), "idSala", "nomeSala");
            return View();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservaSala.Models.ReservaSala reservaSala = db.ReservaSalas.Find(id);
            if (reservaSala == null)
            {
                return HttpNotFound();
            }

            return View(reservaSala);
        }

        // GET: ReservaSalas/Create
        public ActionResult Create()
        {
            ViewBag.idSala = new SelectList(db.Salas.Where(x => x.salaAtiva), "idSala", "nomeSala");

            return View();
        }

        public JsonResult RangerHora(ReservaSala.Models.ReservaSala reservaSala)
        {
            var lista = new List<object>();

            foreach (var item in db.ReservaSalas.Where(x => x.reservaDia == reservaSala.reservaDia &&
                                      x.idSala == reservaSala.idSala && x.idResSala != reservaSala.idResSala))
            {
                lista.Add(new
                {
                    HoraInicial = item.reservaHoraIni.ToString(@"hh\:mm"),
                    HoraFinal = item.reservaHoraFim.ToString(@"hh\:mm")
                });
            }

            return Json(lista.ToArray());
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(ReservaSala.Models.ReservaSala reservaSala)
        {
            if (validaHoraMenor(reservaSala))
            {
                if (validaData(reservaSala))
                {
                    reservaSala.dataReservaCriacao = DateTime.Now;
                    reservaSala.idUsuSala = Convert.ToInt16(Session["UserID"]);
                    if (ModelState.IsValid)
                    {
                        db.ReservaSalas.Add(reservaSala);
                        db.SaveChanges();
                        TempData["mensagem-title"] = "Sala reservada com sucesso";
                        TempData["mensagem"] = "Tudo certo!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["mensagem-title"] = "Sala não pode ser reservada!";
                        TempData["mensagem-erro"] = "Oopps, Algo se encontra errado!";
                        ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
                        return View(reservaSala);
                    }
                }
                else
                {
                    TempData["mensagem-title"] = "Sala não pode ser reservada!";
                    TempData["mensagem-erro"] = "Oopps, Algo se encontra errado!";
                    ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
                    return View(reservaSala);
                }

            }

            TempData["mensagem-title"] = "Sala não pode ser reservada!";
            TempData["mensagem-erro"] = "A hora final esta menor que a hora inicial";
            ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
            return View(reservaSala);
        }

        public ActionResult Edit(int? id)
        {
            var sessao = Convert.ToInt16(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservaSala.Models.ReservaSala reservaSala = db.ReservaSalas.Find(id);
            if (reservaSala == null)
            {
                return HttpNotFound();
            }
            if (reservaSala.idUsuSala == Convert.ToInt16(Session["UserID"]))
            {

                ViewBag.idSala = new SelectList(db.Salas.Where(x => x.salaAtiva), "idSala", "nomeSala", reservaSala.idSala);
                return View(reservaSala);
            }
            TempData["mensagem-title"] = "Você não possui acesso para editar esta reserva";
            TempData["mensagem-erro"] = "Sem acesso!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ReservaSala.Models.ReservaSala reservaSala)
        {
            if (reservaSala.idUsuSala == Convert.ToInt16(Session["UserID"]))
            {
                if (validaHoraMenor(reservaSala))
                {
                    if (validaHoraInicial(reservaSala))
                    {
                        if (validaData(reservaSala))
                        {
                            if (ModelState.IsValid)
                            {
                                db.Entry(reservaSala).State = EntityState.Modified;
                                db.SaveChanges();
                                TempData["mensagem-title"] = "Sala reservada editada com sucesso";
                                TempData["mensagem"] = "Tudo certo!";
                                return RedirectToAction("Index");
                            }
                        }
                        else
                        {
                            TempData["mensagem-title"] = "Sala reservada não pode ser editada!";
                            TempData["mensagem-erro"] = "Oopps, Algo se encontra errado!";
                            ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
                            ViewBag.idUsuSala = new SelectList(db.UsuarioSalas, "idUsuSala", "nomeUsuario", reservaSala.idUsuSala);
                            return View(reservaSala);
                        }
                    }
                    else
                    {
                        TempData["mensagem-title"] = "Sala não pode ser reservada!";
                        TempData["mensagem-erro"] = "Oopps, Algo se encontra errado!";
                        ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
                        ViewBag.idUsuSala = new SelectList(db.UsuarioSalas, "idUsuSala", "nomeUsuario", reservaSala.idUsuSala);
                        return View(reservaSala);
                    }
                }
                TempData["mensagem-title"] = "Sala reservada não pode ser editada!";
                TempData["mensagem-erro"] = "A hora Final menor que a hora Inicial";
                ViewBag.idSala = new SelectList(db.Salas, "idSala", "nomeSala", reservaSala.idSala);
                ViewBag.idUsuSala = new SelectList(db.UsuarioSalas, "idUsuSala", "nomeUsuario", reservaSala.idUsuSala);
                return View(reservaSala);
            }

            TempData["mensagem-title"] = "Você não possui acesso para editar essa reserva";
            TempData["mensagem-erro"] = "Sem acesso!";
            return RedirectToAction("Index");
        }

        // GET: ReservaSalas/Delete/5
        public ActionResult Delete(int? id)
        {
            var sessao = Convert.ToInt16(Session["UserID"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReservaSala.Models.ReservaSala reservaSala = db.ReservaSalas.Find(id);
            if (reservaSala == null)
            {
                return HttpNotFound();
            }
            if (reservaSala.idUsuSala == sessao)
            {
                return View(reservaSala);
            }
            TempData["mensagem-title"] = "Você não possui acesso para deletar essa reserva";
            TempData["mensagem-erro"] = "Sem acesso!";
            return RedirectToAction("Index");

        }

        // POST: ReservaSalas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReservaSala.Models.ReservaSala reservaSala = db.ReservaSalas.Find(id);
            if (reservaSala.idUsuSala == Convert.ToInt16(Session["UserID"]))
            {
                db.ReservaSalas.Remove(reservaSala);
                db.SaveChanges();
                TempData["mensagem-title"] = "Sala reservada deletada com sucesso";
                TempData["mensagem"] = "Tudo certo!";
                return RedirectToAction("Index");
            }
            TempData["mensagem-title"] = "Você não possui acesso para deletar essa reserva";
            TempData["mensagem-erro"] = "Sem acesso!";
            return RedirectToAction("Index");
        }

        //Abre a modal de add usuario
        public ActionResult _modalUsuario()
        {
            return RedirectToAction("Create", "UsuarioSalas");
        }

        public ActionResult _modalInformacao(int idSala)
        {
            return RedirectToAction("Details", "Salas");
        }

        //Valida a hora da reserva de acordo com o dia 
        public bool validaData(Models.ReservaSala reservaSala)
        {
            //primeira validação busca a lista de todos no dia de acordo com a sala
            var validacaoLista = db.ReservaSalas.AsNoTracking().Where(x => x.reservaDia == reservaSala.reservaDia && x.idSala == reservaSala.idSala).ToList();

            if (validacaoLista.Count() == 0)
            {
                return true;
            }
            else
            {
                var id = reservaSala.idResSala;
                var edicaoReserva = validacaoLista.Any(y => y.idResSala == id);
                if (edicaoReserva)
                {
                    var encontradoEdiRes = validacaoLista.Find(r => r.idResSala == id);

                    var horaIniUp = reservaSala.reservaHoraIni;
                    var horaFimUp = reservaSala.reservaHoraFim;

                    var valEdit = validacaoLista.Any(y => (horaIniUp <= y.reservaHoraIni) ||
                                            (horaFimUp >= y.reservaHoraFim));

                    if (valEdit)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                return validacaoLista.Any(x => (reservaSala.reservaHoraFim <= x.reservaHoraIni) ||
                                                   (reservaSala.reservaHoraIni >= x.reservaHoraFim));
            }
        }

        //Valida se a hora final é menor que a hora inicial
        public bool validaHoraMenor(Models.ReservaSala reservaSala)
        {
            return reservaSala.reservaHoraFim > reservaSala.reservaHoraIni;
        }

        //valida se no caso de edição da hora no dia for igual ao dia em que vc esta editando
        public bool validaHoraInicial(Models.ReservaSala reservaSala)
        {
            var dia = reservaSala.reservaDia;
            if (dia.Day == DateTime.Now.Day && dia.Month == DateTime.Now.Month && dia.Year == DateTime.Now.Year)
            {
                var hrAtual = (DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString());
                var hrUsuario = (reservaSala.reservaHoraIni.Hours.ToString() + ":" + reservaSala.reservaHoraIni.Minutes.ToString());
                var compHr = TimeSpan.Parse(hrUsuario) >= TimeSpan.Parse(hrAtual);
                return compHr;
            }

            //retorna true se o dia não igual ao de hoje
            return true;
        }

        [HttpPost]
        public JsonResult ReservasDia(int idSala)
        {
            List<ReservaSala.Models.ReservaSala> reservasala = new List<ReservaSala.Models.ReservaSala>();
            DateTime date = DateTime.Now;
            var shortDate = date.Date;

            if (idSala == 0)
            {
                return Json(null);
            }

            if (idSala > 0)
            {
                //reservasala = db.ReservaSalas.Where(r => r.idSala == idSala).ToList();
                var informacaoReservaSala = db.ReservaSalas.Where(r => r.idSala == idSala && r.reservaDia >= shortDate ).ToList().Select(x => new { id = x.idResSala, title = x.titulo, start = (x.reservaDia + x.reservaHoraIni), end = (x.reservaDia + x.reservaHoraFim), textColor = "white" });
                return Json(informacaoReservaSala);
            }

            return null;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Error500()
        {
            return View();
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
