using AutoMapper;
using BLLayer.Interface;
using BLLayer.Repository;
using DALayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;
using PLayer.Helper;
using PLayer.Models;

namespace PLayer.Controllers
{
    [Authorize]
    public class MenuItemController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuItem> _logger;

        public MenuItemController(  IUnitOfWork unitOfWork,
                                    IMapper mapper, 
                                    ILogger<MenuItem> logger)
        {
           _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        #region Index
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<MenuItem> items;
            if (SearchValue.IsNullOrEmpty())
                items = await _unitOfWork.MenuRepository.GetAllAsync();
            else
                items = await _unitOfWork.MenuRepository.GetEmployeeByNameAsync(SearchValue);


            var mapItem = _mapper.Map<IEnumerable<MenuItem>, IEnumerable<MenuItemViewModel>>(items);
            return View(mapItem);

        }
        #endregion

        #region Create
        public async Task<IActionResult> Create()
        {

            ViewBag.category = await _unitOfWork.CategoryRepository.GetAllAsync();

            return View(new MenuItemViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create(MenuItemViewModel itemVM)
        {


            if (ModelState.IsValid)
            {
                itemVM.ImageUrl = DocumentSetting.UploadFiles(itemVM.Image, "Images");

                var mapItem = _mapper.Map<MenuItemViewModel, MenuItem>(itemVM);

                await _unitOfWork.MenuRepository.AddAsync(mapItem);
                int result = await _unitOfWork.CompleteAsync();
                if (result > 0)
                    TempData["Message"] = "dish is created";

                return RedirectToAction(nameof(Index));
            }
            ViewBag.category = await _unitOfWork.MenuRepository.GetAllAsync();

            return View(itemVM);
        }
        #endregion

        #region Details
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
                return BadRequest();
            var items = await _unitOfWork.MenuRepository.GetAsync(id.Value);
            if (items is null)
                return NotFound();
            var mapItem = _mapper.Map<MenuItem, MenuItemViewModel>(items);


            return View(viewName, mapItem);
        }
        #endregion

        #region Update
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.category = await _unitOfWork.CategoryRepository.GetAllAsync();

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MenuItemViewModel itemVM, [FromRoute] int? id)
        {
            if (id != itemVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (itemVM.Image is not null)
                        itemVM.ImageUrl = DocumentSetting.UploadFiles(itemVM.Image, "Images");

                    var mapItem = _mapper.Map<MenuItemViewModel, MenuItem>(itemVM);

                    _unitOfWork.MenuRepository.Update(mapItem);
                    int result = await _unitOfWork.CompleteAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return RedirectToAction("Error", "Home");
                }
            }

            return View(itemVM);

        }

        #endregion

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(MenuItemViewModel itemVM, [FromRoute] int? id)
        {
            if (id != itemVM.Id)
                return BadRequest();
            try
            {
                var mapItem = _mapper.Map<MenuItemViewModel, MenuItem>(itemVM);
                _unitOfWork.MenuRepository.Delete(mapItem);
                int result = await _unitOfWork.CompleteAsync();

                if (result > 0 && itemVM.ImageUrl is not null)
                {
                    DocumentSetting.DeleteFile(itemVM.ImageUrl, "Images");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return View(itemVM);
            }
        } 
        #endregion

    }
}
