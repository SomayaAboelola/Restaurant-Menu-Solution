using AutoMapper;
using BLLayer.Interface;
using DALayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PLayer.Models;

namespace PLayer.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<Category> _logger;
        public CategoryController(  IUnitOfWork unitOfWork,
                                    IMapper mapper,
                                    ILogger<Category> logger)

        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        #region Index
        public async Task<IActionResult> Index()
        {
            var category = await _unitOfWork.CategoryRepository.GetAllAsync();

            var mapCategory = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryViewModel>>(category);

            return View(mapCategory);
        }
        #endregion
        #region Create
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryVM)
        {
            if (ModelState.IsValid)
            {
                var mapCategory = _mapper.Map<CategoryViewModel, Category>(categoryVM);
                await _unitOfWork.CategoryRepository.AddAsync(mapCategory);
                int result = await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoryVM);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var category = await _unitOfWork.CategoryRepository.GetAsync(id.Value);
            if (category is null)
                return NotFound();
            var mapCategory = _mapper.Map<Category, CategoryViewModel>(category);

            return View(viewName, mapCategory);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Edit(int? id)
        {
            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryViewModel categoryVM, [FromRoute] int? id)
        {
            if (id != categoryVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var mapCategory = _mapper.Map<CategoryViewModel, Category>(categoryVM);

                    _unitOfWork.CategoryRepository.Update(mapCategory);
                    int result = await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(categoryVM);

        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(CategoryViewModel categoryVM, [FromRoute] int? id)
        {
            if (id != categoryVM.Id)
                return BadRequest();
            try
            {
                var mapCategory = _mapper.Map<CategoryViewModel, Category>(categoryVM);

                _unitOfWork.CategoryRepository.Delete(mapCategory);
                int result = await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(categoryVM);
            }
        } 
        #endregion

    }
}
