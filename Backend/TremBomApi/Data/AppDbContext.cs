using Microsoft.EntityFrameworkCore;
using TremBomApi.Models;

namespace TremBomApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<Local> Locais { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Local>().HasData(
        new Local 
        { 
            Id = 101, 
            Nome = "Mercado Central", 
            Categoria = "Edifício Gastronômico",
            ImagemUrl = "https://viajenaweb.com/wp-content/uploads/2016/12/O-que-fazer-no-Mercado-Central-de-Belo-Horizonte-768x432.jpg.webp",
            Descricao = "O clássico: fígado com jiló, queijos premiados e o melhor doce de leite.",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        },
        new Local 
        { 
            Id = 102, 
            Nome = "Mercado Novo", 
            Categoria = "Edifício Gastronômico",
            ImagemUrl = "https://www.hojeemdia.com.br/image/policy:1.992457.1702919772:1702919772/image.jpg?f=2x1&w=1200",
            Descricao = "Cozinha de raiz, bares artesanais e um ambiente retrô industrial incrível.",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        },
        new Local 
        { 
            Id = 103, 
            Nome = "Edifício Maletta", 
            Categoria = "Edifício Gastronômico/Bar",
            ImagemUrl = "https://resize.casapino.com.br/?u=https://cms-bomgourmet.s3.amazonaws.com/bomgourmet/2018/10/201810/maletta-belo-horizonte-varanda-20f288a0.jpg&w=661",
            Descricao = "Varandas icônicas e bares históricos no centro de BH.",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        },
        new Local 
        { 
            Id = 104, 
            Nome = "Bar do Orlando", 
            Categoria = "Bar",
            ImagemUrl = "https://folhadesetelagoas.com/images/noticias/190/9f40c1539471322fcf360b2a9be33a36.jpeg",
            Descricao = "O bar mais antigo de BH, patrimônio do bairro Santa Tereza.",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        },
        new Local 
        { 
            Id = 105, 
            Nome = "Juramento 202", 
            Categoria = "Bar Artesanal",
            ImagemUrl = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/15/04/71/ad/img-20181005-wa0018-largejpg.jpg?w=1000&h=-1&s=1",
            Descricao = "Cervejaria local com clima de calçada e muita mineiridade.",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        },
        new Local 
        { 
            Id = 106, 
            Nome = "Forno da Saudade", 
            Categoria = "Pizarria",
            ImagemUrl = "https://andadireito.com.br/wp-content/uploads/2025/12/Forno-da-Saudade-5.png",
            Descricao = "é uma pizzaria badalada e charmosa, localizada em um casarão dos anos 30 no bairro Carlos Prates, em Belo Horizonte, com vista panorâmica da cidade e ambiente de praça",
            DataCadastro = new DateTime(2026, 4, 22),
            Ativo = true
        }
    );
}
    }
}