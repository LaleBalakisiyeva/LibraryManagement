using AutoMapper;
using LibraryManagement.Business.DTOs.Category;
using LibraryManagement.Business.Services.Interfaces;
using LibraryManagement.Core.Entities;
using LibraryManagement.DAL.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Business.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache; 
        private const string CacheKey = "all_categories";

        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<CategoryGetDto>> GetAllAsync()
        {
            if (!_cache.TryGetValue(CacheKey, out IEnumerable<CategoryGetDto>? categories))
            {
                var entities = await _unitOfWork.Categories.GetAllAsync();
                categories = _mapper.Map<IEnumerable<CategoryGetDto>>(entities);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(CacheKey, categories, cacheOptions);
            }

            return categories!;
        }

        public async Task<CategoryGetDto> GetByIdAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new Exception("Category not found!");
            return _mapper.Map<CategoryGetDto>(category);
        }

        public async Task CreateAsync(CategoryCreateDto dto)
        {
            var category = _mapper.Map<Category>(dto);
            await _unitOfWork.Categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            _cache.Remove(CacheKey);
        }

        public async Task UpdateAsync(CategoryUpdateDto dto)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(dto.Id);
            if (category == null) throw new Exception("Category not found!");

            _mapper.Map(dto, category);
            _unitOfWork.Categories.Update(category);
            await _unitOfWork.SaveChangesAsync();

            _cache.Remove(CacheKey);
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _unitOfWork.Categories.GetByIdAsync(id);
            if (category == null) throw new Exception("Category not found!");

            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync();

            _cache.Remove(CacheKey);
        }
    }
}
