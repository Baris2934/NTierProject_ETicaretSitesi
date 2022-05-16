using Project.BLL.DesignPatterns.GenericRepository.ConcRep;
using Project.DTO.DTOClasses;
using Project.DTO.ExternalDTOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.API.Controllers
{
    public class CategoryController : ApiController
    {
        CategoryRepository _cRep;

        public CategoryController()
        {
            _cRep = new CategoryRepository();
        }

        [HttpGet]
        public List<ExternalCategoryDTO> ListCategories()  // Herhangi bir yere açacağımız bilgileri bu şekilde tanımlıyoruz. ExternalDTO'yu bu yüzden kullandık.
        {
            return _cRep.Select(x => new ExternalCategoryDTO
            {
                CategoryName = x.CategoryName,
                Description = x.Description

            }).ToList();
        }

        [HttpGet]
        public List<BaseEntityDTO> ListCategoriesForAdmin() // Admin'e açacağımız bilgileri bu şekilde tanımlıyoruz. Normal DTO Classlarından yardım aldık.
        {
            return _cRep.SelectByDTO(x => new CategoryDTO
            {
                CategoryName = x.CategoryName,
                ID = x.ID,
                Description = x.Description
            }).ToList();
        }
    }
}
