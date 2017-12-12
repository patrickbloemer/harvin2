﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Harvin.Models;
using Harvin.DAO;

namespace Harvin.Controllers
{
    public class FuncionariosController : Controller
    {
        private Entities db = new Entities();

        // GET: Funcionarios
        public ActionResult Index()
        {
            var funcionarios = db.Funcionarios.Include(f => f.cargo);
            return View(funcionarios.ToList());
        }

        // GET: Funcionarios/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // GET: Funcionarios/Create
        public ActionResult Create()
        {
            ViewBag.cargoId = new SelectList(db.Cargos, "cargoId", "nome");
            return View();
        }

        // POST: Funcionarios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,cargoId,home,verificarConsumo,realizarPedido,pedidosPendentes,clientes,reservarMesa,configuracoes,relatorios,nome,sobrenome,cpf,dataDeNascimento,cep,endereco,complemento,bairro,cidade,email,telefone,senha")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                if(FuncionarioDAO.BuscaFuncionarioPorCPF(funcionario) == null && FuncionarioDAO.BuscaFuncionarioPorEmail(funcionario) == null)
                {
                    db.Funcionarios.Add(funcionario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } else
                {
                    if(FuncionarioDAO.BuscaFuncionarioPorCPF(funcionario) != null) {
                        ModelState.AddModelError("", "Já existe um funcionário cadastrado no sistema com esse CPF!");
                    }
                    if(FuncionarioDAO.BuscaFuncionarioPorEmail(funcionario) != null) {
                        ModelState.AddModelError("", "Já existe um funcionário cadastrado no sistema com esse email!");
                    }

                }
 
            }

            ViewBag.cargoId = new SelectList(db.Cargos, "cargoId", "nome", funcionario.cargoId);
            return View(funcionario);
        }

        // GET: Funcionarios/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            ViewBag.cargoId = new SelectList(db.Cargos, "cargoId", "nome", funcionario.cargoId);
            return View(funcionario);
        }

        // POST: Funcionarios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,cargoId,home,verificarConsumo,realizarPedido,pedidosPendentes,clientes,reservarMesa,configuracoes,relatorios,nome,sobrenome,cpf,dataDeNascimento,cep,endereco,complemento,bairro,cidade,email,telefone,senha")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(funcionario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cargoId = new SelectList(db.Cargos, "cargoId", "nome", funcionario.cargoId);
            return View(funcionario);
        }

        // GET: Funcionarios/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Funcionario funcionario = db.Funcionarios.Find(id);
            if (funcionario == null)
            {
                return HttpNotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Funcionario funcionario = db.Funcionarios.Find(id);
            db.Funcionarios.Remove(funcionario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        // GET: Clinicas/Login
        public ActionResult Login() {
            //ClinicaLoginDAO.NovoGuidPraSessao();
            return View();
        }

        // POST: Clinicas/Login
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "cpf,senha")] Funcionario funcionario) {
            if (ModelState.IsValid) {
                Funcionario f = new Funcionario();
                f = FuncionarioLoginDAO.VerificaLogin(funcionario);
                if (f != null) {
                    FuncionarioLoginDAO.AdicionarFuncionario(f);
                    return RedirectToAction("Index");
                }else {
                    ViewBag.Mensagem = "Login e/ou Senha inválido (s)";
                    return View(funcionario);
                }
            }
            return View(funcionario);
        }
    }
}