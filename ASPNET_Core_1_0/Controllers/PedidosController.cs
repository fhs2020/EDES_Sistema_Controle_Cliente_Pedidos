using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASPNET_Core_1_0.Data;
using ASPNET_Core_1_0.Models;

namespace ASPNET_Core_1_0.Controllers
{
    public class PedidosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pedidos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Pedidos.ToListAsync());
        }

        // GET: Pedidos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.Pedidos
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // GET: Pedidos/Create
        public IActionResult Create()
        {
            var clientes = _context.Cliente.ToList();

            var clienteLista = _context.Cliente.AsEnumerable().Select(c => new
            {
                ID = c.ID,
                NomeCliente = string.Format("{0} - {1} - {2} ", c.Empresa, c.Nome, c.Sobrenome)
            }).ToList();

            ViewBag.Clientes = new SelectList(clienteLista, "ID", "NomeCliente");

            return View();
        }

        // POST: Pedidos/Create
        [HttpPost]
        public async Task<IActionResult> Create(Pedidos pedidos)
        {
            var cliente = _context.Cliente.Where(x => x.ID == pedidos.ClienteId).LastOrDefault();

            pedidos.ClienteNome = cliente.Nome + " " + cliente.Sobrenome;

            pedidos.DataPedido = DateTime.Now;

            var produtos = _context.Produto.ToList();

            ViewBag.ListaProdutos = produtos;

            if (ModelState.IsValid)
            {
                _context.Add(pedidos);
                await _context.SaveChangesAsync();

                return RedirectToAction("AddItems", pedidos);
            }
            return View(pedidos);
        }

        [HttpGet]
        public IActionResult FinalizarPedidos()
        {
            //var pedido = _context.Pedidos.Where(x => x.Id == pedidoId).LastOrDefault();


            //var cliente = _context.Cliente.Where(x => x.ID == pedido.ClienteId).LastOrDefault();

            //ViewBag.Cliente = cliente;

            //var items = _context.ItemPedido.Where(x => x.PedidoId == pedidoId).ToList();

            //pedido.ListaItems = new List<ItemPedido>();

            //pedido.ListaItems = items;

            //ViewBag.ListaItems = items;

            return View();

        }

        [HttpPost]
        public IActionResult FinalizarPedidos(int? pedidoId)
        {
            var pedido = _context.Pedidos.Where(x => x.Id == pedidoId).LastOrDefault();


            var cliente = _context.Cliente.Where(x => x.ID == pedido.ClienteId).LastOrDefault();

            ViewBag.Cliente = cliente.Nome;

            var items = _context.ItemPedido.Where(x => x.PedidoId == pedidoId).ToList();

            pedido.ListaItems = new List<ItemPedido>();

            pedido.ListaItems = items;

            ViewBag.ListaItems = items;

            return View(pedido);

        }


        public IActionResult AddItems(Pedidos pedidos)
        {
            var cliente = _context.Cliente.Where(x => x.ID == pedidos.ClienteId).LastOrDefault();

            pedidos.ClienteNome = cliente.Nome + " " + cliente.Sobrenome;

            pedidos.DataPedido = DateTime.Now;

            var produtos = _context.Produto.ToList();

            ViewBag.ListaProdutos = produtos;


            return View(pedidos);

        }


        [HttpPost]
        public async Task<IActionResult> AddItemsToOrder(int pedidoId, int produtoID, int quantidade)
        {

            var pedido = _context.Pedidos.Where(x => x.Id == pedidoId).LastOrDefault();

            var produto = _context.Produto.Where(x => x.ID == produtoID).LastOrDefault();

            var searchPedido = _context.ItemPedido.Where(x => x.PedidoId == pedidoId).ToList();

            var itemPedido = new ItemPedido();


            var updateItem = searchPedido.Where(x => x.ProdutoID == produtoID).LastOrDefault();

            if (updateItem != null)
            {
                updateItem.Quantidade = quantidade;

                _context.Update(updateItem);
                await _context.SaveChangesAsync();
            }
            else
            {
                itemPedido.ProdutoID = produtoID;
                itemPedido.PedidoId = pedidoId;
                itemPedido.Quantidade = quantidade;
                itemPedido.Descricao = produto.Descricao;

                _context.Add(itemPedido);
                await _context.SaveChangesAsync();
            }

            var cliente = _context.Cliente.Where(x => x.ID == pedido.ClienteId).LastOrDefault();

            var produtos = _context.Produto.ToList();

            ViewBag.ListaProdutos = produtos;

            var listaDeItems = _context.ItemPedido.Where(x => x.PedidoId == pedidoId).ToList();

            return Json(listaDeItems);

        }

        [HttpPost]
        public IActionResult OnPageLoad(int pedidoId)
        {
            var pedido = _context.Pedidos.Where(x => x.Id == pedidoId).LastOrDefault();

            var listaDeItems = _context.ItemPedido.Where(x => x.PedidoId == pedidoId).ToList();

            return Json(listaDeItems);

        }


        // GET: Pedidos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.Pedidos.SingleOrDefaultAsync(m => m.Id == id);
            if (pedidos == null)
            {
                return NotFound();
            }
            return View(pedidos);
        }

        // POST: Pedidos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClienteId,ClienteNome,DataPedido")] Pedidos pedidos)
        {
            if (id != pedidos.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pedidos);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PedidosExists(pedidos.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(pedidos);
        }

        // GET: Pedidos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pedidos = await _context.Pedidos
                .SingleOrDefaultAsync(m => m.Id == id);
            if (pedidos == null)
            {
                return NotFound();
            }

            return View(pedidos);
        }

        // POST: Pedidos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedidos = await _context.Pedidos.SingleOrDefaultAsync(m => m.Id == id);
            _context.Pedidos.Remove(pedidos);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool PedidosExists(int id)
        {
            return _context.Pedidos.Any(e => e.Id == id);
        }
    }
}
