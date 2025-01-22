using DaccApi.Helpers;
using DaccApi.Infrastructure.Dapper;
using DaccApi.Model;
using Microsoft.AspNetCore.Mvc;
using DaccApi

namespace DaccApi.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IRepositoryDapper _repositoryDapper;

        public List<Product> GetProducts()
        {
            var products = new List<Product>();

            try
            {
                // Lógica de obter produtos usando Query do SQL

            }
            catch (Exception ex) {

                throw ex;
            }
        } 
    }
}
