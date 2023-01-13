﻿using hosman_api.Interface;
using hosman_api.Models;
using Microsoft.AspNetCore.Mvc;

namespace hosman_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhongController : ControllerBase
    {
        private readonly IPhongRepository _repo;
        public PhongController(IPhongRepository repo)
        {
            _repo = repo;
        }
        [HttpGet]
        public IActionResult GetAllItems()
        {
            try
            {
                return Ok(_repo.GetAllItems());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("{maPhong}")]
        public IActionResult GetItemByID(string maPhong)
        {
            PhongModel item = _repo.GetItemByID(maPhong);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public IActionResult PostNewItem(PhongModel newItem)
        {
            try
            {
                //TODO Kiểm tra có phòng nào cùng tên trong cùng khu này hay không
                List<PhongModel> listPhong = _repo.GetPhongByKhuTro(newItem.MaKhuTro);
                foreach(var p in listPhong)
                {
                    if(p.TenPhong == newItem.TenPhong)
                        return BadRequest("Phòng đã trùng tên phòng trước!");
                }    
                newItem.MaPhong = Guid.NewGuid().ToString();
                return _repo.PostNewItem(newItem) ? Ok(newItem) : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{maPhong}")]
        public IActionResult PutItem(string maPhong, PhongModel updateItem)
        {
            //TODO Kiểm tra có phòng nào cùng tên trong cùng khu này hay không
            List<PhongModel> listPhong = _repo.GetPhongByKhuTro(updateItem.MaKhuTro);
            foreach (var p in listPhong)
            {
                if (p.TenPhong == updateItem.TenPhong)
                    return BadRequest("Phòng đã trùng tên phòng trước!");
            }
            if (maPhong != updateItem.MaPhong)
                return NotFound();
            return _repo.PutItem(updateItem) ? Ok() : BadRequest();
        }

        [HttpDelete("{maPhong}")]
        public IActionResult DeleteItem(string maPhong)
        {
            try
            {
                if (_repo.GetItemByID(maPhong) == null)
                    return NotFound();
                return _repo.RemoveItem(maPhong) ? Ok() : BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
