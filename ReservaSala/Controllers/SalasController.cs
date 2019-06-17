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
    public class SalasController : Controller
    {
        private ReservaSalaContext db = new ReservaSalaContext();

        //Detalhes sala
        [HttpPost]
        public JsonResult DetalhesSala(int IdSala)
        {
            Salas sala = new Salas();

            if (IdSala > 0)
            {
                sala = db.Salas.Find(IdSala);
            }

            return Json(sala);
        } 

        // GET: Salas
        public ActionResult Index()
        {
            return View(db.Salas.ToList());
        }

        // GET: Salas/Details/5
        public ActionResult Details(int id)
        {
            Salas salas = db.Salas.Find(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }

        public ActionResult Detalhes(int id)
        {
            Salas salas = db.Salas.Find(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }


        // GET: Salas/Create
        [AuthorizationFilter("Administrador")]
        public ActionResult Create()
        {
            //Lista de andares
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "S1",
                Value = "S1"
            });
            listItems.Add(new SelectListItem
            {
                Text = "1º Andar",
                Value = "1 andar"
            });
            listItems.Add(new SelectListItem
            {
                Text = "2º Andar",
                Value = "2 Andar"
            });
            listItems.Add(new SelectListItem
            {
                Text = "3º Andar",
                Value = "3 Andar"
            });

            ViewBag.Andares = listItems;

            //Ativo e Inativo
            List<SelectListItem> ativosItem = new List<SelectListItem>();
            ativosItem.Add(new SelectListItem
            {
                Text = "Liberada para uso",
                Value = Convert.ToString(true)
            });
            ativosItem.Add(new SelectListItem
            {
                Text = "Não liberada para uso",
                Value = Convert.ToString(false)
            });

            ViewBag.Ativo = ativosItem;

            return View();
        }

        // POST: Salas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter("Administrador")]
        public ActionResult Create( Salas salas)
        {
            salas.salaAtiva = Convert.ToBoolean(salas.salaAtiva);

            if (ModelState.IsValid)
            {
                db.Salas.Add(salas);
                db.SaveChanges();
                TempData["mensagem-title"] = "Sala salva com sucesso";
                TempData["mensagem"] = "Sala registrada";
                return RedirectToAction("Index");
            }
            TempData["mensagem-title"] = "Opps...!";
            TempData["mensagem-erro"] = "Erro ao salvar a sala, verifique os dados";
            return View(salas);
        }

        // GET: Salas/Edit/5
        [AuthorizationFilter("Administrador")]
        public ActionResult Edit(int id)
        {
            Salas salas = db.Salas.Find(id);

            //Lista de andares
            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem
            {
                Text = "S1",
                Value = "S1"
            });
            listItems.Add(new SelectListItem
            {
                Text = "1º Andar",
                Value = "1º andar"
            });
            listItems.Add(new SelectListItem
            {
                Text = "2º Andar",
                Value = "2º Andar"
            });
            listItems.Add(new SelectListItem
            {
                Text = "3º Andar",
                Value = "3º Andar"
            });

            ViewBag.Andares = listItems;

            //Ativo e Inativo
            List<SelectListItem> ativosItem = new List<SelectListItem>();
            ativosItem.Add(new SelectListItem
            {
                Text = "Liberada para uso",
                Value = Convert.ToString(true)
            });
            ativosItem.Add(new SelectListItem
            {
                Text = "Não liberada para uso",
                Value = Convert.ToString(false)
            });

            ViewBag.Ativo = ativosItem;

            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }

        // POST: Salas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter("Administrador")]
        public ActionResult Edit( Salas salas)
        {
            salas.salaAtiva = Convert.ToBoolean(salas.salaAtiva);
            if (ModelState.IsValid)
            {
                db.Entry(salas).State = EntityState.Modified;
                db.SaveChanges();
                TempData["mensagem-title"] = "Tudo Ok..!";
                TempData["mensagem"] = "Sala editada com sucesso";
                return RedirectToAction("Index");
            }
            TempData["mensagem-title"] = "Oppss....!";
            TempData["mensagem-erro"] = "Erro ao editar a sala, verifique os dados";
            return RedirectToAction("Edit", salas);
        }

        // GET: Salas/Delete/5
        public ActionResult Delete(int id)
        {
            Salas salas = db.Salas.Find(id);
            if (salas == null)
            {
                return HttpNotFound();
            }
            return View(salas);
        }

        // POST: Salas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Salas salas = db.Salas.Find(id);
            db.Salas.Remove(salas);
            db.SaveChanges();
            TempData["mensagem-title"] = "Tudo Ok..!";
            TempData["mensagem"] = "Sala deletada com sucesso";
            return RedirectToAction("Index");
        }

        //modal sala
        public ActionResult _modalSala(int id)
        {
            return RedirectToAction("Details", "Salas", new { id } );
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
