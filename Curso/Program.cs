using System;
using System.Collections.Generic;
using System.Linq;
using CursoEFCore.Domain;
using CursoEFCore.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace CursoEFCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // using var db = new Data.ApplicationContext();

            // //Só usar em desenvolvimento
            // //db.Database.Migrate();

            // var existe = db.Database.GetPendingMigrations().Any();

            // if(existe)
            // {
            //     //Regra de acordo com a necessidade
            // }

            //InserirDados();
            //InserirDadosEmMassa();
            //ConsultarDados();
            //CadastrarPedido();
            //ConsultarPedidoCarregamentoAdiantado();
            //AtualizarDados();
            RemoverRegistro();
        }

        private static void RemoverRegistro()
        {
            using var db = new Data.ApplicationContext();

            var cliente = InserirCliente(db);

            db.Clientes.Remove(cliente);

            //db.SaveChanges();
            //db.Remove(cliente);
            db.Entry(cliente).State = EntityState.Deleted;

            Console.WriteLine($"Cliente {cliente.Id} removido");
        }

        private static Cliente InserirCliente(Data.ApplicationContext db)
        {
            var cliente = new Cliente
            {
                Nome = $"Cliente {DateTime.Now}",
                CEP = "99999999",
                Cidade = "Cidade",
                Estado = "UF",
                Telefone= "99999999999"
            };

            db.Add(cliente);

            db.SaveChanges();

            return cliente;
        }

        private static void AtualizarDados()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.Find(1);

            //Atualiza tudo!
            //db.Clientes.Update(cliente);
            //db.SaveChanges();

            //Atualiza só o nome
            //cliente.Nome = $"Cliente Modificado {DateTime.Now} ";
            //db.SaveChanges();

            //Outra forma de atualizar somente alguns campos
            var clienteDesconectado = new
            {
                Nome = "Cliente Desconectado",
                Telefone="6130000000"
            };

            db.Entry(cliente).CurrentValues.SetValues(clienteDesconectado);
            db.SaveChanges();            
        }

        private static void ConsultarPedidoCarregamentoAdiantado()
        {
            using var db = new Data.ApplicationContext();

            var pedidos = db.Pedidos
                            .Include(c=>c.Items)
                            .ThenInclude(c=>c.Produto)
                            .ToList();

            Console.WriteLine(pedidos.Count);
        }

        private static void CadastrarPedido()
        {
            using var db = new Data.ApplicationContext();

            var cliente = db.Clientes.FirstOrDefault();

            var produto = db.Produtos.FirstOrDefault();

            var pedido = new Pedido
            {
                ClienteId = cliente.Id,
                IniciadoEm = DateTime.Now,
                FinalizadoEm = DateTime.Now,
                Observacao = "Pedido Teste",
                Status = StatusPedido.Analise,
                TipoFrete = TipoFrete.SemFrete,
                Items = new List<PedidoItem>
                {
                    new PedidoItem
                    {
                        ProdutoId = produto.Id,
                        Desconto = 0,
                        Quantidade = 1,
                        Valor = 10
                    }
                }
            };

            db.Pedidos.Add(pedido);

            db.SaveChanges();
        }

        private static void InserirDadosEmMassa()
        {
            using var db = new Data.ApplicationContext();

            var produto = new Produto
            {
                Descricao = "Produto de teste",
                CodigoBarras= "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            var cliente = new Cliente
            {
                Nome = "Thiago Borges Vieira",
                CEP = "99999999",
                Cidade = "Valparaíso",
                Estado = "GO",
                Telefone= "61999999999"
            };

            db.AddRange(produto, cliente);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registro(s): {registros}");
        }

        private static void ConsultarDados()
        {
            using var db = new Data.ApplicationContext();

            //var consultaPorSintax = (from c in db.Clientes where c.Id>0 select c).ToList();

            var consultaPorMetodo = db.Clientes.Where(c=>c.Id > 0)
                                        .OrderBy(c=>c.Id)
                                        .ToList();

            foreach(var cliente in consultaPorMetodo)
            {
                Console.WriteLine($"Consultando Cliente: {cliente.Id}");

                //db.Clientes.Find(cliente.Id);
                db.Clientes.FirstOrDefault(c=>c.Id == cliente.Id);
            }
        }

        private static void InserirDados()
        {
            var produto = new Produto
            {
                Descricao = "Produto de teste",
                CodigoBarras= "1234567891231",
                Valor = 10m,
                TipoProduto = TipoProduto.MercadoriaParaRevenda,
                Ativo = true
            };

            using var db = new Data.ApplicationContext();

            //db.Produtos.Add(produto);
            //db.Set<Produto>().Add(produto);
            //db.Entry(produto).State = EntityState.Added;
            db.Add(produto);

            var registros = db.SaveChanges();

            Console.WriteLine($"Total de Registro(s): {registros}");
        }
    }
}
